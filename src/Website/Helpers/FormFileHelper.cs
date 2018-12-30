using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Helpers
{
    public static class FormFileHelper
    {
        public static string ConvertToString(IFormFile file)
        {
            var result = string.Empty;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
