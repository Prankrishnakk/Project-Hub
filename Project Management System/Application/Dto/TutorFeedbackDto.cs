using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class TutorFeedbackDto
    {
        public string? Feedback { get; set; }
        public DateTime ReviewedAt { get; set; }
        public string TutorName { get; set; }
    }
}
