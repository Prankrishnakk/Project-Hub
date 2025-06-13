using System;
using Domain.Model;

namespace Domain.Model
{
    public class TutorReview
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public int GroupId{ get; set; }
        public int TutorId { get; set; }
        public string? Feedback { get; set; }
        public float Mark {  get; set; }
        public DateTime ReviewedAt { get; set; } = DateTime.Now;

  
        public virtual Student Tutor { get; set; } 
    }
}
