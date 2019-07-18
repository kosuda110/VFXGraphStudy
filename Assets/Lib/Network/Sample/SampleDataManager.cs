using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosu.UnityLibrary
{
    public class SampleDataManager : BaseDataManager<SampleDataManager>
    {

        private static readonly string MY_ADDRESS = NetworkUtils.GetIP(NetworkUtils.ADDRESSFAM.IPv4);

        protected override void Setup()
        {
            _senders = new ISender[6];
            _recievers = new IReciever[6];
            // tcp osc
            _senders[0] = new TCPOSCSender(MY_ADDRESS, 20000);
            _recievers[0] = new TCPOSCReciever(MY_ADDRESS, 20000)
            {
                IsQueueing = true,
                onLatestDataRecieved = (msg) =>
                {
                    if (msg.IsBundle())
                    {
                        Debug.LogError("Not implemented");
                    }
                    else
                    {
                        Debug.Log($"UDP OSCMessage (Thread) : Address = {msg.Address} Data = {msg.Data[0].ToString()}");
                    }
                },
                onDataRecieved = (msg) =>
                {
                    if (msg.IsBundle())
                    {
                        Debug.LogError("Not implemented");
                    }
                    else
                    {
                        Debug.Log($"UDP OSCMessage : Address = {msg.Address} Data = {msg.Data[0].ToString()}");
                    }
                }
            };
            // udp osc
            _senders[1] = new UDPOSCSender(MY_ADDRESS, 20001);
            _recievers[1] = new UDPOSCReciever(20001)
            {
                IsQueueing = true,
                onLatestDataRecieved = (msg) =>
                {
                    if (msg.IsBundle())
                    {
                        Debug.LogError("Not implemented");
                    }
                    else
                    {
                        Debug.Log($"UDP OSCMessage (Thread) : Address = {msg.Address} Data = {msg.Data[0].ToString()}");
                    }
                },
                onDataRecieved = (msg) =>
                {
                    if (msg.IsBundle())
                    {
                        Debug.LogError("Not implemented");
                    }
                    else
                    {
                        Debug.Log($"UDP OSCMessage : Address = {msg.Address} Data = {msg.Data[0].ToString()}");
                    }
                }
            };
            // tcp
            _senders[2] = new BaseTCPSender(MY_ADDRESS, 20002);
            _recievers[2] = new BaseTCPReciever<string>(MY_ADDRESS, 20002)
            {
                IsQueueing = true,
                onLatestDataRecieved = (str) =>
                {
                    Debug.Log("(Thread) " + str);
                },
                onDataRecieved = (str) =>
                {
                    Debug.Log(str);
                }
            };
            // udp
            _senders[3] = new BaseUDPSender(MY_ADDRESS, 20003);
            _recievers[3] = new BaseUDPReciever<string>(20003)
            {
                IsQueueing = true,
                onLatestDataRecieved = (str) =>
                {
                    Debug.Log("(Thread) " + str);
                },
                onDataRecieved = (str) =>
                {
                    Debug.Log(str);
                }
            };
            // tcp json
            _senders[4] = new TCPJsonSender(MY_ADDRESS, 20004);
            _recievers[4] = new TCPJsonReciever(MY_ADDRESS, 20004)
            {
                IsQueueing = true,
                onLatestDataRecieved = (msg) =>
                {
                    var recievedData = JsonUtility.FromJson<BaseJsonData>(msg);

                    switch (recievedData.className)
                    {
                        case nameof(SampleJsonData):
                            var data = JsonUtility.FromJson<SampleJsonData>(msg);
                            Debug.Log($"recieved samplejsondata (thread) : test1 = {data.test1} test2 = {data.test2} test3 = {data.test3} test4 = {data.test4}");
                            break;

                        case nameof(SampleJsonData2):
                            var data2 = JsonUtility.FromJson<SampleJsonData2>(msg);
                            Debug.Log($"recieved samplejsondata2 (thread) : test5 = {data2.test5} test6 = {data2.test6} test7 = {data2.test7} test8 = {data2.test8}");
                            break;
                    }
                },
                onDataRecieved = (msg) =>
                {
                    var recievedData = JsonUtility.FromJson<BaseJsonData>(msg);

                    switch (recievedData.className)
                    {
                        case nameof(SampleJsonData):
                            var data = JsonUtility.FromJson<SampleJsonData>(msg);
                            Debug.Log($"recieved samplejsondata : test1 = {data.test1} test2 = {data.test2} test3 = {data.test3} test4 = {data.test4}");
                            break;

                        case nameof(SampleJsonData2):
                            var data2 = JsonUtility.FromJson<SampleJsonData2>(msg);
                            Debug.Log($"recieved samplejsondata2 : test5 = {data2.test5} test6 = {data2.test6} test7 = {data2.test7} test8 = {data2.test8}");
                            break;
                    }
                }
            };
            // udp json
            _senders[5] = new UDPJsonSender(MY_ADDRESS, 20005);
            _recievers[5] = new UDPJsonReciever(20005)
            {
                IsQueueing = true,
                onLatestDataRecieved = (msg) =>
                {
                    var recievedData = JsonUtility.FromJson<BaseJsonData>(msg);

                    switch (recievedData.className)
                    {
                        case nameof(SampleJsonData):
                            var data = JsonUtility.FromJson<SampleJsonData>(msg);
                            Debug.Log($"recieved samplejsondata (thread) : test1 = {data.test1} test2 = {data.test2} test3 = {data.test3} test4 = {data.test4}");
                            break;

                        case nameof(SampleJsonData2):
                            var data2 = JsonUtility.FromJson<SampleJsonData2>(msg);
                            Debug.Log($"recieved samplejsondata2 (thread) : test5 = {data2.test5} test6 = {data2.test6} test7 = {data2.test7} test8 = {data2.test8}");
                            break;
                    }
                },
                onDataRecieved = (msg) =>
                {
                    var recievedData = JsonUtility.FromJson<BaseJsonData>(msg);

                    switch (recievedData.className)
                    {
                        case nameof(SampleJsonData):
                            var data = JsonUtility.FromJson<SampleJsonData>(msg);
                            Debug.Log($"recieved samplejsondata : test1 = {data.test1} test2 = {data.test2} test3 = {data.test3} test4 = {data.test4}");
                            break;

                        case nameof(SampleJsonData2):
                            var data2 = JsonUtility.FromJson<SampleJsonData2>(msg);
                            Debug.Log($"recieved samplejsondata2 : test5 = {data2.test5} test6 = {data2.test6} test7 = {data2.test7} test8 = {data2.test8}");
                            break;
                    }
                }
            };
        }

        protected override void BeforeClose()
        {
            TCPSendOSCMessage("/test", "before close tcp osc message");
            UDPSendOSCMessage("/test", "before close udp osc message");
            TCPSendMessage("before close tcp message");
            UDPSendMessage("before close udp message");
            TCPSendJsonMessage(new SampleJsonData2()
            {
                test5 = "tcp",
                test6 = "before close json message",
                test7 = 10,
                test8 = 1.111f
            });
            UDPSendJsonMessage(new SampleJsonData()
            {
                test1 = "udp",
                test2 = "before close json message",
                test3 = 10,
                test4 = 1.111f
            });
        }

        public void TCPSendOSCMessage(string address, params object[] args)
        {
            _senders[0]?.Send(NetworkUtils.CreateOSCMessage(address, args));
        }

        public void UDPSendOSCMessage(string address, params object[] args)
        {
            _senders[1]?.Send(NetworkUtils.CreateOSCMessage(address, args));
        }

        public void TCPSendMessage(string msg)
        {
            _senders[2]?.Send(msg);
        }

        public void UDPSendMessage(string msg)
        {
            _senders[3]?.Send(msg);
        }

        public void TCPSendJsonMessage(BaseJsonData data)
        {
            _senders[4]?.Send(data);
        }

        public void UDPSendJsonMessage(BaseJsonData data)
        {
            _senders[5]?.Send(data);
        }

    }
}
