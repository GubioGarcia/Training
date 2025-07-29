using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels.MuscleGroupViewModels;

namespace Training.Application.Interfaces
{
    public interface IMuscleGroupService
    {
        List<MuscleGroupViewModel> Get(string tokenId);
        MuscleGroupViewModel GetById(Guid id, string tokenId);
        List<MuscleGroupViewModel> GetByName(string name, string tokenId);
        List<MuscleGroupViewModel> GetByProfessional(Guid id, string tokenId);
        MuscleGroupViewModel Post(string tokenId, MuscleGroupRequestViewModel muscleGroupRequestViewModel);
        MuscleGroupViewModel Put(string tokenId, MuscleGroupUpdateViewModel muscleGroupUpdateViewModel);
        void Delete(string tokenId, Guid id);
    }
}
