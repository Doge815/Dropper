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
            byte[] data = new byte[1024];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                do
                {
                    var readCount = ns.Read(data, 0, data.Length);
                    memoryStream.Write(data, 0, readCount);
                } while (ns.DataAvailable);

                return Encoding.ASCII.GetString(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            }
        }
        public static void WriteOnNetworkStream(this NetworkStream ns, string message)
        {
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(message);
            ns.Write(bytesToSend, 0, bytesToSend.Length);
        }
    
    }
}
