using ExtendedSerialPort;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
using System.Windows.Threading;

namespace Projet_Projet_2_Tran_Strasser
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        ReliableSerialPort serialPort1;
        public MainWindow()
        {
            InitializeComponent();
            serialPort1 = new ReliableSerialPort("COM9", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            serialPort1.DataReceived += SerialPort1_DataReceived;
            serialPort1.Open();

            //Config timer
            timerAffichage = new DispatcherTimer();
            timerAffichage.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timerAffichage.Tick += TimerAffichage_Tick;
            timerAffichage.Start();
        }

        string receivedText;
        Queue<byte> byteListReceived = new Queue<byte>();
        DispatcherTimer timerAffichage;


        public void SerialPort1_DataReceived(object sender, DataReceivedArgs e)
        {
            //textBoxReception.Text += Encoding.UTF8.GetString(e.Data, 0, e.Data.Length);
            //receivedText += Encoding.UTF8.GetString(e.Data, 0, e.Data.Length); 
            for (int i = 0; i < e.Data.Length; i++)
            {
                byteListReceived.Enqueue(e.Data[i]);
            }
        }

        private void TimerAffichage_Tick(object sender, EventArgs e)
        {
            while (byteListReceived.Count > 0)
            {
                byte b = byteListReceived.Dequeue();
                textBoxReception.Text += b.ToString() + "/" + b.ToString("X2") + " ";
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            sendMessage();
        }

        private void textBoxEmission_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                sendMessage();
            }
        }

        private void sendMessage()
        {
            serialPort1.WriteLine(textBoxEmission.Text);
            textBoxEmission.Text = "";
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            textBoxReception.Text = "";
            textBoxEmission.Text = "";
        }

        byte[] byteList = new byte[20];
        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i < 20; i++)
            {
                byteList[i] = (byte)(2 * i);
            }
            serialPort1.Write(byteList, 0, byteList.Length);
        }
    }
}
