using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationAPI.Application.DTOs
{
    public class RegistrationInfoDTO
    {
        public Guid Id { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SecretPhrase { get; set; }
    }
}
