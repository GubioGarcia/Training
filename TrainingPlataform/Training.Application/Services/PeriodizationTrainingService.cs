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
    public class PeriodizationTrainingService : IPeriodizationTrainingService
    {

        private readonly IUserServiceBase<Professional> userServiceBaseProfessional;
        private readonly IUserServiceBase<Client> userServiceBaseClient;
        private readonly IPeriodizationTrainingRepository periodizationTrainingRepository;
        private readonly IMapper mapper;

        public PeriodizationTrainingService(IUserServiceBase<Professional> userServiceBaseProfessional, IUserServiceBase<Client> userServiceBaseClient,
                                     IMapper mapper, IPeriodizationTrainingRepository periodizationTrainingRepository)
        {
            this.userServiceBaseProfessional = userServiceBaseProfessional;
            this.userServiceBaseClient = userServiceBaseClient;
            this.periodizationTrainingRepository = periodizationTrainingRepository;
            this.mapper = mapper;
        }

        public List<PeriodizationTrainingViewModel> Get(string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                List<PeriodizationTrainingViewModel> _periodizationTrainingViewModels = [];

                IEnumerable<PeriodizationTraining> _periodizationTrainings = this.periodizationTrainingRepository.GetAll();

                _periodizationTrainingViewModels = mapper.Map<List<PeriodizationTrainingViewModel>>(_periodizationTrainings);

                return _periodizationTrainingViewModels;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
