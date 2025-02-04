using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels.WorkoutCategoryViewModels;

namespace Training.Application.Interfaces
{
    public interface IWorkoutCategoryService
    {
        List<WorkoutCategoryViewModel> Get(string tokenId);
        WorkoutCategoryViewModel GetById(Guid id, string tokenId);
        List<WorkoutCategoryViewModel> GetByName(string name, string tokenId);
        List<WorkoutCategoryViewModel> GetByProfessional(Guid id, string tokenId);
        WorkoutCategoryViewModel Post(string tokenId, WorkoutCategoryRequestViewModel workoutCategoryRequestViewModel);
    }
}
