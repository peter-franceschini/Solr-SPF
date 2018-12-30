using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models.Explain
{
    public class FieldQuery
    {
        public string Name { get; set; }
        public string Query { get; set; }

        public FieldQuery(string name, string query)
        {
            Name = name;
            Query = query;
        }
    }
}
