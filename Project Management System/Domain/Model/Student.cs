using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Student
    {
        public int ProjectStatus;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Department { get; set; }
        public string Role { get; set; } = "Student";
        public int? GroupId { get; set; }
        public bool IsBlocked { get; set; }
        public virtual ProjectGroup? Group { get; set; }

        // ➕ Add this if the student is a tutor
        public virtual ICollection<ProjectGroup> TutoredGroups { get; set; }
       
        public virtual ICollection<StudentProject> StudentProjects { get; set; }
        public virtual ICollection<ProjectRequest> ProjectRequests { get; set; }






    }
}
