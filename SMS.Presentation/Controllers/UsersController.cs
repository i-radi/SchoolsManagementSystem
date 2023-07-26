using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Models.Entities.Identity;
using SMS.VModels.DTOS.Users.Queries;
using System.Security.Claims;

namespace SMS.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    //private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public UsersController(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        //IAuthService authService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        //_authService = authService;
        _mapper = mapper;
    }

    [HttpGet]
    //[Authorize(Policy = "SuperAdmin")]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAll()
    {
        var modelItems = await _userManager.Users.ToListAsync();
        var result = _mapper.Map<IEnumerable<GetUserDto>>(modelItems);
        foreach (var userDto in result.Select((value, i) => new { i, value }))
        {
            userDto.value.Role = (await _userManager.GetRolesAsync(modelItems[userDto.i])).FirstOrDefault()!;
        }
        return Ok(result);
    }

    [HttpGet("id")]
    //[Authorize(Policy = "SuperAdmin")]
    public async Task<ActionResult<GetUserDto>> GetById(int id)
    {
        var modelItem = await _userManager.FindByIdAsync(id.ToString());
        var result = _mapper.Map<GetUserDto>(modelItem);
        result.Role = (await _userManager.GetRolesAsync(modelItem!)).FirstOrDefault()!;
        return Ok(result);
    }
}
