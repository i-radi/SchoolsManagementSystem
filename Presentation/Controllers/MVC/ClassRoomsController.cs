using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Persistance.IRepos;

namespace Presentation.Controllers.MVC
{
    public class ClassRoomsController : Controller
    {
        private readonly IClassRoomRepo _classRoomRepo;
        private readonly IGradeRepo _gradeRepo;

        public ClassRoomsController(IClassRoomRepo classRoomRepo, IGradeRepo gradeRepo)
        {
            _classRoomRepo = classRoomRepo;
            _gradeRepo = gradeRepo;
        }

        // GET: ClassRooms
        public async Task<IActionResult> Index(int gradeId)
        {
            var classrooms = _classRoomRepo.GetTableNoTracking().Include(c => c.Grade).AsQueryable();
            if (gradeId > 0)
            {
                classrooms = classrooms.Where(c => c.GradeId == gradeId);
            }

            return View(await classrooms.ToListAsync());
        }

        // GET: ClassRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classRoom = await _classRoomRepo.GetByIdAsync(id.Value);
            if (classRoom == null)
            {
                return NotFound();
            }

            return View(classRoom);
        }

        // GET: ClassRooms/Create
        public IActionResult Create()
        {
            ViewData["GradeId"] = new SelectList(_gradeRepo.GetTableAsTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: ClassRooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,GradeId")] ClassRoom classRoom)
        {
            if (ModelState.IsValid)
            {
                await _classRoomRepo.AddAsync(classRoom);
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradeId"] = new SelectList(_gradeRepo.GetTableAsTracking().ToList(), "Id", "Name", classRoom.GradeId);
            return View(classRoom);
        }

        // GET: ClassRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classRoom = await _classRoomRepo.GetByIdAsync(id.Value);
            if (classRoom == null)
            {
                return NotFound();
            }
            ViewData["GradeId"] = new SelectList(_gradeRepo.GetTableAsTracking().ToList(), "Id", "Name", classRoom.GradeId);
            return View(classRoom);
        }

        // POST: ClassRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GradeId")] ClassRoom classRoom)
        {
            if (id != classRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _classRoomRepo.UpdateAsync(classRoom);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassRoomExists(classRoom.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradeId"] = new SelectList(_gradeRepo.GetTableAsTracking().ToList(), "Id", "Name", classRoom.GradeId);
            return View(classRoom);
        }

        // GET: ClassRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classRoom = await _classRoomRepo.GetByIdAsync(id.Value);
            if (classRoom == null)
            {
                return NotFound();
            }

            return View(classRoom);
        }

        // POST: ClassRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classRoom = await _classRoomRepo.GetByIdAsync(id);
            if (classRoom != null)
            {
                await _classRoomRepo.DeleteAsync(classRoom);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClassRoomExists(int id)
        {
            return (_classRoomRepo.GetTableNoTracking().ToList().Any(e => e.Id == id));
        }
    }
}
