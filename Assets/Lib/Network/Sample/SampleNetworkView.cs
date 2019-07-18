using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosu.UnityLibrary
{
    public class SampleNetworkView : BaseDebugUIView
    {

        [SerializeField]
        private DebugUIButton _tcpOSCSendButtton = null;

        [SerializeField]
        private DebugUIButton _udpOSCSendButtton = null;

        [SerializeField]
        private DebugUIButton _tcpSendButtton = null;

        [SerializeField]
        private DebugUIButton _udpSendButtton = null;

        [SerializeField]
        private DebugUIButton _tcpJsonSendButtton = null;

        [SerializeField]
        private DebugUIButton _udpJsonSendButtton = null;

        protected override void Bind()
        {
            base.Bind();
            _tcpOSCSendButtton.onClick = () =>
            {
                SampleDataManager.Instance.TCPSendOSCMessage("/test", "tcp osc message");
            };
            _udpOSCSendButtton.onClick = () =>
            {
                SampleDataManager.Instance.UDPSendOSCMessage("/test", "udp osc message");
            };
            _tcpSendButtton.onClick = () =>
            {
                SampleDataManager.Instance.TCPSendMessage("tcp message");
            };
            _udpSendButtton.onClick = () =>
            {
                SampleDataManager.Instance.UDPSendMessage("udp message");
            };
            _tcpJsonSendButtton.onClick = () =>
            {
                SampleDataManager.Instance.TCPSendJsonMessage(new SampleJsonData2()
                {
                    test5 = "tcp",
                    test6 = "json message",
                    test7 = 10,
                    test8 = 1.111f
                });
            };
            _udpJsonSendButtton.onClick = () =>
            {
                SampleDataManager.Instance.UDPSendJsonMessage(new SampleJsonData()
                {
                    test1 = "udp",
                    test2 = "json message",
                    test3 = 10,
                    test4 = 1.111f
                });
            };
        }

        protected override void UnBind()
        {
            base.UnBind();
        }

    }
}
