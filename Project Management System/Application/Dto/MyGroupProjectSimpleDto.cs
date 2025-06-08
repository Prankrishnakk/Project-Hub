using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class MyGroupProjectSimpleDto
    {
        public string GroupName { get; set; }
        public List<string> MemberNames { get; set; }
        public List<string> ProjectTitles { get; set; }
    }

}
