using UnityEngine;

public class VictorySceneLoader : MonoBehaviour
{
    [Header("Configurações de Transição")]
    [SerializeField] private string nomeCenaVitoria = "VictoryScene";

    [Header("Componentes Requeridos")]
    [SerializeField] private UnitySceneLoader sceneLoader;

    private void Awake()
    {
        // Fallback: Se esquecer de arrastar no Inspector, tenta buscar no mesmo GameObject
        if (sceneLoader == null)
        {
            sceneLoader = GetComponent<UnitySceneLoader>();
        }
    }

    private void OnEnable()
    {
        // Se inscreve no evento de vitória do VictoryManager
        VictoryManager.OnVictoryAchieved += MudarParaCenaDeVitoria;
    }

    private void OnDisable()
    {
        // Evita vazamento de memória
        VictoryManager.OnVictoryAchieved -= MudarParaCenaDeVitoria;
    }

    private void MudarParaCenaDeVitoria()
    {
        if (sceneLoader != null)
        {
            Debug.Log("[VictorySceneLoader] Evento de vitória detectado. Solicitando carregamento de cena...");
            sceneLoader.CarregarCena(nomeCenaVitoria);
        }
        else
        {
            Debug.LogError("[VictorySceneLoader] Falha ao transicionar: UnitySceneLoader não foi encontrado ou referenciado!");
        }
    }
}