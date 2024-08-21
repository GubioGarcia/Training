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
        List<ClientMinimalFieldViewModel> Get();
        ClientResponseViewModel GetById(string id);
        bool Post(ClientRequestViewModel clientRequestViewModel);
        bool Put(ClientUpdateRequestViewModel ClientUpdateRequestViewModel);
        bool Delete(string id);
        UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel client);
    }
}
