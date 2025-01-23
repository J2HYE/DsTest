public class Durability : IDurability
{
    public int CurrentDurability { get; private set; }
    public int MaxDurability { get; private set; }
    public bool IsBroken => CurrentDurability <= 0;
    public float DurabilityPercentage => (float)CurrentDurability / MaxDurability;

    public Durability(int maxDurability)
    {
        MaxDurability = maxDurability;
        CurrentDurability = maxDurability;
    }

    public void Decrease(int amount)
    {
        CurrentDurability -= amount;
        if (CurrentDurability < 0)
        {
            CurrentDurability = 0;
        }
    }

    public void Repair(int amount)
    {
        CurrentDurability += amount;
        if (CurrentDurability > MaxDurability)
        {
            CurrentDurability = MaxDurability;
        }
    }

    public float GetDurabilityPercentage()
    {
        return DurabilityPercentage;
    }

    public Durability Clone()
    {
        return new Durability(this.MaxDurability)
        {
            CurrentDurability = this.CurrentDurability
        };
    }
}
