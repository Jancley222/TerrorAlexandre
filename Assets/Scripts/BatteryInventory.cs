using UnityEngine;
using System;

public class BatteryInventory : MonoBehaviour
{
    private int batteryCount = 0;

    // Evento que avisa a UI (ou outros sistemas) quando a quantidade muda
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