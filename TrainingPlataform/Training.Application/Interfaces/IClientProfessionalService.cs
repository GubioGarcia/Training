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
        List<ClientProfessionalViewModel> Get(string tokenId);
        ClientProfessionalViewModel GetById(string tokenId, string id);
        List<ClientProfessionalViewModel> GetClientsByProfessionalId(string tokenId, string id);
        ClientProfessionalViewModel Post(string tokenId, ClientProfessionalRequestViewModel clientProfessionalRequestViewModels);
        ClientProfessionalViewModel Put(string tokenId, ClientProfessionalRequestUpdateViewModel clientProfessionalRequestUpdateViewModels);
        bool Delete(string tokenId, string professionalId, string clientId);
    }
}
