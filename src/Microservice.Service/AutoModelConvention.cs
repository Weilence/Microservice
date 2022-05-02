using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Microservice.Service
{
    public class AutoModelConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var serviceType = controller.ControllerType
                .GetInterfaces()
                .FirstOrDefault(m => CustomAttributeExtensions.GetCustomAttributes<HttpAttribute>(m).Any());

            if (serviceType == null)
            {
                return;
            }

            if (controller.ControllerName.EndsWith("Service"))
            {
                controller.ControllerName =
                    controller.ControllerName.Substring(0, controller.ControllerName.Length - 7);
            }

            foreach (var selector in controller.Selectors)
            {
                var httpAttribute = serviceType.GetCustomAttribute<HttpAttribute>();
                if (string.IsNullOrEmpty(httpAttribute.Template))
                {
                    httpAttribute.Template = "[controller]/[action]";
                }

                selector.AttributeRouteModel = new AttributeRouteModel(
                    new RouteAttribute(httpAttribute.Template));
            }

            foreach (var action in controller.Actions)
            {
                foreach (var method in serviceType.GetMethods())
                {
                    if (action.ActionMethod.ToString() == method.ToString())
                    {
                        var httpAttribute = method.GetCustomAttribute<HttpAttribute>();
                        if (!string.IsNullOrEmpty(httpAttribute.Template))
                        {
                            action.Selectors[0].AttributeRouteModel =
                                new AttributeRouteModel(new RouteAttribute(httpAttribute.Template));
                        }

                        var type = Assembly.Load("Microsoft.AspNetCore.Mvc.Core")
                            .GetType("Microsoft.AspNetCore.Mvc.ActionConstraints.HttpMethodActionConstraint");
                        var instance =
                            Activator.CreateInstance(type, new object[] { new[] { httpAttribute.Method } }) as
                                IActionConstraint;

                        action.Selectors[0].ActionConstraints.Add(instance);
                    }
                }
            }
        }
    }
}