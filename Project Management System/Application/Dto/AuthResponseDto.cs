﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class AuthResponseDto
    {
        public string Name { get; set; } 
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; } = "Student";
     
    }
}
