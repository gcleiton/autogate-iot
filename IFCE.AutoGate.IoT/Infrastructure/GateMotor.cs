using nanoFramework.Hardware.Esp32;
using System.Device.Pwm;
using Iot.Device.ServoMotor;
using IFCE.AutoGate.IoT.Constants;
using System.Threading;
using System;

namespace IFCE.AutoGate.IoT.Infrastructure
{
    class GateMotor
    {
        private const int UpAngle = 90;
        private const int DownAngle = 0;

        private readonly ServoMotor _motor;

        public GateMotor()
        {
            var pwmPin = (int)GateMotorPinout.PWM;

            Configuration.SetPinFunction(pwmPin, DeviceFunction.PWM1);

            var pwmChannel = PwmChannel.CreateFromPin(pwmPin, 50);
            _motor = new ServoMotor(pwmChannel, 90);
            _motor.Start();
        }

        public void Up()
        {
            _motor.WriteAngle(UpAngle);
        }

        public void Down()
        {
            _motor.WriteAngle(DownAngle);
        }
    }
}
