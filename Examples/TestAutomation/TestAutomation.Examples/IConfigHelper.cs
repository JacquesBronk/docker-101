using Microsoft.Extensions.DependencyInjection;

namespace TestAutomation.Examples;

public interface IConfigHelper
{
    ServiceProvider ServiceProvider { get; }
}