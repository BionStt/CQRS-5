using CQRS.Behaviours;
using CQRS.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CQRS.DependencyInjection
{
    public static class DependencyInjection
    { 
        public static void AddDefaultBehaviours(this IServiceCollection services)
        {
            // Add the MediatR behaviour pipeline. Will execute in order of registration.
            // See: https://github.com/jbogard/MediatR/wiki/Behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorisationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        public static void AddValidators(this IServiceCollection services, Assembly assembly)
        {
            var authorizerType = typeof(IValidator<>);
            assembly.GetTypesAssignableTo(authorizerType).ForEach((type) =>
            {
                foreach (var implementedInterface in type.ImplementedInterfaces)
                {
                    services.AddTransient(implementedInterface, type);
                }
            });
        }

        private static List<TypeInfo> GetTypesAssignableTo(this Assembly assembly, Type compareType)
        {
            var typeInfoList = assembly.DefinedTypes.Where(x => x.IsClass
                                && !x.IsAbstract
                                && x != compareType
                                && x.GetInterfaces()
                                        .Any(i => i.IsGenericType
                                                && i.GetGenericTypeDefinition() == compareType))?.ToList();

            return typeInfoList;
        }
    }
}
