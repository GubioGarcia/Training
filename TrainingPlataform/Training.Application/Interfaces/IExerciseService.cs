using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;
using Training.Application.ViewModels.MuscleGroupViewModels;

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
    }
}
