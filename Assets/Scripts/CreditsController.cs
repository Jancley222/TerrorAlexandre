using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // IMPORTANTE: Necess�rio para usar o SceneManager

public class CreditsController : MonoBehaviour
{
    [Header("Configura��es de UI")]
    [SerializeField] private RectTransform creditsTextRect;
    [SerializeField] private TextMeshProUGUI creditsTextMesh;

    [Header("Configura��es de Movimento")]
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float startPositionY = -600f;
    [SerializeField] private float endPositionY = 1200f;

    [Header("Configura��es de Tempo e Cena")]
    [Tooltip("Tempo m�ximo em segundos que os cr�ditos ficar�o ativos na tela antes de mudar de cena.")]
    [SerializeField] private float maxDisplayTime = 15f;
    [Tooltip("Nome exato da cena inicial para onde o jogo deve ir.")]
    [SerializeField] private string menuSceneName = "MenuPrincipal";

    private ICreditMover _creditMover;
    private bool _isScrolling = false;
    private float _timeCounter = 0f;

    private void Awake()
    {
        // D do SOLID: Dependemos de uma abstra��o (interface) e n�o da implementa��o r�gida.
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

        // 1. Gerencia e incrementa o tempo de exibi��o
        _timeCounter += Time.deltaTime;
        _creditMover.Move(creditsTextRect, scrollSpeed, Time.deltaTime);

        // 3. Verifica se o tempo acabou OU se os cr�ditos passaram do limite f�sico na tela
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
        _timeCounter = 0f; // Reinicia o contador ao come�ar
    }

    private void EndCredits()
    {
        _isScrolling = false;
        Debug.Log("Cr�ditos finalizados! Carregando a cena inicial...");

        // Troca a cena para o in�cio usando o nome definido no Inspector
        SceneManager.LoadScene(menuSceneName);
    }

    // Exemplo de como mudar o texto dinamicamente se necess�rio (Mantendo responsabilidade isolada)
    public void UpdateText(string textContent)
    {
        if (creditsTextMesh != null)
        {
            creditsTextMesh.text = textContent;
        }
    }
}