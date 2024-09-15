using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Interfaces;
using Training.Domain.Models;

namespace Training.Domain.Entities
{
    public sealed class Client : EntityUsers, IIdentifiable
    {
        public Guid UsersTypeId { get; set; }
        public UsersType UsersType { get; set; }

        public DateTime DateBirth { get; set; }
        public string InitialObjective { get; set; }
        public decimal Height { get; set; }
        public decimal StartingWeight { get; set; }
        public decimal CurrentWeight { get; set; }
    }
}
