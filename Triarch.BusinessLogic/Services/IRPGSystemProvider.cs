using Triarch.BusinessLogic.Models.Definitions;

namespace Triarch.BusinessLogic.Services;
public interface IRPGSystemProvider
{
    void AddSystem(string name, RPGSystem system);
    RPGSystem LoadSystem(string name);
}