using UnityEngine;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(FlashlightBattery))]
[RequireComponent(typeof(FlashlightVisuals))]
public class FlashlightController : MonoBehaviour
{
    private FlashlightBattery battery;
    private FlashlightVisuals visuals;
    private bool isOn = false;

    // SOLID: Evento para notificar sistemas externos (como a UI) sobre a mudança no estado crítico da bateria
    public event Action<bool> OnFlashlightEmptyStateChanged;

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

        if (isOn)
        {
            NotifyEnemiesHitByFlashlight();
        }

        // Usando o GetButtonDown clássico da Unity
        if (Input.GetButtonDown("Reload"))
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
        // Correção: Especificando explicitamente o uso do UnityEngine.Random
        if (UnityEngine.Random.value < 0.02f)
        {
            visuals.TriggerFlicker();
        }
    }

    private void HandleBatteryEmpty()
    {
        isOn = false;
        visuals.SetLightState(false);

        // Notifica que a lanterna ficou sem bateria
        OnFlashlightEmptyStateChanged?.Invoke(true);
    }

    private void OnDestroy()
    {
        battery.OnBatteryLow -= HandleBatteryLow;
        battery.OnBatteryEmpty -= HandleBatteryEmpty;
    }

    private void NotifyEnemiesHitByFlashlight()
    {
        float flashlightRange = 20f;
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
        BatteryInventory inventory = GetComponentInParent<BatteryInventory>();

        if (inventory == null)
        {
            Debug.LogWarning("[Lanterna] Erro: Não foi possível encontrar o 'BatteryInventory' no Player ou na Lanterna!");
            return;
        }

        if (inventory.BatteryCount <= 0)
        {
            Debug.Log("[Lanterna] Você apertou R, mas não tem nenhuma bateria no inventário!");
            return;
        }

        if (battery.CurrentBattery >= 100f)
        {
            Debug.Log("[Lanterna] Bateria já está cheia (100%). Não precisa recarregar.");
            return;
        }

        if (inventory.ConsumeBattery())
        {
            battery.Recharge(100f); // Valor da recarga

            // Notifica que a lanterna saiu do estado de "bateria vazia"
            OnFlashlightEmptyStateChanged?.Invoke(false);

            Debug.Log("[Lanterna] Lanterna recarregada com sucesso usando 1 bateria!");
        }
    }
}