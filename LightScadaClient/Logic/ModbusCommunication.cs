using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using EasyModbus;

namespace LigthScadaClient.Logic
{
    public class ModbusCommunication
    {
        public event Action OnError;

        private ModbusClient m_modbusClient;
        private CancellationTokenSource m_source = new CancellationTokenSource();

        public bool Start()
        {
            try
            {
                CreateClient();
                m_modbusClient.Connect();
                StatusLogger.Instance.Log("Modbus Client connected");
                ReadValues(m_source.Token);
                return true;
            }
            catch (Exception e)
            {
                StatusLogger.Instance.Log(e.Message);
                return false;
            }
        }

        public void Stop(Action OnStop)
        {
            Task.Run(() =>
            {
                m_source.Cancel();
                m_modbusClient.Disconnect();
                StatusLogger.Instance.Log("Modbus Client disconnected");
                OnStop?.Invoke();
            });
        }

        private void ReadValues(CancellationToken token)
        {
            Task.Run(async () =>
            {
                if (m_modbusClient != null && !m_modbusClient.Connected)
                    return;
                try
                {
                    LocalConfiguration.Instance.DataSet.CoilRegisters.ForEach(
                        (x) => x.CurrentValue = m_modbusClient.ReadCoils(x.RegisterAddress, 1)[0] ? 1 : 0);
                    LocalConfiguration.Instance.DataSet.DiscreteInputs.ForEach(
                        (x) => x.CurrentValue = m_modbusClient.ReadDiscreteInputs(x.RegisterAddress, 1)[0] ? 1 : 0);
                    LocalConfiguration.Instance.DataSet.HoldingRegisters.ForEach(
                        (x) => x.CurrentValue = m_modbusClient.ReadHoldingRegisters(x.RegisterAddress, 1)[0]);
                    LocalConfiguration.Instance.DataSet.InputRegisters.ForEach(
                        (x) => x.CurrentValue = m_modbusClient.ReadInputRegisters(x.RegisterAddress, 1)[0]);
                    await ServerCommunication.Instance.SendData(LocalConfiguration.Instance.DataSet, LocalConfiguration.Instance.ApiKey);
                }
                catch (Exception e)
                {
                    OnError?.Invoke();
                    StatusLogger.Instance.Log(e.Message);
                }
                token.WaitHandle.WaitOne(LocalConfiguration.Instance.Interval * 1000);
                if (!token.IsCancellationRequested)
                    ReadValues(token);
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