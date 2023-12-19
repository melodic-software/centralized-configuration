using Microsoft.Extensions.Hosting;
using static Enterprise.Hosting.Constants.EnvironmentConstants;

namespace Enterprise.Hosting.Extensions
{
    public static class IHostEnvironmentExtensions
    {
        /// <summary>
        /// Check if the current environment is the "Local" environment.
        /// <see cref="LocalEnvironment"/>
        /// </summary>
        /// <param name="webHost"></param>
        /// <returns></returns>
        public static bool IsLocal(this IHostEnvironment webHost)
        {
            return webHost.IsEnvironment(LocalEnvironment);
        }
    }
}
