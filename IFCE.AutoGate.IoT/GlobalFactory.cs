using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using IFCE.AutoGate.IoT.Infrastructure;

namespace IFCE.AutoGate.IoT
{
    class GlobalFactory
    {
        private static GpioController _gpioController => new GpioController();

        public static TransitService CreateTransitService()
        {
            var apiBaseUrl = Resources.GetString(Resources.StringResources.AutoGateApiBaseURL);
            var certificateContent = Resources.GetString(Resources.StringResources.Certificate);

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl),
                HttpsAuthentCert = new X509Certificate(certificateContent),
                Timeout = TimeSpan.FromSeconds(5)
            };

            return new TransitService(httpClient);
        }

        public static MfRc522RfidReader CreateMfRc522RfidReader() => new MfRc522RfidReader(_gpioController);
        public static GateMotor CreateGateMotor() => new GateMotor();
        public static TrafficLightModule CreateTrafficLightModule() => new TrafficLightModule(_gpioController);
        public static ObstacleAvoidanceInfraredSensor CreateObstacleAvoidanceInfraredSensor() => new ObstacleAvoidanceInfraredSensor(_gpioController);
    }
}
 