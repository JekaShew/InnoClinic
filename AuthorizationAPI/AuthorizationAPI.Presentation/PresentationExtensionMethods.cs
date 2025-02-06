using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationAPI.Presentation
{
        public static class PresentationExtensionMethods
        {

            public static IServiceCollection AddPresentationControllers(this IServiceCollection services)
            {
                services.AddControllers()
                    .AddApplicationPart(typeof(Presentation.ReferenceAssembly).Assembly);

            return services;
            }
        }
}
