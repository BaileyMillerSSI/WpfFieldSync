using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainApp.IPC
{
    /// <summary>
    /// Hold all the logic for broadcasting change events!
    /// </summary>
    public class ServerHost
    {
        public int Port { get; private set; }

        private readonly TcpListener RootServer;
        private ConcurrentBag<RemoteConnection> ActiveConnections = new ConcurrentBag<RemoteConnection>();
        
        private CancellationTokenSource KillListenerSource = new CancellationTokenSource();
        private CancellationToken KillListenerToken { get => KillListenerSource.Token; }
        private Task ListenerTask;

        public ServerHost(int Port)
        {
            this.Port = Port;

            RootServer = new TcpListener(IPAddress.Loopback, Port);
            RootServer.Start();
            
            ListenerTask = Task.Run(new Action(ListenerFn));
        }

        void ListenerFn()
        {
            while (!KillListenerSource.IsCancellationRequested)
            {
                // Accept connection
                var conn = RootServer.AcceptTcpClient();

                ActiveConnections.Add(new RemoteConnection(conn));
            }
        }

        public void BroadcastChange(String PropName, Object Value)
        {
            foreach (var conn in ActiveConnections)
            {
                conn.EmitChange(PropName, Value);
            }
        }

    }
}
