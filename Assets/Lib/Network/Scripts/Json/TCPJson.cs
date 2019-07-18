using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosu.UnityLibrary
{
    public class TCPJsonSender : BaseTCPSender
    {

        public TCPJsonSender(string host, int port) : base(host, port) { }

        protected override void Constructed(string host, int port)
        {
            Debug.Log("[TCPOSCSender] Invoke Contructor Method");
            _host = host;
            _port = port;
            Connect();
        }

        protected override byte[] Serialize<T>(T data)
        {
            var str = JsonUtility.ToJson(data);
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(str as string);
            return byteData;
        }

    }

    public class TCPJsonReciever : BaseTCPReciever<string>
    {

        public TCPJsonReciever(string host, int port) : base(host, port) { }

        protected override void Constructed(string host, int port)
        {
            Debug.Log("[TCPOSCReciever] Invoke Constructor Method");
            _host = host;
            _port = port;
            StartServer();
        }

        protected override string DeSerialize(byte[] data)
        {
            string str = System.Text.Encoding.UTF8.GetString(data);
            return str;
        }

    }
}
