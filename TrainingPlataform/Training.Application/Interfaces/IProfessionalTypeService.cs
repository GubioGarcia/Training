using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;

namespace Training.Application.Interfaces
{
    public interface IProfessionalTypeService
    {
        List<ProfessionalTypeViewModel> Get(string tokenId);
        ProfessionalTypeViewModel GetById(string tokenId, string id);
        bool Post(string tokenId, ProfessionalTypeViewModel professionalTypeViewModel);
        bool Put(string tokenId, ProfessionalTypeViewModel professionalTypeViewModel);
        bool Delete(string tokenId, string id);
    }
}
