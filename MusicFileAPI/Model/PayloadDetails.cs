using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileAPI.Model
{
    public class PayloadDetails
    {
        public IFormFile musicFile { get; set; }
        public string artist { get; set; }
        public string title { get; set; }
    }
}
