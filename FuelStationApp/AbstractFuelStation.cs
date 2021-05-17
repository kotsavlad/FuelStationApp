using System;
using System.Threading;

public abstract record AbstractFuelStation(int Capacity, int ColumnCount)
{
    public int Reserve { get; protected set; }

    public virtual bool TryRefuel(Car car, int volume)
    {
        var isHandled = false;
        AcquireColumn();
        ReserveEnterCS();
        if (volume <= Reserve)
        {
            Console.WriteLine($"Fuel reserve is {Reserve}. Fueling began for car {car.Name}");
            Reserve -= volume;
            isHandled = true;
        }

        ReserveExitCS();

        if (isHandled)
        {
            // refueling delay
            Thread.Sleep(100 * volume);
            Console.WriteLine($"{car.Name} fueling finished");
        }

        ReleaseColumn();
        return isHandled;
    }

    public virtual void Fill(int amount)
    {
        if (amount <= 0) return;

        ReserveEnterCS();
        Console.WriteLine("\nStation tank refueling began");
        Reserve = Math.Min(Capacity, Reserve + amount);
        Thread.Sleep(20 * amount);
        Console.WriteLine($"Station tank refueled. Current reserve is {Reserve}.\n");
        ReserveExitCS();
    }

    public abstract void ReserveEnterCS();
    public abstract void ReserveExitCS();
    public abstract void AcquireColumn();
    public abstract void ReleaseColumn();
}