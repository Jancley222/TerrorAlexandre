using UnityEngine;
using System;

public class FlashlightBattery : MonoBehaviour
{
    [Header("Configurações de Bateria")]
    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float drainRate = 2f; // Carga perdida por segundo
    [SerializeField] private float flickerThreshold = 20f; // Começa a piscar abaixo disso

    private float currentBattery;

    // Eventos para avisar outras classes sem criar acoplamento rígido
    public event Action<float> OnBatteryChanged;
    public event Action OnBatteryLow;
    public event Action OnBatteryEmpty;

    public float CurrentBattery => currentBattery;
    public bool IsLow => currentBattery <= flickerThreshold && currentBattery > 0;

    private void Start()
    {
        currentBattery = maxBattery;
    }

    public void Drain(float deltaTime)
    {
        if (currentBattery <= 0) return;

        currentBattery -= drainRate * deltaTime;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

        OnBatteryChanged?.Invoke(currentBattery);

        if (currentBattery <= flickerThreshold && currentBattery > 0)
        {
            OnBatteryLow?.Invoke();
        }

        if (currentBattery <= 0)
        {
            OnBatteryEmpty?.Invoke();
        }
    }

    public void Recharge(float amount)
    {
        currentBattery = Mathf.Clamp(currentBattery + amount, 0f, maxBattery);
        OnBatteryChanged?.Invoke(currentBattery);
        Debug.Log($"Bateria recarregada. Carga atual: {currentBattery}%");
    }
}