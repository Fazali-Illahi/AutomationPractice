using Microsoft.Extensions.DependencyInjection;

namespace AutomationPractice.Core.DI.Containers;

public interface IServiceContainer
{
    void Register(IServiceCollection collection);
}