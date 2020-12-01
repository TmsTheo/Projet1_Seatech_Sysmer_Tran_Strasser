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

namespace Projet_Projet_2_Tran_Strasser
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>

   
    public partial class MainWindow : Window
    {
        ReliableSerialPort reliableSerialPort1;
        public MainWindow()
        {
            InitializeComponent();
            reliableSerialPort1 = new ReliableSerialPort("COM9", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            reliableSerialPort1.DataReceived += ReliableSerialPort1_DataReceived;
            reliableSerialPort1.Open();

        }

        public void ReliableSerialPort1_DataReceived(object sender, DataReceivedArgs e)
        {
            textBoxReception.Text += Encoding.UTF8.GetString(e.Data, 0, e.Data.Length);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (buttonEnvoyer.Background == Brushes.RoyalBlue)
                buttonEnvoyer.Background = Brushes.Beige;
            else
                buttonEnvoyer.Background = Brushes.RoyalBlue;
            textBoxReception.Text = textBoxReception.Text + "Reçu : " + textBoxEmission.Text +"\n";
            textBoxEmission.Text = "";
        }

        private void textBoxEmission_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBoxReception.Text = textBoxReception.Text + "Reçu : " + textBoxEmission.Text;
                reliableSerialPort1.WriteLine(textBoxEmission.Text);
                textBoxEmission.Text = "";
            }
        }
    }
}
