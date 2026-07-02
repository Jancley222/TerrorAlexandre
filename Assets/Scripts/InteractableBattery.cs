using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableBattery : MonoBehaviour, IInteractable
{
    [Header("Configurações da Bateria")]
    [SerializeField] private string nomeItem = "Bateria de Lanterna";

    public void Interact()
    {
        ColetarBateria();
    }

    private void ColetarBateria()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Jogador");

        if (player != null)
        {
            BatteryInventory inventory = player.GetComponent<BatteryInventory>();

            if (inventory != null)
            {
                inventory.AddBattery(1);
                Debug.Log($"[Inventário] {nomeItem} guardada no inventário.");
                Destroy(gameObject);
                return;
            }
        }

    }
}