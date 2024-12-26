using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.Interfaces;
using Training.Domain.Entities;
using Training.Domain.Interfaces;
using Training.Auth.Services;
using Training.Application.ViewModels.ProfessionalViewModels;
using Training.Application.ViewModels.AuthenticateViewModels;
using Training.Application.Mapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Training.Domain.Models;
using Template.CrossCutting.ExceptionHandler.Extensions;
using System.Net;
using System.Xml.Linq;

namespace Training.Application.Services
{
    public class ProfessionalService : IProfessionalService
    {
        private readonly IProfessionalRepository professionalRepository;
        private readonly IUsersTypeRepository usersTypeRepository;
        private readonly IProfessionalTypeRepository professionalTypeRepository;
        private readonly IMapper mapper;
        private readonly IChecker checker;
        private readonly ManualMapperSetup manualMapper;
        private readonly IUserServiceBase<Professional> userServiceBase;

        public ProfessionalService(IProfessionalRepository professionalRepository, IUsersTypeRepository usersTypeRepository, IProfessionalTypeRepository professionalTypeRepository, 
                                   IMapper mapper, IChecker checker, ManualMapperSetup manualMapper, IUserServiceBase<Professional> userServiceBase) 
        {
            this.professionalRepository = professionalRepository;
            this.usersTypeRepository = usersTypeRepository;
            this.professionalTypeRepository = professionalTypeRepository;
            this.userServiceBase = userServiceBase;
            this.mapper = mapper;
            this.checker = checker;
            this.manualMapper = manualMapper;
        }

        public List<ProfessionalMinimalFieldViewModel> Get(string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                IEnumerable<Professional> _professionals = this.professionalRepository.GetAll();
                return mapper.Map<List<ProfessionalMinimalFieldViewModel>>(_professionals);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public ProfessionalResponseViewModel GetByid(Guid professionalid, string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            Professional _professional = this.professionalRepository.Find(x => x.Id == professionalid && !x.IsDeleted);
            if (_professional == null)
                throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            return mapper.Map<ProfessionalResponseViewModel>(_professional);
        }

        public ProfessionalResponseViewModel GetByCpf(string cpf, string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            if (!checker.isValidCpf(cpf))
                throw new ApiException("CPF is not valid", HttpStatusCode.BadRequest);

            Professional _professional = this.professionalRepository.Find(x => x.Cpf == cpf && !x.IsDeleted);
            if (_professional == null)
                throw new ApiException("Professional not found", HttpStatusCode.NotFound);
            
            try
            {
                return mapper.Map<ProfessionalResponseViewModel>(_professional);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public List<ProfessionalMinimalFieldViewModel> GetByName(string name, string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            if (string.IsNullOrEmpty(name))
                throw new ApiException("Name is required", HttpStatusCode.BadRequest);

            try
            {
                List<ProfessionalMinimalFieldViewModel> _professionalMinimalFieldViewModels = new List<ProfessionalMinimalFieldViewModel>();

                IEnumerable<Professional> _professionals = this.professionalRepository.Query(p => EF.Functions.Like(p.Name, $"%{name}%") && !p.IsDeleted);

                _professionalMinimalFieldViewModels = mapper.Map<List<ProfessionalMinimalFieldViewModel>>(_professionals);

                return _professionalMinimalFieldViewModels;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public ProfessionalMinimalFieldViewModel Post(ProfessionalRequestViewModel professionalRequestViewModel, string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            if (!checker.isValidCpf(professionalRequestViewModel.Cpf))
                throw new ApiException("CPF is not valid", HttpStatusCode.BadRequest);

            Professional _professional = this.professionalRepository.Find(x => x.Cpf == professionalRequestViewModel.Cpf && !x.IsDeleted);
            if (_professional != null)
                throw new ApiException("There is already a professional registered with this CPF", HttpStatusCode.BadRequest);

            if (!checker.isValidFone(professionalRequestViewModel.Fone))
                throw new ApiException("Phone is not valid", HttpStatusCode.BadRequest);
            
            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == professionalRequestViewModel.UsersTypeId && !x.IsDeleted);
            if (_usersType == null)
                throw new ApiException("Id type user not found", HttpStatusCode.NotFound);

            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalRequestViewModel.ProfessionalTypesId 
                                                                                     && !x.IsDeleted);
            if (_professionalType == null)
                throw new ApiException("Id professional type not found", HttpStatusCode.NotFound);

            try
            {
                _professional = mapper.Map<Professional>(professionalRequestViewModel);
                _professional.Password = this.HashPassword(_professional.Password);
                _professional.DateRegistration = DateTime.Now;

                if (professionalRequestViewModel.UrlProfilePhoto == "")
                    _professional.UrlProfilePhoto = null;

                this.professionalRepository.Create(_professional);

                return mapper.Map<ProfessionalMinimalFieldViewModel>(_professional);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public ProfessionalResponseViewModel Put(ProfessionalRequestUpdateViewModel professionalRequestUpdateViewModel, string tokenId)
        {
            // Verifica se o ID é válido
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Recebe o tipo do usuário logado e instância profissional logado
            string loggedInUserType = this.userServiceBase.LoggedInUserType(tokenId);
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            // Se o usuário logado for do tipo 'Professional', usuário só pode alterar seu próprio registro
            if (loggedInUserType.Equals("Professional", StringComparison.OrdinalIgnoreCase))
            {
                if (_professionalLogged.Id != professionalRequestUpdateViewModel.Id)
                    throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);
            }
            // Se o usuário logado não for 'Admin' ou 'Professional', a operação não é autorizada
            else if (!loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);
            }

            // Busca o profissional a ser atualizado
            Professional _professional = this.professionalRepository.Find(x => x.Id == professionalRequestUpdateViewModel.Id && !x.IsDeleted)
                                        ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            if (professionalRequestUpdateViewModel.Cpf != null)
            {
                if (!checker.isValidCpf(professionalRequestUpdateViewModel.Cpf))
                    throw new ApiException("CPF is not valid", HttpStatusCode.BadRequest);

                Professional _auxProfessional = this.professionalRepository.Find(x => x.Cpf == professionalRequestUpdateViewModel.Cpf);
                if (_auxProfessional != null && _auxProfessional.Id != _professional.Id)
                    throw new ApiException("There is already a professional registered with this CPF", HttpStatusCode.BadRequest);
            }

            if (professionalRequestUpdateViewModel.ProfessionalTypeId != null)
            {
                ProfessionalType professionalType = this.professionalTypeRepository.Find(
                                                         x => x.Id == professionalRequestUpdateViewModel.ProfessionalTypeId && !x.IsDeleted);
                if (professionalType == null)
                    throw new ApiException("Id professional type not found", HttpStatusCode.NotFound);
            }

            if (professionalRequestUpdateViewModel.Fone != null)
                if (!checker.isValidFone(professionalRequestUpdateViewModel.Fone))
                    throw new ApiException("Phone is not valid", HttpStatusCode.BadRequest);

            try
            {
                manualMapper.MapProfessionalRequestUpdateToProfessional(professionalRequestUpdateViewModel, _professional);
                _professional.DateUpdated = DateTime.Now;

                if (professionalRequestUpdateViewModel.Password != null)
                    _professional.Password = this.HashPassword(_professional.Password);

                this.professionalRepository.Update(_professional);

                return mapper.Map<ProfessionalResponseViewModel>(_professional);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public bool Delete(string id, string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            if (!Guid.TryParse(id, out Guid professionalId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            Professional _professional = this.professionalRepository.Find(x => x.Id == professionalId && !x.IsDeleted);
            if (_professional == null)
                throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            return this.professionalRepository.Delete(_professional);
        }

        public UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel professional)
        {
            if(string.IsNullOrEmpty(professional.Cpf) || string.IsNullOrEmpty(professional.Password))
                throw new ApiException("CPF/Password is required", HttpStatusCode.BadRequest);

            if (!checker.isValidCpf(professional.Cpf))
                throw new ApiException("CPF is not valid", HttpStatusCode.BadRequest);

            Professional _professional = this.professionalRepository.Find(x => x.Cpf == professional.Cpf && !x.IsDeleted);
            
            if (_professional == null || !this.VerifyPassword(professional.Password, _professional.Password))
                throw new ApiException("Invalid credentials", HttpStatusCode.BadRequest);

            try
            {
                UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == _professional.UsersTypeId && !x.IsDeleted);
                ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == _professional.ProfessionalTypesId && !x.IsDeleted);

                var (_token, _expiry) = TokenService.GenerateToken(_professional);

                return new ProfessionalAuthenticateResponseViewModel(_token, _expiry, _professional.Id, _professional.Cpf, _professional.Name,
                                                             _usersType.Name, _professionalType.Name);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
