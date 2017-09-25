using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonDetectorClient
{
    public class Detection
    {
        [JsonProperty("score")]
        public float score { get; set; }
        [JsonProperty("classId")]
        public int classId { get; set; }
        [JsonProperty("box")]
        public Box box { get; set; }
    }
}
