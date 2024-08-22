using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels.AuthenticateViewModels;
using Training.Application.ViewModels.ProfessionalViewModels;

namespace Training.Application.Interfaces
{
    public interface IProfessionalService
    {
        List<ProfessionalMinimalFieldViewModel> Get();
        ProfessionalResponseViewModel GetByid(string id);
        bool Post(ProfessionalRequestViewModel professionalViewModel);
        bool Put(ProfessionalRequestUpdateViewModel professionalViewModel);
        bool Delete(string id);

        UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel professional);
    }
}
