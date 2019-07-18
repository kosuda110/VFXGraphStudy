using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UniRx;

namespace Kosu.UnityLibrary
{
    public class BaseTCPSender : ISender
    {

        private TcpClient _tcpClient;

        private bool _isConnected;

        private System.IDisposable _connectStream;

        protected string _host;

        protected int _port;

        public BaseTCPSender(string host, int port)
        {
            Constructed(host, port);
        }

        ~BaseTCPSender()
        {
            Destructed();
        }

        protected virtual void Constructed(string host, int port)
        {
            Debug.Log("[BaseTCPSender] Invoke Contructor Method");
            _host = host;
            _port = port;
            Connect();
        }

        protected virtual void Destructed()
        {
            Debug.Log("[BaseTCPSender] Invoke Destructor Method");
            Close();
        }

        protected void Connect()
        {
            _connectStream = UniRxUtils.StartCoroutine(ConnectEnumerator, () =>
            {
                Debug.Log($"Success tcp connect : IpAddress = {_host} Port = {_port}");
                _connectStream = null;
            });
        }

        private IEnumerator ConnectEnumerator()
        {
            while (!_isConnected)
            {
                try
                {
                    _tcpClient = new TcpClient(_host, _port);
                    _isConnected = true;
                }
                catch (SocketException e)
                {
                    Debug.LogError(e.Message);
                }

                if (!_isConnected)
                {
                    yield return new WaitForSeconds(1);
                }
            }
        }

        public void Close()
        {
            _connectStream?.Dispose();
            _connectStream = null;
            _tcpClient?.Close();
            _tcpClient = null;
        }

        public void Send<T>(T data) where T : class
        {
            if (_tcpClient == null ||
                !_tcpClient.Connected)
            {
                Debug.LogError($"Not Conencted : IPAddress = {_host} Port = {_port}");
                return;
            }

            try
            {
                using (NetworkStream stream = _tcpClient.GetStream())
                {
                    if (stream.CanWrite)
                    {
                        var bytes = Serialize(data);
                        stream.Write(bytes, 0, bytes.Length);
                        Debug.Log("send to tcp server : length = " + bytes.Length + "(byte)");
                    }
                }
            }
            catch (SocketException e)
            {
                Debug.LogError("SocketException : " + e.ToString());
            }

            _isConnected = _tcpClient.Connected;
            Connect();
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

    public class BaseTCPReciever<T> : IReciever where T : class
    {

        public System.Action<T> onLatestDataRecieved;

        public System.Action<T> onDataRecieved;

        public bool IsQueueing { get; set; } = false;

        private TcpListener _tcpListener;

        private Thread _th;

        private Queue<T> _packetQueue = new Queue<T>();

        protected string _host;

        protected int _port;

        private bool _isServerStarted;

        private System.IDisposable _startServerStream;

        private System.IDisposable _updateListenPacketStream;

        public BaseTCPReciever(string host, int port)
        {
            Constructed(host, port);
        }

        ~BaseTCPReciever()
        {
            Destructed();
        }

        protected virtual void Constructed(string host, int port)
        {
            Debug.Log("[BaseTCPReciever] Invoke Constructor Method");
            _host = host;
            _port = port;
            StartServer();
        }

        protected virtual void Destructed()
        {
            Debug.Log("[BaseTCPReciever] Invoke Destructor Method");
            Close();
        }

        protected void StartServer()
        {
            _startServerStream = UniRxUtils.StartCoroutine(StartServerEnumerator, () =>
            {
                Debug.Log($"Success start tcp server : IpAddress = {_host} Port = {_port}");
                _startServerStream = null;
                _updateListenPacketStream = Observable.EveryUpdate().Subscribe(_ => UpdateListenPacket());
            });
        }

        private IEnumerator StartServerEnumerator()
        {
            while (!_isServerStarted)
            {
                try
                {
                    _tcpListener = new TcpListener(IPAddress.Parse(_host), _port);
                    _tcpListener.Start();
                    _isServerStarted = true;
                }
                catch (SocketException e)
                {
                    Debug.LogError(e.Message);
                }

                yield return new WaitForSeconds(1);
            }

            _th = new Thread(new ThreadStart(Recieve));
            _th.IsBackground = true;
            _th.Start();
        }

        public void Close()
        {
            _startServerStream?.Dispose();
            _startServerStream = null;
            _updateListenPacketStream?.Dispose();
            _updateListenPacketStream = null;
            _th?.Abort();
            _th = null;
            _tcpListener?.Stop();
            _tcpListener = null;
        }

        private void Recieve()
        {
            byte[] bytes = new byte[1024];

            while (true)
            {
                using (var tcpClient = _tcpListener.AcceptTcpClient())
                {
                    using (NetworkStream stream = tcpClient.GetStream())
                    {
                        using (System.IO.MemoryStream mem = new System.IO.MemoryStream())
                        {
                            int length = 0;

                            while ((length = stream.Read(bytes, 0, bytes.Length)) > 0)
                            {
                                mem.Write(bytes, 0, length);
                            }

                            if (mem != null && mem.ToArray().Length > 0)
                            {
                                var recievedData = DeSerialize(mem.ToArray());
                                OnLatestDataRecieved(recievedData);

                                if (IsQueueing)
                                {
                                    _packetQueue.Enqueue(recievedData);
                                }
                            }
                        }
                    }
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
