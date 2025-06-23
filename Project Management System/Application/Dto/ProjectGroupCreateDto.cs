using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class ProjectGroupCreateDto
    {
      
        public string GroupName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public List<int> StudentIds { get; set; }
    }
}
