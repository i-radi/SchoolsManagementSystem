using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SMS.Core.IServices;
using SMS.Models.Entities.Identity;
using SMS.Models.Helpers;
using SMS.Persistance.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SMS.Core.Services;

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

    public async Task<JwtAuthResult> LoginAsync(LoginDto dto)
    {
        var authModel = new JwtAuthResult();

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            authModel.Message = "Email or Password is incorrect!";
            return authModel;
        }

        return await GetJWTToken(user);
    }

    public async Task<JwtAuthResult> RegisterAsync(RegisterDto dto)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            return new JwtAuthResult { Message = "Email is already registered!" };

        if (await _userManager.FindByNameAsync(dto.UserName) is not null)
            return new JwtAuthResult { Message = "Username is already registered!" };

        var user = _mapper.Map<User>(dto);

        var result = await _userManager.CreateAsync(user, dto.Password);
        await _userManager.AddToRoleAsync(user, dto.Role);

        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description},";

            return new JwtAuthResult { Message = errors };
        }

        return await GetJWTToken(user);
    }

    public async Task<JwtAuthResult> GetJWTToken(User user)
    {
        var (jwtToken, accessToken) = await GenerateJWTToken(user);

        var response = new JwtAuthResult
        {
            AccessToken = accessToken,
            IsAuthenticated = true,
            Email = user.Email,
            Username = user.UserName,
            Role = (await _userManager.GetRolesAsync(user!)).FirstOrDefault()!
        };
        return response;
    }

    private async Task<(JwtSecurityToken, string)> GenerateJWTToken(User user)
    {
        var claims = await GetClaims(user);
        var jwtToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature));
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return (jwtToken, accessToken);
    }

    public async Task<List<Claim>> GetClaims(User user)
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

    public JwtSecurityToken ReadJWTToken(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            throw new ArgumentNullException(nameof(accessToken));
        }
        var handler = new JwtSecurityTokenHandler();
        var response = handler.ReadJwtToken(accessToken);
        return response;
    }

    public async Task<string> ValidateToken(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = _jwtSettings.ValidateIssuer,
            ValidIssuers = new[] { _jwtSettings.Issuer },
            ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
            ValidAudience = _jwtSettings.Audience,
            ValidateAudience = _jwtSettings.ValidateAudience,
            ValidateLifetime = _jwtSettings.ValidateLifeTime,
        };
        try
        {
            var validator = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);

            if (validator == null)
            {
                return "InvalidToken";
            }

            return "NotExpired";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    #endregion
}
