using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UniRx;

namespace Kosu.UnityLibrary
{

    public class BaseUDPSender : ISender
    {

        private UdpClient _udp;

        private string _host;

        private int _port;

        public BaseUDPSender(string host, int port)
        {
            Constructed(host, port);
        }

        ~BaseUDPSender()
        {
            Destructed();
        }

        protected virtual void Constructed(string host, int port)
        {
            Debug.Log("[BaseUDPSender] Invoke Constructor Method");
            Connect(host, port);
        }

        protected virtual void Destructed()
        {
            Debug.Log("[BaseUDPSender] Invoke Destructor Method");
            Close();
        }

        protected void Connect(string host, int port)
        {
            _udp = new UdpClient();
            _host = host;
            _port = port;
        }

        public void Close()
        {
            _udp?.Close();
            _udp = null;
        }

        public void Send<T>(T data) where T : class
        {
            if (data == null)
            {
                return;
            }

            var sendData = Serialize(data);

            if (sendData != null && sendData.Length > 0)
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(_host), _port);
                _udp.Send(sendData, sendData.Length, remoteEP);
            }
        }

        protected virtual byte[] Serialize<T>(T data) where T : class
        {
            if (!(data is string))
            {
                Debug.LogError($"data format is invalid : datatype = {typeof(T)}");
                return null;
            }

            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(data as string);
            return byteData;
        }

    }

    public class BaseUDPReciever<T> : IReciever where T : class
    {

        public System.Action<T> onLatestDataRecieved;

        public System.Action<T> onDataRecieved;

        public bool IsQueueing { get; set; } = false;

        private UdpClient _udp;

        private Thread _th;

        private System.IDisposable _updateListenPacketStream;

        private Queue<T> _packetQueue = new Queue<T>();

        public BaseUDPReciever(int port)
        {
            Constructed(port);
        }

        ~BaseUDPReciever()
        {
            Destructed();
        }

        protected virtual void Constructed(int port)
        {
            Debug.Log("[BaseUDPReciever] Invoke Constructor Method");
            StartServer(port);
        }

        protected virtual void Destructed()
        {
            Debug.Log("[BaseUDPReciever] Invoke Destructor Method");
            Close();
        }

        protected void StartServer(int port)
        {
            _udp = new UdpClient(port);
            _th = new Thread(new ThreadStart(Recieve))
            {
                IsBackground = true
            };
            _th.Start();
            _updateListenPacketStream = Observable.EveryUpdate().Subscribe(_ => UpdateListenPacket());
        }

        public void Close()
        {
            _updateListenPacketStream?.Dispose();
            _updateListenPacketStream = null;
            _th?.Abort();
            _th = null;
            _udp?.Close();
            _udp = null;
        }

        protected void Recieve()
        {
            while (true)
            {
                try
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = _udp.Receive(ref remoteEP);

                    if (data != null && data.Length > 0)
                    {
                        var recievedData = DeSerialize(data);
                        OnLatestDataRecieved(recievedData);

                        if (IsQueueing)
                        {
                            _packetQueue.Enqueue(recievedData);
                        }
                    }
                }
                catch (SocketException e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }

        protected virtual void OnLatestDataRecieved(T recievedData)
        {
            onLatestDataRecieved.SafeInvoke(recievedData);
        }

        protected void UpdateListenPacket()
        {
            if (!IsQueueing)
            {
                return;
            }

            while (_packetQueue.Count > 0)
            {
                T packet = _packetQueue.Dequeue();

                if (packet == null)
                {
                    return;
                }

                OnDataRecieved(packet);
            }
        }

        protected virtual void OnDataRecieved(T packet)
        {
            onDataRecieved.SafeInvoke(packet);
        }

        protected virtual T DeSerialize(byte[] data)
        {
            var text = System.Text.Encoding.UTF8.GetString(data) as T;
            return text;
        }

    }
}
