using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triarch.BusinessLogic.Models.Definitions;

namespace Triarch.BusinessLogic.Services;
public class RPGSystemProvider : IRPGSystemProvider
{
    private Dictionary<string, RPGSystem> _systemList = new();

    public RPGSystem LoadSystem(string name)
    {
        return _systemList[name];
    }

    public void AddSystem(string name, RPGSystem system)
    {
        _systemList.Add(name, system);
    }
}
