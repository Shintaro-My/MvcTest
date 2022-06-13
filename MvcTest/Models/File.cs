using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MvcTest.Models
{
    public class FileModel
    {
        [Key]
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public byte[] FileBin { get; set; }
        public string FileType { get; set; }
    }

    public record DocumentCreateRequest([FromForm(Name = "file")] IFormFile File);
}
