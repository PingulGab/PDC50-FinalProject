using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using SchedEd.Model;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SchedEd.Model
{
    public class Class
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public JsonNode? Days { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public int StudentCount { get; set; }
    }
}
