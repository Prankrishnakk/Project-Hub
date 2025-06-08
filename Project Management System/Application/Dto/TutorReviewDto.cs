using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class TutorReviewDto
    {
        public int StudentProjectId { get; set; }
        public int TutorId { get; set; }
        public string? Feedback { get; set; }
    }
}
