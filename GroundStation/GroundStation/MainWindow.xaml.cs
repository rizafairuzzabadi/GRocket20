using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
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

namespace GroundStation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand;
        public MainWindow()
        {
            InitializeComponent();
            /*_serialPort = new SerialPort();
            _serialPort.PortName = "COM4";//Set your board COM
            _serialPort.BaudRate = 9600;
            _serialPort.Open();
            while (true)
            {
                string a = _serialPort.ReadExisting();
                Console.WriteLine(a);
                Thread.Sleep(200);
            }*/
            rand = new Random();
        }
        /*private byte[] GetRandomBytes(int n)
        {
            //  Fill an array of bytes of length "n" with random numbers.
            var randomBytes = new byte[n];
            rand.NextBytes(randomBytes);
            return randomBytes;
        }*/

        public double GetRandomNumber(double minimum, double maximum)
        {
            return rand.NextDouble() * (maximum - minimum) + minimum;
        }
        private void TestingRand_Click(object sender, RoutedEventArgs e)
        {
            
            double temp = GetRandomNumber(-1000,1000);
            double speed = GetRandomNumber(0, 1000);
            double pressure_2 = GetRandomNumber(0, 1000);
            double fuel = GetRandomNumber(0, 100);
            double flow = GetRandomNumber(0, 1000);

            string a = temp.ToString("0.00");
            string b = speed.ToString("0.00");
            string c = pressure_2.ToString("0.00");
            string d = flow.ToString("0.00");

            string resulta = String.Format("{0:0.##} C", a);
            string resultb = String.Format("{0:0.##} RPM",b);
            string resultc = String.Format("{0:0.##} kPA",c);
            string resultd = String.Format("{0:0.##} CFM", d);


            FuelBar.Minimum = 0;
            FuelBar.Maximum = 100;
            FuelBar.Value = fuel;

            Temperature_Data.Text = resulta;
            Pressure_Data.Text = resultc;
            SpeedData.Text = resultb;
            AirFlow_Data.Text = resultd;


        }
    }
}
