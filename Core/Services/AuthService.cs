using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using Models.Entities;
using Models.Entities.Identity;
using Models.Results;
using Persistance.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VModels.DTOS.Auth;

namespace Core.Services;

public class AuthService : IAuthService
{
    #region Fields
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDBContext _applicationDBContext;
    private readonly IUserRoleRepo _userRoleRepo;
    private readonly IUserClassRepo _userClassRepo;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly BaseSettings _baseSettings;
    private readonly IAttachmentService _attachmentService;
    #endregion

    #region Constructors
    public AuthService(JwtSettings jwtSettings,
        UserManager<User> userManager,
        ApplicationDBContext applicationDBContext,
        IUserRoleRepo userRoleRepo,
        IUserClassRepo userClassRepo,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment,
        BaseSettings baseSettings
        , IAttachmentService attachmentService)
    {
        _jwtSettings = jwtSettings;
        _userManager = userManager;
        _applicationDBContext = applicationDBContext;
        _userRoleRepo = userRoleRepo;
        _userClassRepo = userClassRepo;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
        _baseSettings = baseSettings;
        _attachmentService = attachmentService;
    }
    #endregion

    #region Handle Methods
    public async Task<Response<string>> AddAsync(AddUserDto dto)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            return ResponseHandler.BadRequest<string>("Email is already registered!");

        var user = _mapper.Map<User>(dto);
        user.UserName = dto.Email.Split('@')[0];
        user.ProfilePicturePath = "emptyAvatar.png";

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description},";

            return ResponseHandler.BadRequest<string>(errors);
        }

        var createdUser = _userManager.FindByEmailAsync(user.Email!);
        _attachmentService.GenerateQrCode(createdUser.Result!.Id, _webHostEnvironment);

        if (dto.Role.roleId > 0)
        {
            await _userRoleRepo.AddAsync(new()
            {
                UserId = (await _userManager.FindByEmailAsync(dto.Email))!.Id,
                RoleId = dto.Role.roleId,
                OrganizationId = dto.Role.organizationId,
                SchoolId = dto.Role.schoolId,
                ActivityId = dto.Role.activityId
            });
        }

        return ResponseHandler.Success<string>($"{dto.Email} created successfully");
    }

    public async Task<Response<GetUserDto>> ChangeUserPasswordAsync(ChangeUserPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return ResponseHandler.BadRequest<GetUserDto>("Invalid Email...");
        }

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description},";

            return ResponseHandler.BadRequest<GetUserDto>(errors);
        }

        user.PlainPassword = dto.NewPassword;
        var updatedUserResult = await _userManager.UpdateAsync(user);
        if (!updatedUserResult.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in updatedUserResult.Errors)
                errors += $"{error.Description},";

            return ResponseHandler.BadRequest<GetUserDto>(errors);
        }

        if (!string.IsNullOrEmpty(user.ProfilePicturePath))
        {
            user.ProfilePicturePath = $"{_baseSettings.url}/{_baseSettings.usersPath}/{user.ProfilePicturePath}";
        }

        var viewmodel = _mapper.Map<GetUserDto>(user);

        return ResponseHandler.Success<GetUserDto>(viewmodel);
    }

    public async Task<Response<JwtAuthResult>> UpdateAsync(ChangeUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

        if (user == null)
        {
            return ResponseHandler.NotFound<JwtAuthResult>("User not found.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != user.Email)
        {
            var checkUser = await _userManager.FindByEmailAsync(dto.Email);
            if (checkUser is not null)
            {
                return ResponseHandler.BadRequest<JwtAuthResult>("The email already exists.");
            }
            user.Email = dto.Email;
            user.UserName = dto.Email.Split('@')[0];
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return ResponseHandler.BadRequest<JwtAuthResult>(errors);
            }
        }

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return ResponseHandler.BadRequest<JwtAuthResult>(errors);
            }
        }

        return ResponseHandler.Success<JwtAuthResult>(await GetJWTToken(user, Guid.NewGuid()));
    }

    public async Task<Response<JwtAuthResult>> LoginAsync(LoginDto dto)
    {
        var authModel = new JwtAuthResult();

        var user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);

        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return ResponseHandler.NotFound<JwtAuthResult>("Email or Password is incorrect!");
        }

        return ResponseHandler.Success<JwtAuthResult>(await GetJWTToken(user, Guid.NewGuid()));
    }

    public async Task<Response<JwtAuthResult>> LoginByUserNameAsync(LoginDto dto)
    {
        var authModel = new JwtAuthResult();

        var user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);

        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return ResponseHandler.NotFound<JwtAuthResult>("Email or Password is incorrect!");
        }

        return ResponseHandler.Success<JwtAuthResult>(await GetJWTToken(user, Guid.NewGuid()));
    }

    private async Task<JwtAuthResult> GetJWTToken(User user, Guid refreshToken = new Guid())
    {
        var userroles = await _userRoleRepo
            .GetTableNoTracking()
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == user.Id)
            .ToListAsync();

        var (accessToken, expireDate) = await GenerateJWTToken(user);
        var response = CreateJwtResponse(user, refreshToken, userroles, accessToken, expireDate);
        //SetUserInformations(user, response);
        //await SetRoles(userroles, response);
        //await SetClassrooms(user, response);

        var updatedUser = await UpdateAccessAndRefreshToken(user, response);
        if (updatedUser.Entity.Id != user.Id)
        {
            return new JwtAuthResult { IsAuthenticated = false };
        }
        return response;
    }

    private JwtAuthResult CreateJwtResponse(User user, Guid refreshToken, List<UserRole> userroles, string accessToken, DateTime expireDate)
    {
        return new JwtAuthResult
        {
            Id = user.Id,
            AccessToken = accessToken,
            AccessTokenExpiryDate = expireDate,
            RefreshToken = refreshToken,
            RefreshTokenExpiryDate = DateTime.Now.AddMinutes(_jwtSettings.RefreshTokenExpireDate),
            IsAuthenticated = true,
            IsSuperAdmin = userroles.Any(r => r.Role!.Name == "SuperAdmin"),
        };
    }

    #region SetUserInformations&SetRoles& SetClassrooms
    /*
    private void SetUserInformations(User user, JwtAuthResult response)
    {
        response.UserInformations.Name = user.Name;
        response.UserInformations.Email = user.Email!;
        response.UserInformations.Birthdate = user.Birthdate;
        response.UserInformations.Address = user.Address;
        response.UserInformations.PhoneNumber = user.PhoneNumber!;
        response.UserInformations.UserName = user.UserName!;
        response.UserInformations.ProfilePicturePath = $"{_baseSettings.url}/{_baseSettings.usersPath}/{user.ProfilePicturePath}";
        response.UserInformations.FatherMobile = user.FatherMobile;
        response.UserInformations.MotherMobile = user.MotherMobile;
        response.UserInformations.FirstMobile = user.FirstMobile;
        response.UserInformations.SecondMobile = user.SecondMobile;
        response.UserInformations.Gender = user.Gender;
        response.UserInformations.GpsLocation = user.GpsLocation;
        response.UserInformations.MentorName = user.MentorName;
        response.UserInformations.NationalID = user.NationalID;
        response.UserInformations.Notes = user.Notes;
        response.UserInformations.SchoolUniversityJob = user.SchoolUniversityJob;
        response.UserInformations.PositionType = user.PositionType;
        response.UserInformations.ParticipationNumber = user.ParticipationNumber;
    }

    private async Task SetRoles(List<UserRole> userroles, JwtAuthResult response)
    {
        foreach (var role in userroles)
        {
            response.Roles.Add(new()
            {
                Name = role.Role!.Name!,
                OrganizationId = role.OrganizationId,
                Organization = (await _applicationDBContext.Organizations.FirstOrDefaultAsync(o => o.Id == role.OrganizationId))?.Name!,
                SchoolId = role.SchoolId,
                School = (await _applicationDBContext.Schools.FirstOrDefaultAsync(o => o.Id == role.SchoolId))?.Name!,
                ActivityId = role.ActivityId,
                Activity = (await _applicationDBContext.Activities.FirstOrDefaultAsync(o => o.Id == role.ActivityId))?.Name!,
            });
        }
    }

    private async Task SetClassrooms(User user, JwtAuthResult response)
    {
        var userClassrooms = await _userClassRepo
            .GetTableNoTracking()
            .Include(ur => ur.Season)
            .Include(ur => ur.UserType)
            .Include(ur => ur.Classroom)
            .ThenInclude(c => c.Grade)
            .ThenInclude(g => g.School)
            .ThenInclude(s => s.Organization)
            .AsSplitQuery()
            .Where(ur => ur.UserId == user.Id && ur.Season.IsCurrent)
            .ToListAsync();

        foreach (var item in userClassrooms)
        {
            response.Classrooms.Add(
                new ClassroomResult
                {
                    Id = item.ClassroomId,
                    Name = item.Classroom.Name,
                    PicturePath = item.Classroom.PicturePath,
                    Location = item.Classroom.Location,
                    Order = item.Classroom.Order,
                    StudentImagePath = item.Classroom.StudentImagePath,
                    TeacherImagePath = item.Classroom.TeacherImagePath,
                    GradeId = item.Classroom.GradeId,
                    GradeName = item.Classroom.Grade.Name,
                    SeasonId = item.SeasonId,
                    SeasonName = item.Season.Name,
                    UserTypeId = item.UserTypeId,
                    UserTypeName = item.UserType.Name,
                    SchoolId = item.Classroom.Grade.SchoolId,
                    SchoolName = item.Classroom.Grade.School.Name,
                    OrganizationId = item.Classroom.Grade.School.OrganizationId,
                    OrganizationName = item.Classroom.Grade.School.Organization.Name,
                });
        }
    }
    */
    #endregion
    private async Task<EntityEntry<User>> UpdateAccessAndRefreshToken(User user, JwtAuthResult response)
    {
        user.AccessToken = response.AccessToken;
        user.RefreshToken = response.RefreshToken;
        user.RefreshTokenExpiryDate = response.RefreshTokenExpiryDate;
        EntityEntry<User> updatedUser = _applicationDBContext.Users.Update(user);
        await _applicationDBContext.SaveChangesAsync();
        return updatedUser;
    }

    private async Task<(string accessToken, DateTime expiryDate)> GenerateJWTToken(User user)
    {
        var claims = await GetClaims(user);
        var jwtToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpireDate),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature));
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return (accessToken, jwtToken.ValidTo);
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var roles = (await _userManager.GetRolesAsync(user)).Distinct();
        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.Name!),
                new Claim(ClaimTypes.NameIdentifier,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber??string.Empty),
                new Claim(nameof(UserClaimModel.Id), user.Id.ToString())
            };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);
        return claims;
    }

    public async Task<Response<JwtAuthResult>> RefreshTokenAsync(RefreshTokenInputDto dto)
    {

        #region Validation

        var user = await _applicationDBContext.Users.AsNoTracking().FirstOrDefaultAsync(s => s.AccessToken == dto.AccessToken);
        if (user is null)
        {
            return ResponseHandler.BadRequest<JwtAuthResult>("Invalid token");
        }

        if (user.RefreshToken != dto.RefreshToken)
        {
            return ResponseHandler.BadRequest<JwtAuthResult>("RefreshToken is invalid");
        }

        if (user.RefreshTokenExpiryDate <= DateTime.Now)
        {
            return ResponseHandler.BadRequest<JwtAuthResult>("Refresh Token is expired");
        }

        #endregion

        #region Generating Token 
        var generateToken = await GetJWTToken(user, Guid.NewGuid());
        return ResponseHandler.Success(generateToken);
        #endregion

    }

    public async Task<Response<bool>> RevokeTokenAsync(string username)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username);
            user!.RefreshToken = Guid.NewGuid();
            await _userManager.UpdateAsync(user);
            return ResponseHandler.Success<bool>(true);
        }
        catch (Exception ex)
        {
            return ResponseHandler.UnprocessableEntity<bool>(ex.Message);
        }
    }

    public async Task<Response<List<RoleResult>>> GetUserRoles(int userId)
    {
        var roleResult = new List<RoleResult>();
        var userroles = await _userRoleRepo
                                .GetTableNoTracking()
                                .Include(ur => ur.Role)
                                .Where(ur => ur.UserId == userId)
                                .ToListAsync();

        foreach (var role in userroles)
        {
            roleResult.Add(new()
            {
                UserRoleId = role.Id,
                Name = role.Role!.Name!,
                OrganizationId = role.OrganizationId,
                Organization = (await _applicationDBContext.Organizations.FirstOrDefaultAsync(o => o.Id == role.OrganizationId))?.Name!,
                SchoolId = role.SchoolId,
                School = (await _applicationDBContext.Schools.FirstOrDefaultAsync(o => o.Id == role.SchoolId))?.Name!,
                ActivityId = role.ActivityId,
                Activity = (await _applicationDBContext.Activities.FirstOrDefaultAsync(o => o.Id == role.ActivityId))?.Name!,
            });
        }
        return ResponseHandler.Success<List<RoleResult>>(roleResult);
    }

    public async Task<Response<List<ClassroomResult>>> GetUserClassrooms(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var classroomsResult = new List<ClassroomResult>();

        var userClassrooms = await _userClassRepo
            .GetTableNoTracking()
            .Include(ur => ur.Season)
            .Include(ur => ur.UserType)
            .Include(ur => ur.Classroom)
            .ThenInclude(c => c.Grade)
            .ThenInclude(g => g.School)
            .ThenInclude(s => s.Organization)
            .AsSplitQuery()
            .Where(ur => ur.UserId == user.Id && ur.Season.IsCurrent)
            .ToListAsync();

        foreach (var item in userClassrooms)
        {
            classroomsResult.Add(
                new ClassroomResult
                {
                    Id = item.ClassroomId,
                    Name = item.Classroom.Name,
                    PicturePath = item.Classroom.PicturePath,
                    Location = item.Classroom.Location,
                    Order = item.Classroom.Order,
                    StudentImagePath = item.Classroom.StudentImagePath,
                    TeacherImagePath = item.Classroom.TeacherImagePath,
                    GradeId = item.Classroom.GradeId,
                    GradeName = item.Classroom.Grade.Name,
                    SeasonId = item.SeasonId,
                    SeasonName = item.Season.Name,
                    UserTypeId = item.UserTypeId,
                    UserTypeName = item.UserType.Name,
                    SchoolId = item.Classroom.Grade.SchoolId,
                    SchoolName = item.Classroom.Grade.School.Name,
                    OrganizationId = item.Classroom.Grade.School.OrganizationId,
                    OrganizationName = item.Classroom.Grade.School.Organization.Name,
                });
        }
        return ResponseHandler.Success<List<ClassroomResult>>(classroomsResult);
    }

    #endregion
}
