using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ProjectRequest
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TutorId { get; set; }
        public int ProjectId { get; set; }
        //public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public RequestStatus Status { get; set; }   
        public virtual Student Student { get; set; }
        public virtual Student Tutor { get; set; }
        public virtual Project Project { get; set; }


    }
}
