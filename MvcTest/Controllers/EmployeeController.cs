using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcTest.Data;
using MvcTest.Models;
using MvcTest.Util;

namespace MvcTest.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class EmployeeController : Controller
    {
        private readonly MvcTestContext _context;

        public EmployeeController(MvcTestContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employee.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public IActionResult IconUpload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IconUpload(IFormFile file)
        {
            string defaultIconId = "74a3e8fe-86d1-4a50-b703-08da4c490ff0";

            if (file == null)
            {
                return Redirect(nameof(Create) + "/" + defaultIconId);
            }
            string filename = file.FileName;
            byte[] content = await file.GetBytesAsync();
            string filetype = file.ContentType;

            if (!filetype.StartsWith("image/"))
            {
                return Redirect(nameof(Create) + "/" + defaultIconId);
            }

            FileModel fm = new FileModel()
            {
                FileId = new Guid(),
                FileName = filename,
                FileBin = content,
                FileType = filetype
            };
            try
            {
                _context.FileModel.Update(fm);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Redirect(nameof(Create) + "/" + defaultIconId);
            }
            //Employee emp = new Employee() { PhotoFileName = fm.FileId.ToString() };
            //return View("~/Views/Employee/Create.cshtml", emp);
            return Redirect(nameof(Create) + "/" + fm.FileId.ToString());

        }



        // GET: Employee/Create
        [HttpGet("Employee/Create/{name}")]
        public IActionResult Create()
        {
            return View(_context.Department.ToList());
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Employee/Create/{name}")]
        public async Task<IActionResult> Create(string name, [Bind("EmployeeId,EmployeeName,Department,DateOfJoining,PhotoFileName")] Employee employee)
        {
            employee.PhotoFileName = name;
            try
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
            //return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,EmployeeName,Department,DateOfJoining,PhotoFileName")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}
