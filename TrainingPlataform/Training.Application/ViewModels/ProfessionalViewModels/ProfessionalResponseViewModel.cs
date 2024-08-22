using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.ProfessionalViewModels
{
    public class ProfessionalResponseViewModel : ProfessionalMinimalFieldViewModel
    {
        [JsonPropertyOrder(4)]
        public string Password { get; set; }
        [JsonPropertyOrder(5)]
        public Guid UsersTypeId { get; set; }
        [JsonPropertyOrder(6)]
        public Guid ProfessionalTypeId { get; set; }
        [JsonPropertyOrder(7)]
        public string Fone { get; set; }
        [JsonPropertyOrder(8)]
        public string CurrentNumberClients { get; set; }
        [JsonPropertyOrder(9)]
        public string? ProfessionalRegistration { get; set; }
        [JsonPropertyOrder(10)]
        public string? UrlProfilePhoto { get; set; }
    }
}
