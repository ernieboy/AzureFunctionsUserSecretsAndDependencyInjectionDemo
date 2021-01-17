using FunctionApp1.Model;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

[assembly: FunctionsStartup(typeof(FunctionApp1.Startup))]

namespace FunctionApp1
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            var configuration = builder.GetContext().Configuration;

            builder.Services.AddOptions<AppSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(nameof(AppSettings)).Bind(settings);
            });

            var customerAge = int.Parse(configuration["AppSettings:CustomerAge"]);
            var customerGender = configuration["AppSettings:CustomerGender"];

            builder.Services.AddScoped<Customer>((s) => { return new Customer(customerAge, customerGender); });
            //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>(); } }
        }
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            var builtConfig = builder.ConfigurationBuilder.Build();
            var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];

            if (!string.IsNullOrEmpty(keyVaultEndpoint))
            {
                // using Key Vault, either local dev or deployed

                //var azureServiceTokenProvider = new AzureServiceTokenProvider();
                //var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                //builder.ConfigurationBuilder
                //        .AddAzureKeyVault(keyVaultEndpoint)
                //        .SetBasePath(Environment.CurrentDirectory)
                //        .AddJsonFile("local.settings.json", true)
                //        .AddEnvironmentVariables()
                //    .Build();
            }
            else
            {
                // local dev no Key Vault
                builder.ConfigurationBuilder
                   .SetBasePath(Environment.CurrentDirectory)
                   .AddJsonFile("local.settings.json", true)
                   .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                   .AddEnvironmentVariables()
                   .Build();
            }
        }
    }
}
