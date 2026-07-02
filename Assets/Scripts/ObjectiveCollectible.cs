using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class ObjectiveCollectible : MonoBehaviour, IInteractable
{
    [Header("Configurações do Objetivo")]
    [SerializeField] private string nomeItem = "Artefato de Missão";

    // Evento estático que avisa qualquer sistema interessado quando UM item for coletado
    public static event Action OnItemCollected;

    public void Interact()
    {
        Coletar();
    }

    private void Coletar()
    {
        Debug.Log($"[Objetivo] {nomeItem} coletado com sucesso!");

        // Dispara o evento para quem estiver ouvindo (VictoryManager)
        OnItemCollected?.Invoke();

        Destroy(gameObject);
    }
}