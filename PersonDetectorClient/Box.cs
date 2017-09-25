using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonDetectorClient
{
    public class Box
    {
        [JsonProperty("xmin")]
        public float xmin { get; set; }
        [JsonProperty("ymin")]
        public float ymin { get; set; }
        [JsonProperty("xmax")]
        public float xmax { get; set; }
        [JsonProperty("ymax")]
        public float ymax { get; set; }
    }
}
