using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Dto
{
    public class DepartmentProjectGroupDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string TutorName { get; set; }
        public List<string> StudentNames { get; set; }
    }
}

