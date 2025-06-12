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
        Task<List<StudentProject>> GetStudentProjectsByGroupId(int groupId);
        Task<bool> GroupExists(int groupId);
        Task<List<TutorReview>> GetReviewsByTutorId(int tutorId);
        Task<List<Student>> GetStudentsByGroupId(int groupId);
        Task Add(TutorReview review);
    }
}
