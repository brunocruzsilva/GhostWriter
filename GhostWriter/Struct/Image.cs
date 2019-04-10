using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoWriter.Struct
{
    public class Image
    {
        public bool IsErrorResize { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string URI { get; set; }
        public string URIResize { get; set; }
        public string Path { get; set; }
        public string PathResize { get; set; }
    }
}
