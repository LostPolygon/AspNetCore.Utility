using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Ballast.Atlantis.Utility {
    public abstract class AppConfigurator {
        protected IApplicationBuilder ApplicationBuilder { get; }
        protected IWebHostEnvironment Environment { get; }

        protected AppConfigurator(IApplicationBuilder applicationBuilder, IWebHostEnvironment environment) {
            ApplicationBuilder = applicationBuilder;
            Environment = environment;
        }

        public abstract void Configure();
    }
}
