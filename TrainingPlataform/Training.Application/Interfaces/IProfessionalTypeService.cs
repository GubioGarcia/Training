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
        List<ProfessionalTypeViewModel> Get();
        ProfessionalTypeViewModel GetById(string id);
        bool Post(ProfessionalTypeViewModel professionalTypeViewModel);
        bool Put(ProfessionalTypeViewModel professionalTypeViewModel);
        bool Delete(string id);
    }
}
