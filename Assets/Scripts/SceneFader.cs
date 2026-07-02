using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class SceneFader : MonoBehaviour
{
    [Header("Configurações de Transição")]
    [SerializeField] private float duracaoFade = 1.5f;
    [SerializeField] private string nomeCenaVitoria = "VictoryScene";

    private CanvasGroup canvasGroup;
    private bool estaEmTransicao = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // Garante que o jogo comece com a tela limpa e sem bloquear cliques
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        // Escuta o gerenciador de vitória
        VictoryManager.OnVictoryAchieved += IniciarFadeOut;
    }

    private void OnDisable()
    {
        VictoryManager.OnVictoryAchieved -= IniciarFadeOut;
    }

    private void IniciarFadeOut()
    {
        if (!estaEmTransicao)
        {
            StartCoroutine(FadeOutECarregarCena());
        }
    }

    private IEnumerator FadeOutECarregarCena()
    {
        estaEmTransicao = true;
        canvasGroup.blocksRaycasts = true; // Bloqueia inputs físicos ou cliques do jogador

        float tempoPassado = 0f;
        while (tempoPassado < duracaoFade)
        {
            tempoPassado += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(tempoPassado / duracaoFade);
            yield return null; // Espera o próximo frame
        }

        canvasGroup.alpha = 1f;

        // Restaura as configurações do mouse para a tela de vitória/menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Carrega a nova cena do jogo
        SceneManager.LoadScene(nomeCenaVitoria);
    }
}