using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchedEd.Model
{
    public class Student
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string StudentID { get; set; }
        public string ContactNumber { get; set; }
        public int ClassID { get; set; }
        public string Birthdate { get; set; }
        public string ElementaryEducation { get; set; }
        public string SecondaryEducation { get; set; }
        public string TertiaryEducation { get; set; }

        public string ClassName { get; set; }
    }
}
