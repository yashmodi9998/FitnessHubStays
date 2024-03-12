using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FitnessHubStays.Startup))]
namespace FitnessHubStays
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
