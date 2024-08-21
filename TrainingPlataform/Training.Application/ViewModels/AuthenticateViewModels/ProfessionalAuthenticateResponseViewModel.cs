using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels.ProfessionalViewModels;

namespace Training.Application.ViewModels.AuthenticateViewModels
{
    public class ProfessionalAuthenticateResponseViewModel : UserAuthenticateResponseViewModel
    {
        public string ProfessionalType { get; set; }

        public ProfessionalAuthenticateResponseViewModel(string token, DateTime validityToken, Guid id,
                                                         string cpf, string name, string usersType,
                                                         string professionalType)
            : base(token, validityToken, id, cpf, name, usersType)
        {
            ProfessionalType = professionalType;
        }
    }
}
