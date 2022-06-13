using System;
using System.Collections.Generic;
using System.IO;
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
    public class FileController : Controller
    {
        private readonly MvcTestContext _context;

        public FileController(MvcTestContext context)
        {
            _context = context;
        }

        // GET: File
        public async Task<IActionResult> Index(string id)
        {
            var file = await _context.FileModel
                .FirstOrDefaultAsync(m => m.FileId.ToString() == id);
            if (file == null)
            {
                return NotFound(_context.FileModel);
            }
            return File(file.FileBin, file.FileType);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Index([FromForm] DocumentCreateRequest request)
        {
            string filename = request.File.FileName;
            byte[] content = await request.File.GetBytesAsync();
            string filetype = request.File.ContentType;

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
                return BadRequest();
            }
            return Ok(fm.FileId.ToString());
        }


    }
}
