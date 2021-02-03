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
                textBoxReception.Text += b.ToString("X2") + " ";// + "/" + b.ToString("X2") + " ";
                DecodeMessage(b);
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
            //serialPort1.Write(byteList, 0, byteList.Length);
            UartEncodeAndSendMessage(0x0080, 7, Encoding.ASCII.GetBytes("Bonjour"));

        }

        byte CalculateChecksum(int msgFunction, int msgPayloadLength, byte[] msgPayload)
        {
            byte checksum = (byte)(0xFE ^ msgFunction);
            checksum ^= (byte)msgPayloadLength;
            for(int i = 0; i < msgPayloadLength; i++)
                checksum ^= msgPayload[i];
            
            return checksum;
        }

        void UartEncodeAndSendMessage(int msgFunction, int msgPayloadLength, byte[] msgPayload)
        {
            byte[] MessageFormatte = new byte[msgPayloadLength+6];

            MessageFormatte[0] = (byte)0xFE;
            MessageFormatte[1] = (byte)0x00;
            MessageFormatte[2] = (byte)msgFunction;
            if (msgPayloadLength <= 255)
            {
                MessageFormatte[3] = (byte)0x00;
                MessageFormatte[4] = (byte)msgPayloadLength;
            }
            else
            {
                MessageFormatte[3] = (byte)(msgPayloadLength-255);
                MessageFormatte[4] = (byte)(255);
            }

            for (int i = 0; i < msgPayloadLength; i++)
                MessageFormatte[5+i] = msgPayload[i];

            byte checksum = CalculateChecksum(msgFunction, msgPayloadLength, msgPayload);

            MessageFormatte[msgPayloadLength + 5] = checksum;

            serialPort1.Write(MessageFormatte, 0, MessageFormatte.Length);
            
        }

        public enum StateReception
        {
            Waiting,
            FunctionMSB,
            FunctionLSB,
            PayloadLengthMSB,
            PayloadLengthLSB,
            Payload,
            CheckSum
        }

        StateReception rcvState = StateReception.Waiting;
        int msgDecodedFunction = 0;
        int msgDecodedPayloadLength = 0;
        byte[] msgDecodedPayload;
        int msgDecodedPayloadIndex = 0;

        private void DecodeMessage(byte c)
        {
            switch (rcvState)
            {
                case StateReception.Waiting:
                    if (c == (byte)0xFE)
                        rcvState = StateReception.FunctionMSB;
                    break;

                case StateReception.FunctionMSB:
                    msgDecodedFunction = c;
                    rcvState = StateReception.FunctionLSB;
                    break;

                case StateReception.FunctionLSB:
                    msgDecodedFunction = msgDecodedFunction + c;
                    rcvState = StateReception.PayloadLengthMSB;
                    break;

                case StateReception.PayloadLengthMSB:
                    msgDecodedPayloadLength = c;
                    rcvState = StateReception.PayloadLengthLSB;
                    break;

                case StateReception.PayloadLengthLSB:
                    msgDecodedPayloadLength += c;
                    if (msgDecodedPayloadLength != 0)
                    {
                        rcvState = StateReception.Payload;
                        msgDecodedPayload = new byte[msgDecodedPayloadLength];
                        msgDecodedPayloadIndex = 0;
                    }
                    if (msgDecodedPayloadLength == 0)
                        rcvState = StateReception.CheckSum;
                    break;

                case StateReception.Payload:
                    if (msgDecodedPayloadIndex < msgDecodedPayloadLength)
                    {
                        msgDecodedPayload[msgDecodedPayloadIndex] = c;
                        msgDecodedPayloadIndex = msgDecodedPayloadIndex + 1;
                    }
                    if (msgDecodedPayloadIndex == msgDecodedPayloadLength)
                        rcvState = StateReception.CheckSum;
                    break;

                case StateReception.CheckSum:
                    byte calculatedChecksum = CalculateChecksum(msgDecodedFunction, msgDecodedPayloadLength, msgDecodedPayload);
                    byte receivedChecksum = c;
                    if (calculatedChecksum == receivedChecksum)
                    {
                        Console.WriteLine("OK");//Success, on a un message valide
                    }
                    else
                    {
                        Console.WriteLine("ERREUR");
                    }
                        rcvState = StateReception.Waiting;
                    break;

                default:
                    rcvState = StateReception.Waiting;
                    break;
            }
        }

    }
}
