using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels.ProfessionalViewModels;

namespace Training.Application.ViewModels.AuthenticateViewModels
{
    public class UserAuthenticateResponseViewModel
    {
        public string Token { get; set; }
        public DateTime ValidityToken { get; set; }
        public Guid Id { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string UserType { get; set; }
        public string ProfessionalType { get; set; }

        public UserAuthenticateResponseViewModel(string token, DateTime validityToken, Guid id,
                                                 string cpf, string name, string userType, string professionalType)
        {
            Token = token;
            ValidityToken = validityToken;
            Id = id;
            Cpf = cpf;
            Name = name;
            UserType = userType;
            ProfessionalType = professionalType;
        }
    }
}
