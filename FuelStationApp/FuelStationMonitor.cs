using System;
using System.Threading;

public record FuelStationMonitor: AbstractFuelStation
{
    private int _freeColumnCount;
    private object lockObject;


    public FuelStationMonitor(int Capacity, int ColumnCount, int reserve) : base(Capacity, ColumnCount)
    {
        Reserve = reserve;
        _freeColumnCount = ColumnCount;
        lockObject = new object();
    }

    public override bool TryRefuel(Car car, int volume)
    {
        var result = false;
        lock (lockObject)
        {
            while (_freeColumnCount <= 0)
            {
                Monitor.Wait(lockObject);
            }

            if (volume <= Reserve)
            {
                _freeColumnCount--;
                Console.WriteLine($"Fuel reserve is {Reserve}. Fueling began for car {car.Name}");
                Reserve -= volume;
                result = true;
            }
        }

        if (result)
        {
            Thread.Sleep(100 * volume);
            Console.WriteLine($"{car.Name} fueling finished");
            lock (lockObject)
            {
                _freeColumnCount++;
                Monitor.Pulse(lockObject);
            }
        }
        return result;
    }

    public override void Fill(int amount)
    {
        lock (lockObject)
        {
            if (amount + Reserve > Capacity)
                Reserve = Capacity;
            else
            {
                Reserve += amount;
            }

            Console.WriteLine("Station tank refueled");
            Monitor.PulseAll(lockObject);
        }
    }
}