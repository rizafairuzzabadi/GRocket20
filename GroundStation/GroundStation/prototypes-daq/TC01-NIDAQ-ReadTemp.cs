using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NationalInstruments;
using NationalInstruments.DAQmx;
namespace TC01_DAQ_Example
{
	public partial class Form1 : Form
	{
		 public Form1()
		{
			 InitializeComponent();
		}

			private void btnGetData_Click(object sender, EventArgs e)
			{
				Task temperatureTask = new Task();
				AIChannel myAIChannel;
				myAIChannel = temperatureTask.AIChannels.CreateThermocoupleChannel(
					"Dev1/ai0",
					"Temperature",
					0,
					100,
					AIThermocoupleType.J,
					AITemperatureUnits.DegreesC,
					25
					);
				AnalogSingleChannelReader reader = new
					AnalogSingleChannelReader(temperatureTask.Stream);
				double analogDataIn = reader.ReadSingleSample();
				txtTempData.Text = analogDataIn.ToString();
		}
	}
}