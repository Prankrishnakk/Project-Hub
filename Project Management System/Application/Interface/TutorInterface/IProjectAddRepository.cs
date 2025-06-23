using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.TutorInterface
{
    public interface IProjectAddRepository
    {
        Task AddProject(Project project);
        Task Save();
    }
}
