using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.ProfessionalViewModels
{
    public class ProfessionalMinimalFieldViewModel
    {
        [JsonPropertyOrder(1)]
        public Guid Id { get; set; }
        [JsonPropertyOrder(2)]
        public string Cpf { get; set; }
        [JsonPropertyOrder(3)]
        public string Name { get; set; }
    }
}
