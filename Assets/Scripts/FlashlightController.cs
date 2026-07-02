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

        // No Update, logo após visuals.SetLightState(isOn);
        if (isOn)
        {
            NotifyEnemiesHitByFlashlight();
        }

        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            TryReloadFlashlight();
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

    private void NotifyEnemiesHitByFlashlight()
    {
        // Define o raio e direção do feixe da lanterna
        float flashlightRange = 20f; // Ajuste conforme necessário
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        RaycastHit[] hits = Physics.SphereCastAll(origin, 1.5f, direction, flashlightRange);
        foreach (var hit in hits)
        {
            var affectable = hit.collider.GetComponent<IFlashlightAffectable>();
            if (affectable != null)
            {
                affectable.onFlashlightHit(origin);
            }
        }
    }

    private void TryReloadFlashlight()
    {
        BatteryInventory inventory = GetComponent<BatteryInventory>();

        if (inventory != null && inventory.BatteryCount > 0)
        {
            // Verifica se a lanterna já não está cheia (opcional)
            if (battery.CurrentBattery < 100f)
            {
                if (inventory.ConsumeBattery())
                {
                    battery.Recharge(25f); // Valor da recarga
                    Debug.Log("Lanterna recarregada usando uma bateria do inventário!");
                }
            }
        }
    }

}