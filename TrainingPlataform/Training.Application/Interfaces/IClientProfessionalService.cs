using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels.ClientProfessionalViewModels;

namespace Training.Application.Interfaces
{
    public interface IClientProfessionalService
    {
        List<ClientProfessionalViewModel> Get();
        ClientProfessionalViewModel GetById(string id);
        List<ClientProfessionalViewModel> GetClientsByProfessionalId(string id);
        ClientProfessionalViewModel Post(ClientProfessionalRequestViewModel clientProfessionalRequestViewModels);
        ClientProfessionalViewModel Put(ClientProfessionalRequestUpdateViewModel clientProfessionalRequestUpdateViewModels);
        bool Delete(string tokenId, string professionalId, string clientId);
    }
}
