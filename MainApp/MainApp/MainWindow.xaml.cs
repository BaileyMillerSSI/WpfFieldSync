using MainApp.IPC;
using MainApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ServerHost BroadcastServer;
        public static RemoteConnection RemoteConnection;

        PersonModel personModel = new PersonModel()
        {
            FirstName = "Bailey",
            LastName = "Miller",
            Age = 22
        };

        public MainWindow()
        {
            DataContext = personModel;
            
            InitializeComponent();
        }

        private void InitServer()
        {
            BroadcastServer = new ServerHost(11000);
        }

        private void InitConnectToServer()
        {
            var ip = IPAddress.Loopback;
            var port = 11000;

            TcpClient conn = new TcpClient();
            conn.Connect(new IPEndPoint(ip, port));

            RemoteConnection = new RemoteConnection(conn);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InitServer();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            InitConnectToServer();

            RemoteConnection.OnRemotePropChanged += UpdateLocalValue;
        }

        private void UpdateLocalValue(string PropName, object Value)
        {
            personModel.RemotePropertyChanged(PropName, Value);
        }
    }
}
