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

        public void send(String streamName, String text) {
            streamName += "\0\0\0\0\0\0\0\0\0\0\0\0\0\0";
            char[] a = streamName.ToCharArray();

            char[] send_buffer = {
                'V','B','A','N',
                (char)0x40,
                (char)0x00,
                (char)0x00,
                (char)0x01,
                a[0],a[1],a[2],a[3],a[4],a[5],a[6],a[7],a[8],a[9],a[10],a[11],a[12],a[13],a[14],a[15],
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
