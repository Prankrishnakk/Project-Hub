using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.TutorInterface
{
    public interface ITutorReviewRepository
    {
        Task Add(TutorReview review);
        Task<List<TutorReview>> GetReviewsByTutorId(int tutorId);
        Task<TutorReview?> GetReviewByProjectId(int projectId);
        Task<bool> StudentProjectExists(int studentProjectId);
   
    }
}
