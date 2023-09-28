using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai1._4
{
    public class Thermometer
    {
        private double temperature;
        public event Action<double> TemperatureChanged;

        public double Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                OnTemperatureChanged(temperature);
            }
        }

        protected virtual void OnTemperatureChanged(double newTemperature)
        {
            if (TemperatureChanged != null)
            {
                TemperatureChanged.Invoke(newTemperature);
            }
        }

        public void SimulateTemperatureChange()
        {
            Random random = new Random();
            double newTemperature = random.NextDouble() * 100;
            newTemperature = Math.Round(newTemperature, 2);
            Temperature = newTemperature;
        }
    }
}
