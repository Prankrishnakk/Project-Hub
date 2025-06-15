using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class TutorFeedbackDto
    {
        public int ReviewId { get; set; }
        public float Mark { get; set; }
        public string? Feedback { get; set; }
        public DateTime ReviewedAt { get; set; }
        public string TutorName { get; set; }

    }
}
