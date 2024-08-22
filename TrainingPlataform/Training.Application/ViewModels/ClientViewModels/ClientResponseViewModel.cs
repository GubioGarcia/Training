using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.ClientViewModels
{
    public class ClientResponseViewModel : ClientMinimalFieldViewModel
    {
        [JsonPropertyOrder(4)]
        public string Fone { get; set; }
        [JsonPropertyOrder(5)]
        public string InitialObjective { get; set; }
        [JsonPropertyOrder(6)]
        public DateTime DateRegistration { get; set; }
        [JsonPropertyOrder(7)]
        public bool IsActive { get; set; }
    }
}
