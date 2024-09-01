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

        public ProfessionalService(IProfessionalRepository professionalRepository, IUsersTypeRepository usersTypeRepository, IProfessionalTypeRepository professionalTypeRepository, 
                                   IMapper mapper, IChecker checker, ManualMapperSetup manualMapper) 
        {
            this.professionalRepository = professionalRepository;
            this.usersTypeRepository = usersTypeRepository;
            this.professionalTypeRepository = professionalTypeRepository;
            this.mapper = mapper;
            this.checker = checker;
            this.manualMapper = manualMapper;
        }

        public List<ProfessionalMinimalFieldViewModel> Get(string tokenId)
        {
            this.checker.IsValidUserType(this.PullUsersTypeId(tokenId), "Admin");

            List<ProfessionalMinimalFieldViewModel> _professionalMinimalFieldViewModels = new List<ProfessionalMinimalFieldViewModel>();

            IEnumerable<Professional> _professionals = this.professionalRepository.GetAll();

            _professionalMinimalFieldViewModels = mapper.Map<List<ProfessionalMinimalFieldViewModel>>(_professionals);
            
            return _professionalMinimalFieldViewModels;
        }

        public ProfessionalResponseViewModel GetByid(string id, string tokenId)
        {
            this.checker.IsValidUserType(this.PullUsersTypeId(tokenId), "Admin");

            if (!Guid.TryParse(id, out Guid professionalId))
                throw new Exception("Id is not valid");

            Professional _professional = this.professionalRepository.Find(x => x.Id == professionalId && !x.IsDeleted);
            if (_professional == null)
                throw new Exception("Professional not found");

            return mapper.Map<ProfessionalResponseViewModel>(_professional);
        }

        public ProfessionalResponseViewModel GetByCpf(string cpf, string tokenId)
        {
            this.checker.IsValidUserType(this.PullUsersTypeId(tokenId), "Admin");

            if (!checker.isValidCpf(cpf))
                throw new Exception("CPF is not valid");

            Professional _professional = this.professionalRepository.Find(x => x.Cpf == cpf && !x.IsDeleted);
            if (_professional == null)
                throw new Exception("Professional not found");

            return mapper.Map<ProfessionalResponseViewModel>(_professional);
        }

        public List<ProfessionalMinimalFieldViewModel> GetByName(string name, string tokenId)
        {
            this.checker.IsValidUserType(this.PullUsersTypeId(tokenId), "Admin");

            if (string.IsNullOrEmpty(name))
                throw new Exception("Name is required");

            List<ProfessionalMinimalFieldViewModel> _professionalMinimalFieldViewModels = new List<ProfessionalMinimalFieldViewModel>();

            IEnumerable<Professional> _professionals = this.professionalRepository.Query(p => EF.Functions.Like(p.Name, $"%{name}%") && !p.IsDeleted);

            _professionalMinimalFieldViewModels = mapper.Map<List<ProfessionalMinimalFieldViewModel>>(_professionals);

            return _professionalMinimalFieldViewModels;
        }

        public ProfessionalMinimalFieldViewModel Post(ProfessionalRequestViewModel professionalRequestViewModel, string tokenId)
        {
            this.checker.IsValidUserType(this.PullUsersTypeId(tokenId), "Admin");

            if (!checker.isValidCpf(professionalRequestViewModel.Cpf))
                throw new Exception("CPF is not valid");

            Professional _professional = this.professionalRepository.Find(x => x.Cpf == professionalRequestViewModel.Cpf && !x.IsDeleted);
            if (_professional != null)
                throw new Exception("There is already a professional registered with this CPF");

            if (!checker.isValidFone(professionalRequestViewModel.Fone))
                throw new Exception("Phone is not valid");
            
            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == professionalRequestViewModel.UsersTypeId && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("Id type user not found");

            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalRequestViewModel.ProfessionalTypesId 
                                                                                     && !x.IsDeleted);
            if (_professionalType == null)
                throw new Exception("Id professional type not found");

            _professional = mapper.Map<Professional>(professionalRequestViewModel);
            _professional.Password = this.HashPassword(_professional.Password);
            _professional.DateRegistration = DateTime.Now;

            if (professionalRequestViewModel.UrlProfilePhoto == "")
                _professional.UrlProfilePhoto = null;
            
            this.professionalRepository.Create(_professional);

            return mapper.Map<ProfessionalMinimalFieldViewModel>(_professional);
        }

        public ProfessionalResponseViewModel Put(ProfessionalRequestUpdateViewModel professionalRequestUpdateViewModel, string tokenId)
        {
            this.checker.IsValidUserType(this.PullUsersTypeId(tokenId), "Admin");

            Professional _professional = this.professionalRepository.Find(x => x.Id == professionalRequestUpdateViewModel.Id && !x.IsDeleted);
            if (_professional == null)
                throw new Exception("Professional not found");

            if (professionalRequestUpdateViewModel.Cpf != null)
            {
                if (!checker.isValidCpf(professionalRequestUpdateViewModel.Cpf))
                    throw new Exception("CPF is not valid");

                Professional _auxProfessional = this.professionalRepository.Find(x => x.Cpf == professionalRequestUpdateViewModel.Cpf);
                if (_auxProfessional != null && _auxProfessional.Id != _professional.Id)
                    throw new Exception("There is already a professional registered with this CPF");
            }

            if (professionalRequestUpdateViewModel.ProfessionalTypeId != null)
            {
                ProfessionalType professionalType = this.professionalTypeRepository.Find(
                                                         x => x.Id == professionalRequestUpdateViewModel.ProfessionalTypeId && !x.IsDeleted);
                if (professionalType == null)
                    throw new Exception("Id professional type not found");
            }

            if (professionalRequestUpdateViewModel.Fone != null)
                if (!checker.isValidFone(professionalRequestUpdateViewModel.Fone))
                    throw new Exception("Phone is not valid");
            
            manualMapper.MapProfessionalRequestUpdateToProfessional(professionalRequestUpdateViewModel, _professional);
            _professional.DateUpdated = DateTime.Now;

            if (professionalRequestUpdateViewModel.Password != null)
                _professional.Password = this.HashPassword(_professional.Password);         

            this.professionalRepository.Update(_professional);

            return mapper.Map<ProfessionalResponseViewModel>(_professional); ;
        }

        public bool Delete(string id, string tokenId)
        {
            this.checker.IsValidUserType(this.PullUsersTypeId(tokenId), "Admin");

            if (!Guid.TryParse(id, out Guid professionalId))
                throw new Exception("Id is not valid");

            Professional _professional = this.professionalRepository.Find(x => x.Id == professionalId && !x.IsDeleted);
            if (_professional == null)
                throw new Exception("Professional not found");

            return this.professionalRepository.Delete(_professional);
        }

        public UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel professional)
        {
            if(string.IsNullOrEmpty(professional.Cpf) || string.IsNullOrEmpty(professional.Password))
                throw new Exception("CPF/Password is required");

            if (!checker.isValidCpf(professional.Cpf))
                throw new Exception("CPF is not valid");

            Professional _professional = this.professionalRepository.Find(x => x.Cpf == professional.Cpf && !x.IsDeleted);
            
            if (_professional == null || !this.VerifyPassword(professional.Password, _professional.Password))
                throw new Exception("Invalid credentials");
            
            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == _professional.UsersTypeId && !x.IsDeleted);
            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == _professional.ProfessionalTypesId && !x.IsDeleted );

            var (_token, _expiry) = TokenService.GenerateToken(_professional);

            return new ProfessionalAuthenticateResponseViewModel(_token, _expiry, _professional.Id, _professional.Cpf, _professional.Name,
                                                         _usersType.Name, _professionalType.Name);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public Guid PullUsersTypeId(string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid id))
                throw new Exception("Id is not valid");

            Professional _professional = this.professionalRepository.Find(x => x.Id == id && !x.IsDeleted);
            if (_professional == null)
                throw new Exception("Professional not found");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == _professional.UsersTypeId && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("User type not found");

            return _usersType.Id;
        }
    }
}
