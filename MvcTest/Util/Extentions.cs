using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MvcTest.Util
{
    public static class Extentions
    {
        public static async Task<byte[]> GetBytesAsync(this IFormFile formFile)
        {
            using var memStream = new MemoryStream();
            await formFile.CopyToAsync(memStream);
            return memStream.ToArray();
        }
    }
}
