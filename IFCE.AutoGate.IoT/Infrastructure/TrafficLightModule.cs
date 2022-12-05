using IFCE.AutoGate.IoT.Constants;
using System.Device.Gpio;

namespace IFCE.AutoGate.IoT.Infrastructure
{
    internal class TrafficLightModule
    {
        private readonly GpioPin _greenPin;
        private readonly GpioPin _yellowPin;
        private readonly GpioPin _redPin;

        public TrafficLightModule(GpioController gpioController)
        {
            _greenPin = gpioController.OpenPin((int)TrafficLightModulePinout.Green, PinMode.Output);
            _yellowPin = gpioController.OpenPin((int)TrafficLightModulePinout.Yellow, PinMode.Output);
            _redPin = gpioController.OpenPin((int)TrafficLightModulePinout.Red, PinMode.Output);
        }

        public void TurnOff()
        {
            TurnOffGreen();
            TurnOffYellow();
            TurnOffRed();
        }

        public void LightUpGreen()
        {
            TurnOff();
            _greenPin.Write(PinValue.High);
        }

        public void TurnOffGreen()
        {
            _greenPin.Write(PinValue.Low);
        }

        public void LightUpYellow()
        {
            TurnOff();
            _yellowPin.Write(PinValue.High);
        }

        public void TurnOffYellow()
        {
            _yellowPin.Write(PinValue.Low);
        }

        public void LightUpRed()
        {
            TurnOff();
            _redPin.Write(PinValue.High);
        }
        public void TurnOffRed()
        {
            _redPin.Write(PinValue.Low);
        }
    }
}
