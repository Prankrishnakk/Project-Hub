using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class AdminGroupSummaryDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public ProjectStatus Status { get; set; }
        public string TutorName { get; set; }
        public List<string> StudentNames { get; set; }
    }
}
