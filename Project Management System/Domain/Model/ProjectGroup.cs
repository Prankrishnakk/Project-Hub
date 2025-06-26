using Domain.Enum;
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
        public int? ProjectId { get; set; }
        public string GroupName { get; set; }
        public int? TutorId { get; set; }
        public ProjectStatus? Status { get; set; }
        public virtual Student? Tutor { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual Project Project { get; set; }
    }
   
}


