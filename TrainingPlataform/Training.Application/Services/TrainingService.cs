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
    public class TrainingService : ITrainingService
    {
        private readonly IUserServiceBase<Professional> userServiceBase;
        private readonly ITrainingRepository trainingRepository;
        private readonly IMapper mapper;

        public TrainingService(IUserServiceBase<Professional> userServiceBase, ITrainingRepository trainingRepository, IMapper mapper)
        {
            this.userServiceBase = userServiceBase;
            this.trainingRepository = trainingRepository;
            this.mapper = mapper;
        }

        public List<TrainingViewModel> Get(string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                List<TrainingViewModel> _trainingViewModels = [];

                IEnumerable<Domain.Entities.Training> _trainings = this.trainingRepository.GetAll();

                _trainingViewModels = mapper.Map<List<TrainingViewModel>>(_trainings);

                return _trainingViewModels;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
