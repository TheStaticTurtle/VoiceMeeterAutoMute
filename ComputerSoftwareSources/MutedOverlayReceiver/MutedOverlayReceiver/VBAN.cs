using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace MutedOverlayReceiver {
    class VBANText {

        Socket sock;
        IPEndPoint endPoint;

        public VBANText(IPAddress addr, int port) {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            endPoint = new IPEndPoint(addr, port);
        }

        public void send(String text) {

            char[] send_buffer = {
                'V','B','A','N',
                (char)0x40,
                (char)0x00,
                (char)0x00,
                (char)0x01,
                'C','o','m','m','a','n','d','1','\0','\0','\0','\0','\0','\0','\0','\0',
                '\0','\0','\0','\0'
            };

            Console.WriteLine((new string(send_buffer))+ text);

            sock.SendTo(
                Encoding.ASCII.GetBytes(
                    ((new string(send_buffer)) + text).ToCharArray()
                )
            , endPoint);
        }
    }
}
