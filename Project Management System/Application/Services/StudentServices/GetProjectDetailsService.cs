using Application.Dto;
using Application.Interface.StudentInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.StudentServices
{
    public class GetProjectDetailsService : IGetProjectDetailsService
    {
        private readonly IGetProjectDetailsRepository _repository;
        public GetProjectDetailsService(IGetProjectDetailsRepository repository) 
        {
            _repository = repository;
        }

        public async Task<MyGroupProjectSimpleDto> GetMyGroupAndProjects(int studentId)
        {
            return await _repository.GetStudentGroupAndProjects(studentId);
        }
    }
}
