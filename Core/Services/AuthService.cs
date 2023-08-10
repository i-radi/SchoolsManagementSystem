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
    private readonly IMapper _mapper;
    #endregion

    #region Constructors
    public AuthService(JwtSettings jwtSettings,
        UserManager<User> userManager,
        ApplicationDBContext applicationDBContext,
        IMapper mapper)
    {
        _jwtSettings = jwtSettings;
        _userManager = userManager;
        _applicationDBContext = applicationDBContext;
        _mapper = mapper;
    }
    #endregion

    #region Handle Methods
    public async Task<Response<string>> RegisterAsync(RegisterDto dto)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            return ResponseHandler.BadRequest<string>("Email is already registered!");

        var user = _mapper.Map<User>(dto);
        user.UserName = dto.Email;

        var result = await _userManager.CreateAsync(user, dto.Password);
        await _userManager.AddToRoleAsync(user, dto.Role);

        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description},";

            return ResponseHandler.BadRequest<string>(errors);
        }

        return ResponseHandler.Success<string>($"{dto.Email} created successfully");
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
        var (accessToken, expireDate) = await GenerateJWTToken(user);

        var response = new JwtAuthResult
        {
            AccessToken = accessToken,
            AccessTokenExpiryDate = expireDate,
            RefreshToken = refreshToken,
            RefreshTokenExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
            IsAuthenticated = true,
            Email = user.Email ?? string.Empty,
            Roles = (await _userManager.GetRolesAsync(user!)).ToList()
        };

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
            expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature));
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return (accessToken, jwtToken.ValidTo);
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName!),
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
            || user.RefreshToken != dto.RefreshToken
            || user.RefreshTokenExpiryDate <= DateTime.Now)
        {
            return ResponseHandler.BadRequest<JwtAuthResult>("Token is invalid");
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
