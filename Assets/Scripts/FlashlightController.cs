using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FlashlightBattery))]
[RequireComponent(typeof(FlashlightVisuals))]
public class FlashlightController : MonoBehaviour
{
    private FlashlightBattery battery;
    private FlashlightVisuals visuals;
    private bool isOn = false;

    private void Awake()
    {
        battery = GetComponent<FlashlightBattery>();
        visuals = GetComponent<FlashlightVisuals>();
    }

    private void Start()
    {
        visuals.SetLightState(isOn);

        // Se inscrevendo nos eventos da bateria
        battery.OnBatteryLow += HandleBatteryLow;
        battery.OnBatteryEmpty += HandleBatteryEmpty;
    }

    private void Update()
    {
        if (PauseMenu.isGamePaused) return;

        // Input usando o novo Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ToggleFlashlight();
        }

        // Se estiver ligada, drena a bateria gradualmente
        if (isOn && battery.CurrentBattery > 0)
        {
            battery.Drain(Time.deltaTime);
        }
    }

    private void ToggleFlashlight()
    {
        if (battery.CurrentBattery <= 0) return; // Não liga sem bateria

        isOn = !isOn;
        visuals.SetLightState(isOn);
    }

    private void HandleBatteryLow()
    {
        // Chance aleatória por frame para o efeito de game juice não ser repetitivo ou travado
        if (Random.value < 0.02f)
        {
            visuals.TriggerFlicker();
        }
    }

    private void HandleBatteryEmpty()
    {
        isOn = false;
        visuals.SetLightState(false);
    }

    private void OnDestroy()
    {
        // Boa prática: desinscrever dos eventos para evitar Memory Leaks
        battery.OnBatteryLow -= HandleBatteryLow;
        battery.OnBatteryEmpty -= HandleBatteryEmpty;
    }
}