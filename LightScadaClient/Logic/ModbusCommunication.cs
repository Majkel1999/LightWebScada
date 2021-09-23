using System;
using System.Threading;
using System.Threading.Tasks;
using EasyModbus;

namespace LigthScadaClient.Logic
{
    public class ModbusCommunication
    {
        public event Action OnError;

        private ModbusClient m_modbusClient;

        public bool Start()
        {
            try
            {
                CreateClient();
                m_modbusClient.Connect();
                StatusLogger.Instance.Log("Modbus Client connected");
                ReadValues();
                return true;
            }
            catch (Exception e)
            {
                StatusLogger.Instance.Log(e.Message);
                return false;
            }
        }

        public void Stop()
        {
            m_modbusClient.Disconnect();
            StatusLogger.Instance.Log("Modbus Client disconnected");
        }

        private void ReadValues()
        {
            Task.Run(async () =>
            {
                if (m_modbusClient != null && !m_modbusClient.Connected)
                    return;
                try
                {
                    LocalConfiguration.Instance.DataSet.CoilRegisters.ForEach(
                        (x) => x.CurrentValue = m_modbusClient.ReadCoils(x.RegisterAddress, 1)[0]);
                    LocalConfiguration.Instance.DataSet.DiscreteInputs.ForEach(
                        (x) => x.CurrentValue = m_modbusClient.ReadDiscreteInputs(x.RegisterAddress, 1)[0]);
                    LocalConfiguration.Instance.DataSet.HoldingRegisters.ForEach(
                        (x) => x.CurrentValue = m_modbusClient.ReadHoldingRegisters(x.RegisterAddress, 1)[0]);
                    LocalConfiguration.Instance.DataSet.InputRegisters.ForEach(
                        (x) => x.CurrentValue = m_modbusClient.ReadInputRegisters(x.RegisterAddress, 1)[0]);
                    await ServerCommunication.Instance.SendData(LocalConfiguration.Instance.DataSet, LocalConfiguration.Instance.ApiKey);
                    StatusLogger.Instance.Log("Data sent to server");
                }
                catch (Exception e)
                {
                    OnError?.Invoke();
                    StatusLogger.Instance.Log(e.Message);
                }
                Thread.Sleep(5000);
                ReadValues();
            });
        }

        private void CreateClient()
        {
            if (LocalConfiguration.Instance.IsTCP)
                m_modbusClient = new ModbusClient(LocalConfiguration.Instance.IP, LocalConfiguration.Instance.TCPPort);
            else
            {
                m_modbusClient = new ModbusClient(LocalConfiguration.Instance.COMPort);
                m_modbusClient.Baudrate = LocalConfiguration.Instance.Baudrate;
                m_modbusClient.Parity = LocalConfiguration.Instance.Parity;
                m_modbusClient.StopBits = LocalConfiguration.Instance.StopBits;
                m_modbusClient.UnitIdentifier = (byte)LocalConfiguration.Instance.SlaveID;
            }
            m_modbusClient.ConnectionTimeout = 500;
            StatusLogger.Instance.Log("Modbus Client created");
        }
    }
}