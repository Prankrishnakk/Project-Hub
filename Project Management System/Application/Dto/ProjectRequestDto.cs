﻿using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class ProjectRequestDto
    {
     
        public int TutorId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectDescription { get; set; }
      
    }
}
