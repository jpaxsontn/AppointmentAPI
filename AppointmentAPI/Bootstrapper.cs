using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppointmentDomain;
using Nancy;

namespace AppointmentAPI
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            var initializeData = new SeedDataDomain();
            initializeData.SeedData();
        }
    }
}