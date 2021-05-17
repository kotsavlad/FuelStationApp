using System;
using System.Threading;

public record FuelStationMonitor : AbstractFuelStation
{
    private int _freeColumnCount;
    private readonly object _lockObject;


    public FuelStationMonitor(int capacity, int columnCount, int reserve) : base(capacity, columnCount)
    {
        Reserve = reserve;
        _freeColumnCount = columnCount;
        _lockObject = new object();
    }

    // public override bool TryRefuel(Car car, int volume)
    // {
    //     var isHandled = false;
    //     lock (_lockObject)
    //     {
    //         while (_freeColumnCount <= 0)
    //         {
    //             Monitor.Wait(_lockObject);
    //         }
    //
    //         if (volume <= Reserve)
    //         {
    //             _freeColumnCount--;
    //             Console.WriteLine($"Fuel reserve is {Reserve}. {volume} liters fueling began for car {car.Name}");
    //             Reserve -= volume;
    //             isHandled = true;
    //         }
    //     }
    //
    //     if (!isHandled) return false;
    //     Thread.Sleep(100 * volume);
    //     Console.WriteLine($"{volume} liters fueling finished for {car.Name}");
    //     lock (_lockObject)
    //     {
    //         _freeColumnCount++;
    //         Monitor.Pulse(_lockObject);
    //     }
    //
    //     return true;
    // }

    // public override void Fill(int amount)
    // {
    //     if (amount <= 0)  return;
    //     lock (_lockObject)
    //     {
    //         Console.WriteLine("\nStation tank refueling began");
    //         Reserve = Math.Min(Capacity, Reserve + amount);
    //         Thread.Sleep(20 * amount);
    //         Console.WriteLine($"Station tank refueled. Current reserve is {Reserve}.\n");
    //         Monitor.PulseAll(_lockObject);
    //     }
    // }

    public override void ReserveEnterCS() => Monitor.Enter(_lockObject);

    public override void ReserveExitCS() => Monitor.Exit(_lockObject);

    public override void AcquireColumn()
    {
        lock (_lockObject)
        {
            while (_freeColumnCount <= 0)
            {
                Monitor.Wait(_lockObject);
            }

            _freeColumnCount--;
        }
    }

    public override void ReleaseColumn()
    {
        lock (_lockObject)
        {
            _freeColumnCount++;
            Monitor.Pulse(_lockObject);
        }
    }
}