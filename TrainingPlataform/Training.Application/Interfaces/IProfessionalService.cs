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
        List<ProfessionalMinimalFieldViewModel> Get(string tokenId);
        ProfessionalResponseViewModel GetByid(string id, string tokenId);
        public ProfessionalResponseViewModel GetByCpf(string cpf, string tokenId);
        List<ProfessionalMinimalFieldViewModel> GetByName(string name, string tokenId);
        ProfessionalMinimalFieldViewModel Post(ProfessionalRequestViewModel professionalViewModel, string tokenId);
        ProfessionalResponseViewModel Put(ProfessionalRequestUpdateViewModel professionalViewModel, string tokenId);
        bool Delete(string id, string tokenId);

        UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel professional);
        Guid PullUsersTypeId(string tokenId);
    }
}
