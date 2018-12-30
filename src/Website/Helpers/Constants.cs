using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Helpers
{
    public class Constants
    {
        public static IConfiguration Configuration { get; set; }

        public Constants()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public string ConnectionString
        {
            get
            {
                return Configuration["ConnectionString"];
            }
        }

        public static List<string> RgbaColors = new List<string>() { "rgba(255, 99, 132, 1)", "rgba(54, 162, 235, 1)", "rgba(255, 206, 86, 1)", "rgba(75, 192, 192, 1)", "rgba(153, 102, 255, 1)", "rgba(255, 159, 64, 1)", "rgba(63, 191, 191, 1)", "rgba(191, 127, 63, 1)", "rgba(191, 63, 187, 1)" };

        public static List<string> FunctionOfWords = new List<string> { "sum", "product", "max", "computed" };
    }
}
