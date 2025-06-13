using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class TutorReviewDto
    {
        public int ReviewId { get; set; }

       [Range(1, 10, ErrorMessage = "Mark must be between 1 and 10.")]
        public float Mark {  get; set; }
        public int GroupId { get; set; }
        public string? Feedback { get; set; }
    }
}
