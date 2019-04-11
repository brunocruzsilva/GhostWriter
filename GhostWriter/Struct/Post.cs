using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.People;

namespace VideoWriter.Struct
{
    public class Post
    {
        public Post() {
            this.Images = new List<Image>();
            this.Tags = new List<string>(); 
        }

        public int MaxNumberImages { get; set; }
        public bool IsPosted { get; set; }
        public string Keyword { get; set; }
        public string Subtitle { get; set; }
        public string Summary { get; set; }
        public List<Image> Images { get; set; }
        public List<string> Tags { get; set; }

        public TvCredits TvCredits { get; set; }
    }
}
