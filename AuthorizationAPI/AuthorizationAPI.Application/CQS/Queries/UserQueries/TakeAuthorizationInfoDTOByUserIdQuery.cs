using AuthorizationAPI.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationAPI.Application.CQS.Queries.UserQueries
{
    public class TakeAuthorizationInfoDTOByUserIdQuery : IRequest<AuthorizationInfoDTO>
    {
        public Guid UserId { get; set; }
    }
}
