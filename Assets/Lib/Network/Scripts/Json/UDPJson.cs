using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosu.UnityLibrary
{

    public class UDPJsonSender : BaseUDPSender
    {

        public UDPJsonSender(string host, int port) : base(host, port) { }

        protected override void Constructed(string host, int port)
        {
            Debug.Log("[UDPJsonSender] Invoke Constructor Method");
            Connect(host, port);
        }

        protected override byte[] Serialize<T>(T data)
        {
            var str = JsonUtility.ToJson(data);
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(str as string);
            return byteData;
        }

    }

    public class UDPJsonReciever : BaseUDPReciever<string>
    {

        public UDPJsonReciever(int port) : base(port) { }

        protected override void Constructed(int port)
        {
            Debug.Log("[UDPJsonReciever] Invoke Constructor Method");
            StartServer(port);
        }

        protected override string DeSerialize(byte[] data)
        {
            string str = System.Text.Encoding.UTF8.GetString(data);
            return str;
        }

    }
}
