using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.API.Versioning.Options
{
    public class VersioningConfigurationOptions
    {
        /// <summary>
        /// Enables support for versioning via a url segment (like Google and Facebook) -> "api/v1/resource".
        /// </summary>
        public bool EnableUrlVersioning { get; set; } = true;

        /// <summary>
        /// Enables specifying the version via a query string parameter.
        /// This is the out-of-the-box default .NET strategy.
        /// </summary>
        public bool EnableQueryStringVersioning { get; set; } = true;

        /// <summary>
        /// This uses a custom HTTP header.
        /// </summary>
        public bool EnableHeaderVersioning { get; set; } = true;

        /// <summary>
        /// Defaults to "application/json;v=2.0" but can be specified in the constructor (something like "version" or "v").
        /// </summary>
        public bool EnableMediaTypeVersioning { get; set; } = true;
    }
}
