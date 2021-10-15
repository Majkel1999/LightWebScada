using System.Linq;
using DataRegisters;
using Newtonsoft.Json;

namespace FrontEnd.DataHandlers
{
    public class ConfigHandler
    {
        public bool TryAddRegister(ref ClientConfig config, RegisterType type, int registerNumber, out string error)
        {
            switch (type)
            {
                case RegisterType.CoilRegister:
                    {
                        if (config.Registers.CoilRegisters.Where(x => x.RegisterAddress == registerNumber).Any())
                        {
                            error = "Taki rejestr został już dodany to konfiguracji!";
                            return false;
                        }
                        config.Registers.CoilRegisters.Add(new Register
                        {
                            CurrentValue = 0,
                            RegisterAddress = registerNumber
                        });
                        break;
                    }
                case RegisterType.DiscreteInput:
                    {
                        if (config.Registers.DiscreteInputs.Where(x => x.RegisterAddress == registerNumber).Any())
                        {
                            error = "Taki rejestr został już dodany to konfiguracji!";
                            return false;
                        }
                        config.Registers.DiscreteInputs.Add(new Register
                        {
                            CurrentValue = 0,
                            RegisterAddress = registerNumber
                        });
                        break;
                    }
                case RegisterType.InputRegister:
                    {
                        if (config.Registers.InputRegisters.Where(x => x.RegisterAddress == registerNumber).Any())
                        {
                            error = "Taki rejestr został już dodany to konfiguracji!";
                            return false;
                        }
                        config.Registers.InputRegisters.Add(new Register
                        {
                            CurrentValue = 0,
                            RegisterAddress = registerNumber
                        });
                        break;
                    }
                case RegisterType.HoldingRegister:
                    {
                        if (config.Registers.HoldingRegisters.Where(x => x.RegisterAddress == registerNumber).Any())
                        {
                            error = "Taki rejestr został już dodany to konfiguracji!";
                            return false;
                        }
                        config.Registers.HoldingRegisters.Add(new Register
                        {
                            CurrentValue = 0,
                            RegisterAddress = registerNumber
                        });
                        break;
                    }
            }
            error = "";
            return true;
        }

        public bool TryRemoveRegister(ref ClientConfig config, RegisterType type, int registerNumber)
        {
            switch (type)
            {
                case RegisterType.CoilRegister:
                    {
                        return config.Registers.CoilRegisters.Remove(
                       config.Registers.CoilRegisters
                       .Where(x => x.RegisterAddress == registerNumber).First());
                    }
                case RegisterType.DiscreteInput:
                    {
                        return config.Registers.DiscreteInputs.Remove(
                        config.Registers.DiscreteInputs
                        .Where(x => x.RegisterAddress == registerNumber).First());
                    }
                case RegisterType.InputRegister:
                    {
                        return config.Registers.InputRegisters.Remove(
                        config.Registers.InputRegisters
                        .Where(x => x.RegisterAddress == registerNumber).First());
                    }
                case RegisterType.HoldingRegister:
                    {
                        return config.Registers.HoldingRegisters.Remove(
                        config.Registers.HoldingRegisters
                        .Where(x => x.RegisterAddress == registerNumber).First());
                    }
            }
            return false;
        }

        public string ConfigToJson(ClientConfig config)
        {
            return JsonConvert.SerializeObject(config);
        }

        public ClientConfig JsonToConfig(string json)
        {
            return JsonConvert.DeserializeObject<ClientConfig>(json);
        }
    }
}
