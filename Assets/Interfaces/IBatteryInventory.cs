using System;
using UnityEngine;

public interface IBatteryInventory
{
    int BatteryCount { get; }
    event Action<int> OnBatteryCountChanged;
}