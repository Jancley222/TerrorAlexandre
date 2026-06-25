using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableBattery : MonoBehaviour, IInteractable
{
    [Header("Configurações da Bateria")]
    [SerializeField] private float quantidadeCarga = 25f;
    [SerializeField] private string nomeItem = "Bateria de Lanterna";

    public void Interact()
    {
        ColetarBateria();
    }

    private void ColetarBateria()
    {
        // Procura pelo componente de bateria no Player do mapa
        GameObject player = GameObject.FindGameObjectWithTag("Jogador");

        if (player != null)
        {
            FlashlightBattery playerBattery = player.GetComponentInChildren<FlashlightBattery>();

            if (playerBattery != null)
            {
                playerBattery.Recharge(quantidadeCarga);
                Debug.Log($"[Lanterna/Inventário] {nomeItem} coletada! Adicionado +{quantidadeCarga}% de carga.");
                Destroy(gameObject);
                return;
            }
        }

        Debug.LogWarning("Player ou componente FlashlightBattery não encontrado para aplicar a carga.");
    }
}