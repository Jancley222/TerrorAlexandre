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
        Debug.Log($"[Lanterna/Inventário] {nomeItem} coletada! Adicionado +{quantidadeCarga}% de carga.");

        // Exemplo de integração futura:
        // Lanterna.Instancia.Recarregar(quantidadeCarga);

        Destroy(gameObject);
    }
}