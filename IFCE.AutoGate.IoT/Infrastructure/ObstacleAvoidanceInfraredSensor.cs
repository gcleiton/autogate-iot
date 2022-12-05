using IFCE.AutoGate.IoT.Constants;
using System.Device.Gpio;

namespace IFCE.AutoGate.IoT.Infrastructure
{
    class ObstacleAvoidanceInfraredSensor
    {
        private readonly GpioController _gpioController;
        public ObstacleAvoidanceInfraredSensor(GpioController gpioController)
        {
            _gpioController = gpioController;
            gpioController.OpenPin((int)ObstacleAvoidanceInfraredSensorPinout.OUT, PinMode.Input);
        }

        public bool HasObstacle()
        {
            var pinValue = _gpioController.Read((int)ObstacleAvoidanceInfraredSensorPinout.OUT);

            return pinValue == PinValue.Low;
        }
    }
}
