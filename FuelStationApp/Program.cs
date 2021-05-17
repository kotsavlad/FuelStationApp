using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// const int carCount = 2;
const int carCount = 5;
Console.WriteLine("GO");
var fuelStation = new FuelStationSemaphore(1000, 2, 200);
// var fuelStation = new FuelStationMonitor(1000, 1, 40);
var tasks = new Task[carCount];
for (int i = 0; i < carCount; i++)
{
    var car = new Car($"car {i + 1}", fuelStation, 100, 4);
    (tasks[i] = new Task(car.Run)).Start();
}

// var car1 = new Car("car 1", fuelStation, 100, 1, 50);
// (tasks[0] = new Task(car1.Run)).Start();
// Thread.Sleep(10);
// var car2 = new Car("car 2", fuelStation, 100, 1, 30);
// (tasks[1] = new Task(car2.Run)).Start();

Thread.Sleep(15000);
fuelStation.Fill(2000);
// Thread.Sleep(7000);
// fuelStation.Fill(100);
Task.WaitAll(tasks);
Console.WriteLine("FINISH");