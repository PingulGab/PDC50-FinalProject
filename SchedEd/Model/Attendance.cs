using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SchedEd.Model
{
    public class Attendance
    {
        public int ID { get; set; }
        public int ClassID { get; set; }
        public int StudentID { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
