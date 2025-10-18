using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels.ExerciseViewModels;

namespace Training.Application.Interfaces
{
    public interface IExerciseService
    {
        List<ExerciseViewModel> Get(string tokenId);
        ExerciseViewModel GetById(Guid id, string tokenId);
        List<ExerciseViewModel> GetByName(string name, string tokenId);
        List<ExerciseViewModel> GetByProfessional(Guid id, string tokenId);
        List<ExerciseViewModel> GetByMuscleGroup(Guid id, string tokenId);
        List<ExerciseViewModel> GetByWorkoutCategory(Guid id, string tokenId);
        ExerciseViewModel Post(string tokenId, ExerciseRequestViewModel exerciseRequestViewModel);
        ExerciseViewModel Put(string tokenId, ExerciseUpdateViewModel exerciseUpdateViewModel);
        void Delete(string tokenId, Guid id);
    }
}
