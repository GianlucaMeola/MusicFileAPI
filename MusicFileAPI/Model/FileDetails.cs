using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileAPI.Model
{
    public class FileDetails
    {
        public string title { get; set; }
        public string artist { get; set; }
        public Uri uri { get; set; }
    }
}
