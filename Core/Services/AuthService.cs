using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using Models.Entities.Identity;
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
    private readonly SharedSettings _sharedSettings;
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
        , IAttachmentService attachmentService,
        SharedSettings userSettings)

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
        _sharedSettings = userSettings;
    }
    #endregion

    #region Handle Methods
    public async Task<Result<string>> AddAsync(AddUserDto dto)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            return ResultHandler.BadRequest<string>("Email is already registered!");

        var user = _mapper.Map<User>(dto);
        user.UserName = dto.Email.Split('@')[0];
        user.ProfilePicturePath = _sharedSettings.DefaultProfileImage;

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description},";

            return ResultHandler.BadRequest<string>(errors);
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

        return ResultHandler.Success<string>($"{dto.Email} created successfully");
    }

    public async Task<Result<GetUserDto>> ChangeUserPasswordAsync(ChangeUserPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return ResultHandler.BadRequest<GetUserDto>("Invalid Email...");
        }

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description},";

            return ResultHandler.BadRequest<GetUserDto>(errors);
        }

        user.PlainPassword = dto.NewPassword;
        var updatedUserResult = await _userManager.UpdateAsync(user);
        if (!updatedUserResult.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in updatedUserResult.Errors)
                errors += $"{error.Description},";

            return ResultHandler.BadRequest<GetUserDto>(errors);
        }

        if (!string.IsNullOrEmpty(user.ProfilePicturePath))
        {
            user.ProfilePicturePath = $"{_baseSettings.url}/{_baseSettings.usersPath}/{user.ProfilePicturePath}";
        }

        var viewmodel = _mapper.Map<GetUserDto>(user);

        return ResultHandler.Success<GetUserDto>(viewmodel);
    }

    public async Task<Result<JwtAuthResult>> UpdateAsync(ChangeUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

        if (user == null)
        {
            return ResultHandler.NotFound<JwtAuthResult>("User not found.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != user.Email)
        {
            var checkUser = await _userManager.FindByEmailAsync(dto.Email);
            if (checkUser is not null)
            {
                return ResultHandler.BadRequest<JwtAuthResult>("The email already exists.");
            }
            user.Email = dto.Email;
            user.UserName = dto.Email.Split('@')[0];
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return ResultHandler.BadRequest<JwtAuthResult>(errors);
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

                return ResultHandler.BadRequest<JwtAuthResult>(errors);
            }
        }

        return ResultHandler.Success<JwtAuthResult>(await GetJWTToken(user, Guid.NewGuid()));
    }

    public async Task<Result<JwtAuthResult>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);

        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return ResultHandler.NotFound<JwtAuthResult>("Email or Password is incorrect!");
        }

        return ResultHandler.Success<JwtAuthResult>(await GetJWTToken(user, Guid.NewGuid()));
    }

    public async Task<Result<JwtAuthResult>> LoginByUserNameAsync(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);

        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return ResultHandler.NotFound<JwtAuthResult>("Email or Password is incorrect!");
        }

        return ResultHandler.Success<JwtAuthResult>(await GetJWTToken(user, Guid.NewGuid()));
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
                new(ClaimTypes.Name,user.Name!),
                new(ClaimTypes.NameIdentifier,user.UserName!),
                new(ClaimTypes.Email,user.Email!),
                new(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber??string.Empty),
                new(nameof(UserClaimModel.Id), user.Id.ToString())
            };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            if (role == "OrganizationAdmin")
            {
                var orgId = _userRoleRepo.GetTableNoTracking().FirstOrDefault(ur => ur.UserId == user.Id && ur.RoleId == 2)?.OrganizationId;
                claims.Add(new Claim(nameof(UserClaimModel.OrgId), orgId.ToString()!));
            }
            if (role == "SchoolAdmin")
            {
                var schoolId = _userRoleRepo.GetTableNoTracking().FirstOrDefault(ur => ur.UserId == user.Id && ur.RoleId == 3)?.SchoolId;
                claims.Add(new Claim(nameof(UserClaimModel.SchoolId), schoolId.ToString()!));
            }
        }
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);
        return claims;
    }

    public async Task<Result<JwtAuthResult>> RefreshTokenAsync(RefreshTokenInputDto dto)
    {

        #region Validation

        var user = await _applicationDBContext.Users.AsNoTracking().FirstOrDefaultAsync(s => s.AccessToken == dto.AccessToken);
        if (user is null)
        {
            return ResultHandler.BadRequest<JwtAuthResult>("Invalid token");
        }

        if (user.RefreshToken != dto.RefreshToken)
        {
            return ResultHandler.BadRequest<JwtAuthResult>("RefreshToken is invalid");
        }

        if (user.RefreshTokenExpiryDate <= DateTime.Now)
        {
            return ResultHandler.BadRequest<JwtAuthResult>("Refresh Token is expired");
        }

        #endregion

        #region Generating Token 
        var generateToken = await GetJWTToken(user, Guid.NewGuid());
        return ResultHandler.Success(generateToken);
        #endregion

    }

    public async Task<Result<bool>> RevokeTokenAsync(string username)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username);
            user!.RefreshToken = Guid.NewGuid();
            await _userManager.UpdateAsync(user);
            return ResultHandler.Success<bool>(true);
        }
        catch (Exception ex)
        {
            return ResultHandler.UnprocessableEntity<bool>(ex.Message);
        }
    }

    public async Task<Result<List<RoleResult>>> GetUserRoles(int userId)
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
        return ResultHandler.Success<List<RoleResult>>(roleResult);
    }

    public async Task<Result<List<ClassroomResult>>> GetUserClassrooms(int userId)
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
        return ResultHandler.Success<List<ClassroomResult>>(classroomsResult);
    }

    #endregion
}
