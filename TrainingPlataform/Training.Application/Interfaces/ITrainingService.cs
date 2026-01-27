using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels.TrainingViewModels;
using Training.Application.ViewModels.WorkoutCategoryViewModels;

namespace Training.Application.Interfaces
{
    public interface ITrainingService
    {
        List<TrainingViewModel> Get(string tokenId);
        TrainingViewModel GetById(Guid id, string tokenId);
        List<TrainingViewModel> GetByName(string name, string tokenId);
        List<TrainingViewModel> GetByProfessional(Guid id, string tokenId);
        TrainingViewModel Post(string tokenId, TrainingRequestViewModel trainingRequestViewModel);
        TrainingViewModel Put(string tokenId, TrainingUpdateViewModel trainingUpdateViewModel);
        void Delete(string tokenId, Guid id);
    }
}
