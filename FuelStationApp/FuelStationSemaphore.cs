using System;
using System.Threading;

record FuelStationSemaphore: AbstractFuelStation
{
    private SemaphoreSlim _semaphore;
    private Mutex _mutex = new Mutex();
    
    public FuelStationSemaphore(int Capacity, int ColumnCount, int reserve) : base(Capacity, ColumnCount)
    {
        Reserve = reserve;
        _semaphore = new(ColumnCount, ColumnCount);
    }

    public override bool TryRefuel(Car car, int volume)
    {
        bool result = false;
        _semaphore.Wait();
        if (volume <= Reserve)
        {
            _mutex.WaitOne();
            if (volume <= Reserve)
            {
                Console.WriteLine($"Fuel reserve is {Reserve}. Fueling began for car {car.Name}");
                Reserve -= volume;
                result = true;
            }
            _mutex.ReleaseMutex();
            Thread.Sleep(100 * volume);
            Console.WriteLine($"{car.Name} fueling finished");
        }
        _semaphore.Release();
        return result;
    }

    public override void Fill(int amount)
    {
        _mutex.WaitOne();
        if (amount + Reserve > Capacity)
            Reserve = Capacity;
        else
        {
            Reserve += amount;
        }

        Console.WriteLine("Station tank refueled");
        _mutex.ReleaseMutex();
    }
}