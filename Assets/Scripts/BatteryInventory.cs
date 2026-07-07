using UnityEngine;
using System;

public class BatteryInventory : MonoBehaviour
{
    private int batteryCount = 0;

    public event Action<int> OnBatteryCountChanged;

    public int BatteryCount => batteryCount;

    public void AddBattery(int amount = 1)
    {
        batteryCount += amount;
        OnBatteryCountChanged?.Invoke(batteryCount);
    }

    public bool ConsumeBattery()
    {
        if (batteryCount > 0)
        {
            batteryCount--;
            OnBatteryCountChanged?.Invoke(batteryCount);
            return true;
        }
        return false;
    }
}