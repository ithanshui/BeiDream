﻿using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BeiDream.Web.Mvc.Dependency
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// Reference to DI kernel.
        /// </summary>
        private readonly IKernel _kernel; //TODO: Remove this and use IocHelper?

        /// <summary>
        /// Creates a new instance of WindsorControllerFactory.
        /// </summary>
        /// <param name="kernel">Reference to DI kernel</param>
        public WindsorControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        /// <summary>
        /// Called by MVC system and releases/disposes given controller instance.
        /// </summary>
        /// <param name="controller">Controller instance</param>
        public override void ReleaseController(IController controller)
        {
            _kernel.ReleaseComponent(controller);
        }

        /// <summary>
        /// Called by MVC system and creates controller instance for given controller type.
        /// </summary>
        /// <param name="requestContext">Request context</param>
        /// <param name="controllerType">Controller type</param>
        /// <returns></returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
            }
            //开启单次请求周期作用域范围
            using (var scope = _kernel.BeginScope())
            {
                return (IController)_kernel.Resolve(controllerType);
            }
        }
    }
}