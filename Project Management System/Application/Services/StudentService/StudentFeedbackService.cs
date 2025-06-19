using Application.Dto;
using Application.Interface.StudentInterface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.StudentServices
{
    public class StudentFeedbackService : IStudentFeedbackService
    {
        private readonly IStudentFeedbackRepository _repository;

        public StudentFeedbackService(IStudentFeedbackRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<TutorFeedbackDto>> GetMyFeedbacks(int studentId)
        {
            try
            {
                return await _repository.GetFeedbacksByStudentId(studentId);
            }
            catch (Exception ex)
            {
    
                throw new ApplicationException($"Failed to fetch feedbacks for student ID {studentId}: {ex.Message}", ex);
            }
        }
    }
}
