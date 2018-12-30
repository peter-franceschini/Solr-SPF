using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models.Explain
{
    public class DocumentExplain
    {
        public ExplainModel ExplainModel { get; set; }
        public DocumentModel DocumentModel { get; set; }
        public int Iteration { get; set; }
    }
}
