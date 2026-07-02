using UnityEngine;
using TMPro;

public class BatteryInventoryUI : MonoBehaviour
{
    [Header("Componentes de UI")]
    [SerializeField] private TextMeshProUGUI batteryText;

    [Header("Referências")]
    [SerializeField] private BatteryInventory playerInventory;

    private void Start()
    {
        if (playerInventory != null)
        {
            // Inscreve no evento para atualizar a UI automaticamente
            playerInventory.OnBatteryCountChanged += UpdateBatteryUI;

            // Inicializa o texto com o valor atual
            UpdateBatteryUI(playerInventory.BatteryCount);
        }
        else
        {
            Debug.LogError("[UI] Referência do BatteryInventory não foi atribuída no Inspetor!");
        }
    }

    private void UpdateBatteryUI(int count)
    {
        if (batteryText != null)
        {
            batteryText.text = $"x{count}";
        }
    }

    private void OnDestroy()
    {
        // Desinscreve para evitar Memory Leaks
        if (playerInventory != null)
        {
            playerInventory.OnBatteryCountChanged -= UpdateBatteryUI;
        }
    }
}