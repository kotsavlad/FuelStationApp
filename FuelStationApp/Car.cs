using System;
using System.Threading;

public record Car(string Name, AbstractFuelStation Station, int TankVolume, int RefuelCount, int RequiredVolume = 0)
{
    private static readonly Random random = new Random();

    public virtual int GetVolume() =>
        RequiredVolume == 0 ? random.Next(TankVolume) + 1 : RequiredVolume;

    public void Run()
    {
        Console.WriteLine($"{Name} STARTED; refuel count is {RefuelCount}");
        for (int i = 0; i < RefuelCount; i++)
        {
            var volume = GetVolume();
            Console.WriteLine($"{Name} tries to get {volume} litres for {i + 1} times");
            int trialCount = 0;
            while (!Station.TryRefuel(this, volume))
            {
                Console.WriteLine(
                    $"{Name} trial #{++trialCount} of refueling #{i + 1} was unsuccessfully due to insufficient reserve. {Name} is waiting");
                Thread.Sleep(random.Next(1, 5) * 1000);
            }

            Console.WriteLine($"{Name} left the station for {i + 1} times");
            Thread.Sleep(random.Next(1, 3) * 1000);
        }

        Console.WriteLine($"{Name} SAID GOOD-BYE");
    }
}