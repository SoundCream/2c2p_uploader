using System;
using _2C2P.FileUploader.Models.ConfigurationOptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace _2C2P.FileUploader.Extensions
{
    public static class ConfigurationVerifierExtension
    {
        public static void VerifyConfiguration(this IApplicationBuilder applicationBuilder)
        {
            var appConfiguration = applicationBuilder.ApplicationServices.GetService<IOptions<AppConfiguration>>();
            Verify(appConfiguration.Value);
        }

        private static void Verify(AppConfiguration appConfiguration)
        {
            if (string.IsNullOrWhiteSpace(appConfiguration.DateFormat))
            {
                throw new NullReferenceException($"{AppConfiguration.ConfigurataionName} DateFormat is missing value.");
            }

            if (appConfiguration.MaximunFilesize < 1)
            {
                throw new NullReferenceException($"{AppConfiguration.ConfigurataionName} MaximunFilesize cannot less than 1 byte.");
            }

            if (appConfiguration.AllowFileExtionsions == null)
            {
                throw new NullReferenceException($"{AppConfiguration.ConfigurataionName} AllowFileExtionsions is missing value");
            }

            if (appConfiguration.AllowFileExtionsions.Length < 1)
            {
                throw new NullReferenceException($"{AppConfiguration.ConfigurataionName} AllowFileExtionsions cannot less than 1 item.");
            }
        }
    }
}
