using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Dropper.Extensions
{
    public static class Extensions
    {
        public static string ReadNetworkStream(this NetworkStream ns)
        {
            try
            {
                byte[] data = new byte[1024];
                MemoryStream ms = new MemoryStream();

                int numBytesRead;
                while ((numBytesRead = ns.Read(data, 0, data.Length)) > 0)
                    ms.Write(data, 0, numBytesRead);

                return Encoding.ASCII.GetString(ms.ToArray(), 0, (int)ms.Length);
            }
            catch
            {
                return "";
            }


        }
        public static void WriteOnNetworkStream(this NetworkStream ns, string message)
        {
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(message);
            ns.Write(bytesToSend, 0, bytesToSend.Length);
        }

    }
}
