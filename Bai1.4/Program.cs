using Bai1._4;
using System;
using System.Threading;

class Program
{
    static void Main()
    {
        Thermometer thermometer = new Thermometer();
        Display display = new Display();

        thermometer.TemperatureChanged += display.ShowTemperature;

        while (!Console.KeyAvailable)
        {
            thermometer.SimulateTemperatureChange();
            Console.WriteLine("You can press any key to exit.\n");
            Thread.Sleep(1000);
        }
    }
}