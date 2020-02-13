using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileAPI.Model
{
    public class CloudStorageOptions
    {
        public string ConnectionString { get; set; }
        public string Container { get; set; }
    }
}
