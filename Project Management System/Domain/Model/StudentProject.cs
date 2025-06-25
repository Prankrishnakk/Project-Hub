using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class StudentProject
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }     
        public DateTime UploadedAt { get; set; }
        public bool IsReviewed { get; set; }
        public bool FinalSubmission {get; set; }
        public virtual Student Student { get; set; }

    }
}
