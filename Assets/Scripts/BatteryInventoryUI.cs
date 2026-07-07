using UnityEngine;
using TMPro;

public class BatteryInventoryUI : MonoBehaviour
{
    [Header("Componentes de UI")]
    [SerializeField] private TextMeshProUGUI batteryCountText;    // Texto focado no inventário (Ex: x5)
    [SerializeField] private TextMeshProUGUI interactionPromptText; // Texto para alertas (Ex: Pressione R...)

    [Header("Referências")]
    [SerializeField] private BatteryInventory playerInventory;
    [SerializeField] private FlashlightController flashlightController;

    private bool isFlashlightEmpty = false;

    private void Start()
    {
        // Inscrição no Inventário
        if (playerInventory != null)
        {
            playerInventory.OnBatteryCountChanged += HandleBatteryCountChanged;
        }
        else
        {
            Debug.LogError("[UI] Referência do BatteryInventory não foi atribuída no Inspetor!");
        }

        // Inscrição na Lanterna
        if (flashlightController != null)
        {
            flashlightController.OnFlashlightEmptyStateChanged += HandleFlashlightEmptyStateChanged;
        }
        else
        {
            Debug.LogWarning("[UI] FlashlightController não atribuído. Mensagem de recarga não funcionará.");
        }

        // Inicialização do estado visual inicial
        RefreshUI();
    }

    private void HandleBatteryCountChanged(int count)
    {
        RefreshUI();
    }

    private void HandleFlashlightEmptyStateChanged(bool isEmpty)
    {
        isFlashlightEmpty = isEmpty;
        RefreshUI();
    }

    // SOLID: Centraliza a atualização visual isolando regras de renderização por componente
    private void RefreshUI()
    {
        int currentCount = playerInventory != null ? playerInventory.BatteryCount : 0;

        // 1. Atualiza o contador de quantidade de baterias
        if (batteryCountText != null)
        {
            batteryCountText.text = $"x{currentCount}";
        }

        // 2. Controla de forma independente o texto de prompt/alerta no Canvas
        if (interactionPromptText != null)
        {
            if (isFlashlightEmpty && currentCount > 0)
            {
                interactionPromptText.text = "Pressione R para recarregar";
                interactionPromptText.gameObject.SetActive(true); // Garante que está visível
            }
            else
            {
                interactionPromptText.text = string.Empty;
                interactionPromptText.gameObject.SetActive(false); // Oculta quando não necessário
            }
        }
    }

    private void OnDestroy()
    {
        // Desinscrições preventivas para evitar Memory Leaks
        if (playerInventory != null)
        {
            playerInventory.OnBatteryCountChanged -= HandleBatteryCountChanged;
        }

        if (flashlightController != null)
        {
            flashlightController.OnFlashlightEmptyStateChanged -= HandleFlashlightEmptyStateChanged;
        }
    }
}