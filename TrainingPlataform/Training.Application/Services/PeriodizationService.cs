using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Template.CrossCutting.ExceptionHandler.Extensions;
using Training.Application.Interfaces;
using Training.Application.ViewModels;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Application.Services
{
    public class PeriodizationService : IPeriodizationService
    {

        private readonly IUserServiceBase<Professional> userServiceBaseProfessional;
        private readonly IUserServiceBase<Client> userServiceBaseClient;
        private readonly IPeriodizationRepository periodizationRepository;
        private readonly IMapper mapper;

        public PeriodizationService(IUserServiceBase<Professional> userServiceBaseProfessional, IUserServiceBase<Client> userServiceBaseClient,
                                     IMapper mapper, IPeriodizationRepository periodizationRepository)
        {
            this.userServiceBaseProfessional = userServiceBaseProfessional;
            this.userServiceBaseClient = userServiceBaseClient;
            this.periodizationRepository = periodizationRepository;
            this.mapper = mapper;
        }

        public List<PeriodizationViewModel> Get(string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                List<PeriodizationViewModel> _periodizationViewModels = [];

                IEnumerable<Periodization> _periodizations = this.periodizationRepository.GetAll();

                _periodizationViewModels = mapper.Map<List<PeriodizationViewModel>>(_periodizations);

                return _periodizationViewModels;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
