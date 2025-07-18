﻿using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.HodInterface
{
    public interface IGetDepartmentGroupsService
    {
        Task<ICollection<DepartmentProjectGroupDto>> GetGroupsByDepartment(string department);
        Task<ICollection<CompletedProjectDto>> GetCompletedProjectsByDepartment(string department);
    }
}
