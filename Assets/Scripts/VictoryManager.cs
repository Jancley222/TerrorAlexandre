using UnityEngine;
using System;

public class VictoryManager : MonoBehaviour
{
    private int totalItemsNaCena;
    private int itemsColetados;

    // Evento que avisa que o jogador coletou tudo e ganhou o jogo
    public static event Action OnVictoryAchieved;

    private void Awake()
    {
        // Busca automaticamente todos os coletáveis da cena ao iniciar
        ObjectiveCollectible[] itens = FindObjectsByType<ObjectiveCollectible>(FindObjectsSortMode.None);
        totalItemsNaCena = itens.Length;
        itemsColetados = 0;

        Debug.Log($"[Vitória] Sistema iniciado. Itens necessários para vencer: {totalItemsNaCena}");
    }

    private void OnEnable()
    {
        // Se inscreve no evento de coleta do item
        ObjectiveCollectible.OnItemCollected += RegistrarColeta;
    }

    private void OnDisable()
    {
        // Desinscrição obrigatória para evitar vazamento de memória
        ObjectiveCollectible.OnItemCollected -= RegistrarColeta;
    }

    private void RegistrarColeta()
    {
        itemsColetados++;
        Debug.Log($"[Vitória] Progresso: {itemsColetados}/{totalItemsNaCena}");

        if (itemsColetados >= totalItemsNaCena && totalItemsNaCena > 0)
        {
            IniciarVitoria();
        }
    }

    private void IniciarVitoria()
    {
        Debug.Log("[Vitória] Condição de vitória atingida! Notificando sistemas de transição...");
        OnVictoryAchieved?.Invoke();
    }
}