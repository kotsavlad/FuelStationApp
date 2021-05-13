using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

const int CarCount = 5;
Console.WriteLine("GO");
// var fuelStation = new FuelStationSemaphore(1000, 2, 200);
var fuelStation = new FuelStationMonitor(1000, 2, 200);
var tasks = new Task[CarCount];
for (int i = 0; i < CarCount; i++)
{
    var car = new Car("car " + (i + 1), fuelStation, 100, 4);
    tasks[i] = new Task(car.Run);
    tasks[i].Start();
}

Thread.Sleep(20000);
fuelStation.Fill(2000);
Task.WaitAll(tasks);
Console.WriteLine("FINISH");