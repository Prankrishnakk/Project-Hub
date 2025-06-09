using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class ProjectByGroupDto
    {
        public int ProjectId { get; set; }
        public string FileName{ get; set; }
        public string StudentName { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
