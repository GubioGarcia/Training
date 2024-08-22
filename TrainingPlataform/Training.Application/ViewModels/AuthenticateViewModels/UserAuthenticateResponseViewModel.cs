using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Training.Application.ViewModels.ProfessionalViewModels;

namespace Training.Application.ViewModels.AuthenticateViewModels
{
    public class UserAuthenticateResponseViewModel
    {
        [JsonPropertyOrder(1)]
        public string Token { get; set; }
        [JsonPropertyOrder(2)]
        public DateTime ValidityToken { get; set; }
        [JsonPropertyOrder(3)]
        public Guid Id { get; set; }
        [JsonPropertyOrder(4)]
        public string Cpf { get; set; }
        [JsonPropertyOrder(5)]
        public string Name { get; set; }
        [JsonPropertyOrder(6)]
        public string UsersType { get; set; }

        public UserAuthenticateResponseViewModel(string token, DateTime validityToken, Guid id,
                                                 string cpf, string name, string usersType)
        {
            Token = token;
            ValidityToken = validityToken;
            Id = id;
            Cpf = cpf;
            Name = name;
            UsersType = usersType;
        }
    }
}
