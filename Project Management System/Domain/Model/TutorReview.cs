using System;
using Domain.Model;

namespace Domain.Model
{
    public class TutorReview
    {
        public int Id { get; set; }

        public int StudentProjectId { get; set; }
        public int TutorId { get; set; }

        public string? Feedback { get; set; }

        public DateTime ReviewedAt { get; set; } = DateTime.Now;

  
        public virtual StudentProject StudentProject { get; set; }
        public virtual Student Tutor { get; set; } 
    }
}
