using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableItem : MonoBehaviour, IInteractable
{
    [Header("Dados do Item")]
    [SerializeField] private string nomeDoItem = "Chave da Asa Leste";
    [SerializeField] private int quantidade = 1;

   
    // Implementação obrigatória do contrato IInteractable.
    
    public void Interact()
    {
        EfetuarColeta();
    }

    private void EfetuarColeta()
    {
        Debug.Log($"[Inventário] Coletou: {quantidade}x {nomeDoItem}.");

        // Aqui você integraria com o seu sistema central de Inventário real, ex:
        // Inventario.Instancia.AdicionarItem(nomeDoItem, quantidade);

        // Destrói o objeto do mapa já que ele foi coletado
        Destroy(gameObject);
    }
}