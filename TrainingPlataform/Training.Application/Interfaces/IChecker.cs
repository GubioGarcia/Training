using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.Interfaces
{
    public interface IChecker
    {
        bool isNumber(string aux);
        bool isValidCpf(string cpf);
        bool isValidFone(string fone);
        bool IsValidUserType(Guid usersTypeId, string type);
        //bool ValidUserType(string id, string userType);
    }
}
