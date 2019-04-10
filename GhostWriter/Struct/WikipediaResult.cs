using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoWriter.Struct
{
    public class WikipediaResult
    {
        public string content { get; set; }
        public List<string> images { get; set; }
        public List<string> links { get; set; }
        public string pageid { get; set; }
        public List<string> references { get; set; }
        public string summary { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }
}
