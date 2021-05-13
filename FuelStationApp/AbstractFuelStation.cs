public abstract record AbstractFuelStation(int Capacity, int ColumnCount)
{
    public int Reserve { get; set; }

    public abstract bool TryRefuel(Car car, int volume);

    public abstract void Fill(int amount);
}