using Application.ApiResponse;
using Application.Dto;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.TutorInterface
{
    public interface ITutorReviewService
    {
        Task<ApiResponse<TutorReviewDto>> ReviewProject(TutorReviewDto dto, int TutorId);

    }
}
