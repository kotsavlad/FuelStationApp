using System;
using System.Threading;

record FuelStationSemaphore : AbstractFuelStation
{
    private readonly SemaphoreSlim _semaphore;
    private readonly Mutex _mutex = new Mutex();

    public FuelStationSemaphore(int capacity, int columnCount, int reserve) : base(capacity, columnCount)
    {
        Reserve = reserve;
        _semaphore = new(columnCount, columnCount);
    }

    // public override bool TryRefuel(Car car, int volume)
    // {
    //     var result = false;
    //     _semaphore.Wait();
    //     if (volume <= Reserve)
    //     {
    //         _mutex.WaitOne();
    //         if (volume <= Reserve)
    //         {
    //             Console.WriteLine($"Fuel reserve is {Reserve}. Fueling began for car {car.Name}");
    //             Reserve -= volume;
    //             result = true;
    //         }
    //
    //         _mutex.ReleaseMutex();
    //         Thread.Sleep(100 * volume);
    //         Console.WriteLine($"{car.Name} fueling finished");
    //     }
    //
    //     _semaphore.Release();
    //     return result;
    // }

    // public override void Fill(int amount)
    // {
    //     _mutex.WaitOne();
    //     if (amount + Reserve > Capacity)
    //         Reserve = Capacity;
    //     else
    //         Reserve += Capacity;
    //     Console.WriteLine("STATION TANK REFUELED");
    //     _mutex.ReleaseMutex();
    // }

    public override void ReserveEnterCS() => _mutex.WaitOne();

    public override void ReserveExitCS() => _mutex.ReleaseMutex();

    public override void AcquireColumn() => _semaphore.Wait();

    public override void ReleaseColumn() => _semaphore.Release();
}