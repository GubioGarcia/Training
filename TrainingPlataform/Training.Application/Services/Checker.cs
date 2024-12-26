using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Template.CrossCutting.ExceptionHandler.Extensions;
using Training.Application.Interfaces;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Application.Services
{
    public class Checker : IChecker
    {
        private readonly IUsersTypeRepository usersTypeRepository;

        public Checker(IUsersTypeRepository usersTypeRepository)
        {
            this.usersTypeRepository = usersTypeRepository;
        }

        public bool isNumber(string aux)
        {
            try
            {
                char[] _auxDigits = aux.ToCharArray();
                foreach (char item in _auxDigits)
                {
                    if (!Char.IsDigit(item))
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        public bool isValidCpf(string cpf)
        {
            if (cpf == null)
                throw new ApiException("CPF is required", HttpStatusCode.BadRequest);

            if (cpf.Length != 11)
                throw new ApiException("CPF is not valid", HttpStatusCode.BadRequest);

            if (!isNumber(cpf))
                throw new ApiException("CPF is not valid. Please enter only numeric digits.", HttpStatusCode.BadRequest);

            try
            {
                char[] _cpfDigits = cpf.ToCharArray();

                #region Digit Checker 1

                int digitCheckerOne = 0;
                for (int i = 0, f = 10; i < 9; i++, f--)
                {
                    digitCheckerOne += (int)char.GetNumericValue(_cpfDigits[i]) * f;
                }
                digitCheckerOne = (digitCheckerOne * 10) % 11;
                if (digitCheckerOne == 10)
                    digitCheckerOne = 0;

                #endregion

                #region Digit Checker 2

                int digitCheckerTwo = 0;
                for (int i = 0, f = 11; i < 10; i++, f--)
                {
                    digitCheckerTwo += (int)char.GetNumericValue(_cpfDigits[i]) * f;
                }
                digitCheckerTwo = (digitCheckerTwo * 10) % 11;
                if (digitCheckerTwo == 10)
                    digitCheckerTwo = 0;

                #endregion

                if (digitCheckerOne != (int)char.GetNumericValue(_cpfDigits[9]) || digitCheckerTwo != (int)char.GetNumericValue(_cpfDigits[10]))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        public bool isValidFone(string fone)
        {
            if (fone == null)
                throw new ApiException("Fone is required", HttpStatusCode.BadRequest);

            if (fone.Length < 8 || fone.Length > 13)
                throw new ApiException("Fone is not valid", HttpStatusCode.BadRequest);

            return isNumber(fone);
        }
    }
}
