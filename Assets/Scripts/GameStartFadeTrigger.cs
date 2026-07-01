// Importa a biblioteca base da Unity para interagir com o ciclo de vida do jogo
using UnityEngine;

// Cria a classe responsável exclusivamente por detectar o início do jogo e ativar o efeito
public class GameStartFadeTrigger : MonoBehaviour
{
    // Expõe um campo no Inspector para arrastares o GameObject que possui o script TextFader
    [SerializeField] private GameObject faderObject;

    // Cria uma variável privada para armazenar a referência abstrata da interface IFader
    private IFader fader;

    // Método nativo da Unity chamado automaticamente assim que o script acorda na memória
    void Awake()
    {
        // Procura e armazena o componente que implementa a interface IFader no objeto arrastado
        fader = faderObject.GetComponent<IFader>();
    }

    // Método nativo da Unity chamado automaticamente no primeiro frame em que o jogo inicia
    void Start()
    {
        // Verifica se a referência do fader foi encontrada com sucesso para evitar erros no console
        if (fader != null)
        {
            // Dá a ordem para iniciar o efeito de Fade Out assim que o player entra no jogo
            fader.FadeOut();
        }
    }
}