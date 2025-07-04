﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Project
    { 
        public int Id { get; set; }
        public string Title { get; set; }
 

        public virtual ICollection<ProjectRequest> ProjectRequests { get; set; }
        public virtual ICollection<ProjectGroup> ProjectGroups { get; set; }
    }
}
