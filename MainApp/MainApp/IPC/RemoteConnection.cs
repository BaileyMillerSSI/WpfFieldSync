using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.IPC
{
    public class RemoteConnection
    {
        public delegate void RemotePropertyHandler(String PropName, Object Value);
        public event RemotePropertyHandler OnRemotePropChanged;
        private TcpClient _Client;
        private NetworkStream NStream;
        private BinaryReader Reader;
        private BinaryWriter Writer;

        private Task ReaderThread;

        public RemoteConnection(TcpClient Client)
        {
            _Client = Client;
            NStream = _Client.GetStream();
            Writer = new BinaryWriter(NStream, Encoding.UTF8, true);
            ReaderThread = Task.Run(new Action(ReaderFn));
        }

        void ReaderFn()
        {
            while (true)
            {
                using (Reader = new BinaryReader(NStream, Encoding.UTF8, true))
                {
                    while (_Client.Connected)
                    {
                        var rawData = Reader.ReadString();
                        Debug.WriteLine(rawData);
                        var firstSemi = rawData.IndexOf(";");

                        var propName = rawData.Substring(0, firstSemi);
                        var value = rawData.Substring(firstSemi + 1, rawData.Length - propName.Length - 1);

                        OnRemotePropChanged?.Invoke(propName, value);
                    }
                }
            }
        }

        public void EmitChange(String PropName, Object Value)
        {
            Writer.Write($"{PropName};{Value}");
        }

    }
}
