using Microsoft.Extensions.DependencyInjection;

namespace TestAutomation.Examples.ConfigurationProviders;

public interface IConfigurationProvider
{
    ServiceCollection GetConfiguration();
}