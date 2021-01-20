// Prototypes
void ds18B_setup(void);
void do_max6675_loop(void);
void do_ds18B_loop(void);

// Defines
//#define ThermoOnly 1  // For use with IDE Tools --> Serial Plotter

#include <Wire.h>

// Sample Arduino MAX6675 Arduino Sketch

#include "max6675.h"

int ktcSO = 8;
int ktcCS = 9;
int ktcCLK = 10;

MAX6675 ktc(ktcCLK, ktcCS, ktcSO);

void do_max6675_loop(void) {
  // basic readout test

#ifndef ThermoOnly
   Serial.print("Temp = ");
   Serial.print(ktc.readCelsius());
   Serial.print("\t Deg F = ");
   Serial.print(ktc.readFahrenheit());
   Serial.print(" Deg C");
#else
   Serial.println(ktc.readCelsius());
#endif
}


#include <OneWire.h>
#include <DallasTemperature.h>

// Data wire is plugged into port 2 on the Arduino
#define ONE_WIRE_BUS 4

// Setup a oneWire instance to communicate with any OneWire devices (not just Maxim/Dallas temperature ICs)
OneWire oneWire(ONE_WIRE_BUS);

// Pass our oneWire reference to Dallas Temperature.
DallasTemperature DallasSensors(&oneWire);

void setup(void)
{
  // start serial port
  Serial.begin(9600);

#ifndef ThermoOnly
  Serial.println("Dallas Temperature IC Control Library Demo");
  Serial.println("...and MAX6675 Thermocouple.");

  // Start up the library
  DallasSensors.begin();
#endif

  // give the MAX a little time to settle
  delay(500);
}

void do_ds18B_loop(void)
{
  // call DallasSensors.requestTemperatures() to issue a global temperature
  // request to all devices on the bus
  Serial.print("Requesting 1-wire devices...");
  DallasSensors.requestTemperatures(); // Send the command to get temperatures
  Serial.println("DONE");

  Serial.print("Temperature for the device 1 (index 0) is: ");
  Serial.print(DallasSensors.getTempCByIndex(0));
  Serial.println(" Deg C");
}

void loop(void) {

#ifndef ThermoOnly
   Serial.println("\n-START-");

   Serial.println("Thermocouple Temp.");
   do_max6675_loop();

   Serial.println("\nRoom Temp.");
   do_ds18B_loop();

   Serial.print("-END-");
   Serial.print("\n");
#else
  do_max6675_loop(); // Only o/p thermocouple data.
#endif

   delay(500);
}