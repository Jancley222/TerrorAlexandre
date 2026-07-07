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

        // Input usando o novo Input System (Mantido conforme seu script original)
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

        // MUDANÇA AQUI: Usando o GetButtonDown clássico da Unity
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
        // BUSCA MELHORADA: Procura o inventário no objeto atual ou nos objetos pais (como o Player)
        BatteryInventory inventory = GetComponentInParent<BatteryInventory>();

        if (inventory == null)
        {
            Debug.LogWarning("[Lanterna] Erro: Não foi possível encontrar o 'BatteryInventory' no Player ou na Lanterna!");
            return;
        }

        // Se encontrou o inventário, verifica a quantidade de baterias
        if (inventory.BatteryCount <= 0)
        {
            Debug.Log("[Lanterna] Você apertou R, mas não tem nenhuma bateria no inventário!");
            return;
        }

        // Verifica se a lanterna já está cheia
        if (battery.CurrentBattery >= 100f)
        {
            Debug.Log("[Lanterna] Bateria já está cheia (100%). Não precisa recarregar.");
            return;
        }

        // Se passou em todos os testes, consome e recarrega
        if (inventory.ConsumeBattery())
        {
            battery.Recharge(25f); // Valor da recarga
            Debug.Log("[Lanterna] Lanterna recarregada com sucesso usando 1 bateria!");
        }
    }

}