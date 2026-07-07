using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // IMPORTANTE: Necessário para usar o SceneManager

public class CreditsController : MonoBehaviour
{
    [Header("Configurações de UI")]
    [SerializeField] private RectTransform creditsTextRect;
    [SerializeField] private TextMeshProUGUI creditsTextMesh;

    [Header("Configurações de Movimento")]
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float startPositionY = -600f; // Geralmente abaixo da tela
    [SerializeField] private float endPositionY = 1200f;   // Geralmente acima da tela

    [Header("Configurações de Tempo e Cena")]
    [Tooltip("Tempo máximo em segundos que os créditos ficarão ativos na tela antes de mudar de cena.")]
    [SerializeField] private float maxDisplayTime = 15f;
    [Tooltip("Nome exato da cena inicial para onde o jogo deve ir.")]
    [SerializeField] private string menuSceneName = "MenuPrincipal";

    private ICreditMover _creditMover;
    private bool _isScrolling = false;
    private float _timeCounter = 0f; // Rastreador do tempo decorrido

    private void Awake()
    {
        // D do SOLID: Dependemos de uma abstração (interface) e não da implementação rígida.
        _creditMover = new LinearMover();
    }

    private void Start()
    {
        SetupInitialPosition();
        StartCredits();
    }

    private void Update()
    {
        if (!_isScrolling) return;

        // 1. Gerencia e incrementa o tempo de exibição
        _timeCounter += Time.deltaTime;

        // 2. Executa o movimento delegando a responsabilidade para o mover correspondente
        _creditMover.Move(creditsTextRect, scrollSpeed, Time.deltaTime);

        // 3. Verifica se o tempo acabou OU se os créditos passaram do limite físico na tela
        if (_timeCounter >= maxDisplayTime || creditsTextRect.anchoredPosition.y >= endPositionY)
        {
            EndCredits();
        }
    }

    public void SetupInitialPosition()
    {
        if (creditsTextRect != null)
        {
            creditsTextRect.anchoredPosition = new Vector2(creditsTextRect.anchoredPosition.x, startPositionY);
        }
    }

    public void StartCredits()
    {
        _isScrolling = true;
        _timeCounter = 0f; // Reinicia o contador ao começar
    }

    private void EndCredits()
    {
        _isScrolling = false;
        Debug.Log("Créditos finalizados! Carregando a cena inicial...");

        // Troca a cena para o início usando o nome definido no Inspector
        SceneManager.LoadScene(menuSceneName);
    }

    // Exemplo de como mudar o texto dinamicamente se necessário (Mantendo responsabilidade isolada)
    public void UpdateText(string textContent)
    {
        if (creditsTextMesh != null)
        {
            creditsTextMesh.text = textContent;
        }
    }
}