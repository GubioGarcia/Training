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
        //WorkoutCategoryViewModel Post(string tokenId, WorkoutCategoryRequestViewModel workoutCategoryRequestViewModel);
    }
}
