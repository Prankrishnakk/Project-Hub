using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ProjectGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string ProjectTitle { get; set; }

        public int? TutorId { get; set; }


        public virtual Student? Tutor { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
   
}


