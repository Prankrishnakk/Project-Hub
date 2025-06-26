using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class MyGroupProjectSimpleDto
    {
        public int ProjectId { get; set; }
        public string GroupName { get; set; }
        public List<string> MemberNames { get; set; }
       
    }

}
