using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Persistance.IRepos;
using System.Data;
using VModels.ViewModels.Users;

namespace Presentation.Controllers.MVC
{
    [Authorize(Policy = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IOrganizationRepo _organizationService;
        private readonly ISchoolRepo _schoolService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public UsersController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOrganizationRepo organizationService,
            ISchoolRepo schoolService,
            IAuthService authService,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _organizationService = organizationService;
            _schoolService = schoolService;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }


        // GET: Users
        public async Task<IActionResult> Index(string searchName = "", string searchRole = "")
        {
            IQueryable<User> usersQuery = _userManager.Users
                    .Include(u => u.Organization);

            if (!string.IsNullOrEmpty(searchName))
            {
                usersQuery = usersQuery.Where(u => u.Name.Contains(searchName));
            }

            if (!string.IsNullOrEmpty(searchRole))
            {
                var usersWithRole = await _userManager.GetUsersInRoleAsync(searchRole);
                var userIds = usersWithRole.Select(u => u.Id);
                usersQuery = usersQuery.Where(u => userIds.Contains(u.Id));
            }

            var modelItems = PaginatedList<User>.Create(usersQuery, 1, 10);

            var result = _mapper.Map<IEnumerable<GetUserDto>>(modelItems);
            //foreach (var userDto in result.Select((value, i) => new { i, value }))
            //{
            //    userDto.value.Role = (await _userManager.GetRolesAsync(modelItems[userDto.i])).FirstOrDefault()!;
            //}
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

        // GET: Users/Roles/5
        public async Task<IActionResult> Roles(int? id)
        {
            var roles = new List<string>();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user is not null)
            {
                roles = (await _userManager.GetRolesAsync(user)).ToList();
            }

            ViewBag.UserId = id;
            return View(roles);
        }

        // GET: Users/DeleteRole
        public async Task<IActionResult> DeleteRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return RedirectToAction("Roles", new { id = userId });
            }
            return NotFound();
        }

        // GET: Users/CreateRole/
        public async Task<IActionResult> CreateRole(int userId)
        {
            var roles = (await _roleManager.Roles.Select(r => r.Name).ToListAsync()).AsEnumerable();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var userRoles = (await _userManager.GetRolesAsync(user!)).AsEnumerable();
            roles = roles.Except(userRoles).ToList();

            var orgs = _organizationService.GetTableNoTracking()
                .Select(o => new { OrganizationId = o.Id, o.Name }).ToList();

            var schoolsQuery = _schoolService.GetTableNoTracking().AsQueryable();
            if (user!.OrganizationId is not null && user.OrganizationId > 0)
            {
                schoolsQuery = schoolsQuery.Where(s => s.OrganizationId == user.OrganizationId);
            }
            var schools = schoolsQuery.Select(o => new { SchoolId = o.Id, o.Name }).ToList();

            var createRoleViewModel = new CreateRoleViewModel
            {
                Id = userId,
                UserName = user!.Email,
                RoleOptions = new SelectList(roles),
                OrganizationOptions = new SelectList(orgs, "OrganizationId", "Name"),
                SchoolOptions = new SelectList(schools, "SchoolId", "Name")
            };

            return View(createRoleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel viewModel)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == viewModel.Id);
            if (user is not null)
            {
                var roles = await _userManager.AddToRoleAsync(user, viewModel.RoleName);

                if (viewModel.SchoolId > 0)
                {
                    user.SchoolId = viewModel.SchoolId;
                }

                if (viewModel.OrganizationId > 0)
                {
                    user.OrganizationId = viewModel.OrganizationId;
                }

                var userResult = await _userManager.UpdateAsync(user);

                if (roles.Succeeded && userResult.Succeeded)
                {
                    return RedirectToAction("Roles", new { id = viewModel.Id });
                }
            }
            return BadRequest();
        }
    }
}
