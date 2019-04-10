using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoWriter.Struct
{
    public class ThumbResult
    {
        public bool isError { get; set; }
        public string errorMessage { get; set; }
        public string errorCode { get; set; }
        public ThumbResultItem results { get; set; }
    }

    public class ThumbResultItem
    {
        public string outputFile { get; set; }
        public string outputFileUrl { get; set; } 
    }
}
