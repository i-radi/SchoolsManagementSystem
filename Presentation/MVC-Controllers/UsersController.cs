using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Presentation.MVC_Controllers
{
    [Authorize(Policy = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public UsersController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IAuthService authService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }


        // GET: Users
        public async Task<IActionResult> Index()
        {
            var modelItems = PaginatedList<User>
            .Create(await _userManager.Users
            .Include(u => u.Organization)
            .ToListAsync(), 1, 10);

            var result = _mapper.Map<IEnumerable<GetUserDto>>(modelItems);
            foreach (var userDto in result.Select((value, i) => new { i, value }))
            {
                userDto.value.Role = (await _userManager.GetRolesAsync(modelItems[userDto.i])).FirstOrDefault()!;
            }
            return View(result);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _userManager.Users
            .Include(u => u.Organization)
            .FirstOrDefaultAsync(u => u.Id == id);

            var result = _mapper.Map<GetUserDto>(modelItem);
            if (result == null)
            {
                return NotFound();
            }
            result.Role = (await _userManager.GetRolesAsync(modelItem!)).FirstOrDefault()!;

            return View(result);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _userManager.Users
            .Include(u => u.Organization)
            .FirstOrDefaultAsync(u => u.Id == id);

            var result = _mapper.Map<GetUserDto>(modelItem);
            if (result == null)
            {
                return NotFound();
            }
            result.Role = (await _userManager.GetRolesAsync(modelItem!)).FirstOrDefault()!;

            return View(result);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modelItem = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (modelItem == null)
            {
                return Problem("Entity set 'ApplicationDBContext.User'  is null.");
            }
            await _userManager.DeleteAsync(modelItem);

            return RedirectToAction(nameof(Index));
        }
    }
}
