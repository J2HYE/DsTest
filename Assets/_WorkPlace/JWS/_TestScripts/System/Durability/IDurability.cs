public interface IDurability
{
    int CurrentDurability { get; }      // 현재 내구도
    int MaxDurability { get; }          // 최대 내구도
    bool IsBroken { get; }              // 내구도 0 여부
    float DurabilityPercentage { get; } // 내구도 퍼센트 (0.0 ~ 1.0)

    void Decrease(int amount);          // 내구도 감소
    void Repair(int amount);            // 내구도 수리
}
