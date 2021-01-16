using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
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
using CsvHelper;

namespace GroundStation
{
    public static class Globals
    {
        public static double temp = 0.00;
        public static double temp2 = 0.00;
        public static double speed = 0.00;
        public static double pressure_1 = 0.00;
        public static double pressure_2 = 0.00;
        public static double pressure_3 = 0.00;
        public static double pressure_4 = 0.00;
        public static double pressure_5 = 0.00;
        public static double pressure_6 = 0.00;
        public static double fuel = 0.00;
        public static double flow = 0.00;

        public static int counter = 0;
        public static List<string> TempArray = new List<string>();
        public static List<string> Temp2Array = new List<string>();
        public static List<string> SpeedArray = new List<string>();
        public static List<string> PreArray = new List<string>();
        public static List<string> Pre1Array = new List<string>();
        public static List<string> Pre2Array = new List<string>();
        public static List<string> Pre3Array = new List<string>();
        public static List<string> Pre4Array = new List<string>();
        public static List<string> Pre5Array = new List<string>();
        public static List<string> FuelArray = new List<string>();
        public static List<string> FlowArray = new List<string>();
    }

    /// <summary>
    /// Class to store one CSV row
    /// </summary>
    public class Project               //CsvRow : List<string>
    {
        public string Temperature { get; set; }
        public string Temperature1 { get; set; }
        public string Speed { get; set; }
        public string Pressure { get; set; }
        public string Pressure1 { get; set; }
        public string Pressure2 { get; set; }
        public string Pressure3 { get; set; }
        public string Pressure4 { get; set; }
        public string Pressure5 { get; set; }
        public string Fuel { get; set; }
        public string Flow { get; set; }

    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand;
        private static System.Timers.Timer GBTimer;
        public MainWindow()
        {
            InitializeComponent();
            /*_serialPort = new SerialPort(); //For getting temperature data from Arduino
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

        public double GetRandomNumber(double minimum, double maximum)
        {
            return rand.NextDouble() * (maximum - minimum) + minimum;
        }
        private double Mapping(double x, double in_min, double in_max, double out_min, double out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
        private void TestingRand_Click(object sender, RoutedEventArgs e)
        {
            GBTimer = new System.Timers.Timer(200);     //5Hz Timer
            GBTimer.Elapsed += OnTimedEvent;
            GBTimer.AutoReset = true;
            GBTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Globals.temp = GetRandomNumber(30, 1000);
            Globals.temp2 = GetRandomNumber(30, 1000);
            Globals.speed = GetRandomNumber(0, 1000);
            Globals.pressure_1 = GetRandomNumber(0, 700);
            Globals.pressure_2 = GetRandomNumber(0, 700);
            Globals.pressure_3 = GetRandomNumber(0, 700);
            Globals.pressure_4 = GetRandomNumber(0, 700);
            Globals.pressure_5 = GetRandomNumber(0, 700);
            Globals.pressure_6 = GetRandomNumber(0, 700);
            Globals.fuel = GetRandomNumber(0, 1000);
            Globals.flow = GetRandomNumber(0, 1000);

            Globals.fuel = Mapping(Globals.fuel, 0, 1000, 0, 100);

            string a = Globals.temp.ToString("0.00");
            string a2 = Globals.temp2.ToString("0.00");
            string b = Globals.speed.ToString("0.00");
            string c = Globals.pressure_1.ToString("0.00");
            string c1 = Globals.pressure_2.ToString("0.00");
            string c2 = Globals.pressure_3.ToString("0.00");
            string c3 = Globals.pressure_4.ToString("0.00");
            string c4 = Globals.pressure_5.ToString("0.00");
            string c5 = Globals.pressure_6.ToString("0.00");
            string d = Globals.flow.ToString("0.00");

            Globals.TempArray.Add(a);
            Globals.Temp2Array.Add(a2);
            Globals.SpeedArray.Add(b);
            Globals.PreArray.Add(c);
            Globals.Pre1Array.Add(c1);
            Globals.Pre2Array.Add(c2);
            Globals.Pre3Array.Add(c3);
            Globals.Pre4Array.Add(c4);
            Globals.Pre5Array.Add(c5);
            Globals.FuelArray.Add(d);
            Globals.FlowArray.Add(Globals.fuel.ToString("0.00"));

            Dispatcher.BeginInvoke(
                new ThreadStart(() => Temperature_Data.Text = String.Format("{0:0.##} K", a)));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => TemperatureEnt_Data.Text = String.Format("{0:0.##} K", a2)));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => SpeedData.Text = String.Format("{0:0.##} RPM", b)));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => MotorPr_Data.Text = String.Format("{0:0.##} kPA", c)));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => CompPr_Data.Text = String.Format("{0:0.##} kPA", c1)));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => CCE_Data.Text = String.Format("{0:0.##} kPA", c2)));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => TurbinePr_Data.Text = String.Format("{0:0.##} kPA", c3)));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => NozzlePr_Data.Text = String.Format("{0:0.##} kPA", c4)));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => Exit_Data.Text = String.Format("{0:0.##} kPA", c5)));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => AirFlow_Data.Text = String.Format("{0:0.##} kg/s", d)));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => FuelBar.Minimum = 0));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => FuelBar.Maximum = 100));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => FuelBar.Value = Globals.fuel));
            Globals.counter++;

        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {

            Globals.temp = 0.00;
            Globals.temp2 = 0.00;
            Globals.speed = 0.00;
            Globals.pressure_1 = 0.00;
            Globals.pressure_2 = 0.00;
            Globals.pressure_3 = 0.00;
            Globals.pressure_4 = 0.00;
            Globals.pressure_5 = 0.00;
            Globals.pressure_6 = 0.00;
            Globals.fuel = 0.00;
            Globals.flow = 0.00;

            string a = Globals.temp.ToString("0.00");
            string a2 = Globals.temp2.ToString("0.00");
            string b = Globals.speed.ToString("0.00");
            string c = Globals.pressure_1.ToString("0.00");
            string c1 = Globals.pressure_2.ToString("0.00");
            string c2 = Globals.pressure_3.ToString("0.00");
            string c3 = Globals.pressure_4.ToString("0.00");
            string c4 = Globals.pressure_5.ToString("0.00");
            string c5 = Globals.pressure_6.ToString("0.00");
            string d = Globals.flow.ToString("0.00");

            FuelBar.Minimum = 0;
            FuelBar.Maximum = 100;
            FuelBar.Value = Globals.fuel;

            Temperature_Data.Text = String.Format("{0:0.##} K", a);
            TemperatureEnt_Data.Text = String.Format("{0:0.##} K", a2);
            SpeedData.Text = String.Format("{0:0.##} RPM", b);
            MotorPr_Data.Text = String.Format("{0:0.##} kPA", c);
            CompPr_Data.Text = String.Format("{0:0.##} kPA", c1);
            CCE_Data.Text = String.Format("{0:0.##} kPA", c2);
            TurbinePr_Data.Text = String.Format("{0:0.##} kPA", c3);
            NozzlePr_Data.Text = String.Format("{0:0.##} kPA", c4);
            Exit_Data.Text = String.Format("{0:0.##} kPA", c5);
            AirFlow_Data.Text = String.Format("{0:0.##} kg/s", d);
            GBTimer.Stop();
            GBTimer.Dispose();

        }

        private void LogData_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Globals.counter; i++)
            {
                var data = new[]
                {
                    new Project { Temperature = Globals.TempArray[i], Temperature1 = Globals.Temp2Array[i], Speed = Globals.SpeedArray[i], Pressure = Globals.PreArray[i], Pressure1 = Globals.Pre1Array[i], Pressure2 = Globals.Pre2Array[i], Pressure3 = Globals.Pre3Array[i], Pressure4 = Globals.Pre4Array[i], Pressure5 = Globals.Pre5Array[i], Fuel = Globals.FuelArray[i], Flow = Globals.FlowArray[i] }
                };
                using (var mem = new MemoryStream())
                using (var writer = new StreamWriter(mem))
                using (var csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {
                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteField("Temperature");
                    csvWriter.WriteField("Speed");
                    csvWriter.WriteField("Pressure");
                    csvWriter.WriteField("Fuel");
                    csvWriter.WriteField("Flow");
                    csvWriter.NextRecord();

                    foreach (var project in data)
                    {
                        csvWriter.WriteField(project.Temperature);
                        csvWriter.WriteField(project.Temperature1);
                        csvWriter.WriteField(project.Speed);
                        csvWriter.WriteField(project.Pressure);
                        csvWriter.WriteField(project.Pressure1);
                        csvWriter.WriteField(project.Pressure2);
                        csvWriter.WriteField(project.Pressure3);
                        csvWriter.WriteField(project.Pressure4);
                        csvWriter.WriteField(project.Pressure5);
                        csvWriter.WriteField(project.Fuel);
                        csvWriter.WriteField(project.Flow);
                        csvWriter.NextRecord();
                    }

                    writer.Flush();
                    var result = Encoding.UTF8.GetString(mem.ToArray());
                    Console.WriteLine(result);
                }
            }

        }

    }
}
               
    



