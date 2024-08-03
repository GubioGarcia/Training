using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Domain.Models
{
    public class EntityUsers
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Cpf { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime DateRegistration { get; set; }
        public string Fone { get; set; }
        public string? UrlProfilePhoto { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;

        private readonly IPasswordHasher _passwordHasher;
        private string _passwordHash;

        public EntityUsers(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public void SetPassword(string password)
        {
            if (_passwordHasher == null)
                throw new InvalidOperationException("Password hasher not initialized.");

            _passwordHash = _passwordHasher.HashPassword(password);
        }

        public bool VerifyPassword(string password)
        {
            if (_passwordHasher == null)
                throw new InvalidOperationException("Password hasher not initialized.");

            return _passwordHasher.VerifyPassword(password, _passwordHash);
        }
    }
}
