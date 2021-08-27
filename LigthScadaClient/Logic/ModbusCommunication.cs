using System;
using System.Threading;
using System.Threading.Tasks;
using EasyModbus;

namespace LigthScadaClient.Logic
{
    public class ModbusCommunication : IDisposable
    {
        private ModbusClient m_modbusClient;

        public ModbusCommunication()
        {
            try
            {
                if (LocalConfiguration.Instance.IsTCP)
                    m_modbusClient = new ModbusClient(LocalConfiguration.Instance.IP, LocalConfiguration.Instance.TCPPort);
                else
                    m_modbusClient = new ModbusClient(LocalConfiguration.Instance.COMPort);
                m_modbusClient.ConnectedChanged += OnClientConnectionChange;
                m_modbusClient.LogFileFilename = "log.txt";
            }
            catch (Exception e)
            {
                StatusLogger.Instance.Log(e.Message);
                return;
            }
            StatusLogger.Instance.Log("Modbus Client created");
        }

        public void Start()
        {
            m_modbusClient.Connect();
            StatusLogger.Instance.Log("Modbus Client connected");
            ReadValues();
        }

        public void Stop()
        {
            m_modbusClient.Disconnect();
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
                        (x) => x.CurrentValue = m_modbusClient.ReadCoils(x.RegisterNumber, 1)[0]);
                    LocalConfiguration.Instance.DataSet.DiscreteInputs.ForEach(
                        (x) => x.CurrentValue = m_modbusClient.ReadDiscreteInputs(x.RegisterNumber, 1)[0]);
                    LocalConfiguration.Instance.DataSet.HoldingRegisters.ForEach(
                        (x) => x.CurrentValue = m_modbusClient.ReadHoldingRegisters(x.RegisterNumber, 1)[0]);
                    LocalConfiguration.Instance.DataSet.InputRegisters.ForEach(
                        (x) => x.CurrentValue = m_modbusClient.ReadInputRegisters(x.RegisterNumber, 1)[0]);
                    await ServerCommunication.Instance.SendData(LocalConfiguration.Instance.DataSet, LocalConfiguration.Instance.ApiKey);
                }
                catch (Exception e)
                {
                    StatusLogger.Instance.Log(e.Message);
                }
                Thread.Sleep(5000);
                ReadValues();
            });
        }

        public void Dispose()
        {
            m_modbusClient.ConnectedChanged -= OnClientConnectionChange;
        }

        private void OnClientConnectionChange(object sender)
        {
            StatusLogger.Instance.Log(sender.ToString() + " connection changed to connected : " + m_modbusClient.Connected);
        }
    }
}