using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ministryofjusticeWebUi.Startup))]

namespace ministryofjusticeWebUi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}