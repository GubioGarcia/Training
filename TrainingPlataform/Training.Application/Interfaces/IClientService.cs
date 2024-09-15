using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;
using Training.Application.ViewModels.AuthenticateViewModels;
using Training.Application.ViewModels.ClientViewModels;

namespace Training.Application.Interfaces
{
    public interface IClientService
    {
        List<ClientMinimalFieldViewModel> Get(string tokenId);
        ClientResponseViewModel GetById(string id, string tokenId);
        ClientResponseViewModel GetByCpf(string cpf, string tokenId);
        List<ClientMinimalFieldViewModel> GetByName(string name, string tokenId);
        ClientMinimalFieldViewModel Post(ClientRequestViewModel clientRequestViewModel, string tokenId);
        ClientResponseViewModel Put(ClientRequestUpdateViewModel ClientRequestUpdateViewModel, string tokenId);
        bool Delete(string id);
        UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel client);
    }
}
