using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    #endregion

    #region Constructors
    public AuthService(JwtSettings jwtSettings,
        UserManager<User> userManager,
        ApplicationDBContext applicationDBContext,
        IUserRoleRepo userRoleRepo,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment)
    {
        _jwtSettings = jwtSettings;
        _userManager = userManager;
        _applicationDBContext = applicationDBContext;
        _userRoleRepo = userRoleRepo;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }
    #endregion

    #region Handle Methods
    public async Task<Response<string>> AddAsync(AddUserDto dto)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            return ResponseHandler.BadRequest<string>("Email is already registered!");

        var user = _mapper.Map<User>(dto);
        user.UserName = dto.Email.Split('@')[0];
        user.ProfilePicturePath = "uploads/users/emptyAvatar.png";

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description},";

            return ResponseHandler.BadRequest<string>(errors);
        }

        var createdUser = _userManager.FindByEmailAsync(user.Email!);
        QR.Generate(createdUser.Result!.Id, _webHostEnvironment);

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

        var user = await _userManager.FindByEmailAsync(dto.Email);

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

        var response = new JwtAuthResult
        {
            AccessToken = accessToken,
            AccessTokenExpiryDate = expireDate,
            RefreshToken = refreshToken,
            RefreshTokenExpiryDate = DateTime.Now.AddMinutes(_jwtSettings.RefreshTokenExpireDate),
            IsAuthenticated = true,
            Email = user.Email ?? string.Empty,
            IsSuperAdmin = userroles.Any(r => r.Role!.Name == "SuperAdmin"),
        };
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

        user.AccessToken = response.AccessToken;
        user.RefreshToken = response.RefreshToken;
        user.RefreshTokenExpiryDate = response.RefreshTokenExpiryDate;
        var updatedUser = await _userManager.UpdateAsync(user);
        if (!updatedUser.Succeeded)
        {
            return new JwtAuthResult { IsAuthenticated = false };
        }
        return response;
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

        var token = new JwtSecurityTokenHandler().ReadJwtToken(dto.AccessToken);
        var username = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            return ResponseHandler.BadRequest<JwtAuthResult>("Invalid token");
        }

        if (user.AccessToken != dto.AccessToken
            || user.RefreshToken != dto.RefreshToken)
        {
            return ResponseHandler.BadRequest<JwtAuthResult>("Token is invalid");
        }

        if (user.RefreshTokenExpiryDate <= DateTime.Now)
        {
            return ResponseHandler.BadRequest<JwtAuthResult>("Refresh Token is expired");
        }

        #endregion

        #region Generating Token 
        var generateToken = await GetJWTToken(user, user.RefreshToken);
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

    #endregion
}
