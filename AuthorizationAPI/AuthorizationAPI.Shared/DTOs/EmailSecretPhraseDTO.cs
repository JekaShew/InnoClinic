using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationAPI.Shared.DTOs
{
    public class EmailSecretPhrasePairDTO
    {
        public string Email { get; set; }
        public string SecretPhrase { get; set; }
    }
}
