using System;
using System.Diagnostics;
using System.Threading;
using IFCE.AutoGate.IoT;
using IFCE.AutoGate.IoT.Models;

var rfidReader = GlobalFactory.CreateMfRc522RfidReader();
var gateMotor = GlobalFactory.CreateGateMotor();
var trafficLight = GlobalFactory.CreateTrafficLightModule();
var proximitySensor = GlobalFactory.CreateObstacleAvoidanceInfraredSensor();
var transitService = GlobalFactory.CreateTransitService();

while (true)
{
    try
    {
        bool alreadyTransited = false;
        gateMotor.Down();
        trafficLight.TurnOff();

        Thread.Sleep(TimeSpan.FromSeconds(2));
        var cardNumber = rfidReader.WaitCard();

        handleTransitRequest(cardNumber);
        Debug.WriteLine("Trânsito autorizado!");
        trafficLight.LightUpGreen();
        gateMotor.Up();

        var hasObstacle = proximitySensor.HasObstacle();
        var stopWatch = Stopwatch.StartNew();
        stopWatch.Start();
        
        while (alreadyTransited == false && hasObstacle == false && stopWatch.Elapsed < TimeSpan.FromSeconds(10))
        {
            hasObstacle = proximitySensor.HasObstacle();
            while (hasObstacle)
            {
                alreadyTransited = true;
                trafficLight.LightUpYellow();
                hasObstacle = proximitySensor.HasObstacle();
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
        }
        gateMotor.Down();
    } 
    catch (Exception e)
    {
        Debug.WriteLine(e.ToString());
        Debug.WriteLine("Trânsito não autorizado!");
        trafficLight.LightUpRed();
        Thread.Sleep(TimeSpan.FromSeconds(5));
    }
}

void handleTransitRequest(int cardNumber)
{
    var transitTypeId = new Random().Next(2) + 1;

    var transit = new Transit
    {
        CardNumber = cardNumber.ToString(),
        TransitTypeId = transitTypeId
    };

    transitService.Insert(transit);
}
