using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        // SOLID (DIP): Em vez de usar 'new LinearMover()', buscamos de forma modular 
        // ou definimos um fallback seguro caso o componente exista ou precise ser instanciado de forma desacoplada.
        _creditMover = GetComponent<ICreditMover>() ?? new LinearMover();
    }

    private void Start()
    {
        SetupInitialPosition();
        StartCredits();
    }

    private void Update()
    {
        if (!_isScrolling) return;

        _timeCounter += Time.deltaTime;
        _creditMover.Move(creditsTextRect, scrollSpeed, Time.deltaTime);

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
        _timeCounter = 0f;
    }

    private void EndCredits()
    {
        _isScrolling = false;
        Debug.Log("Cr�ditos finalizados! Carregando a cena inicial...");

        // O CursorManager (que � DontDestroyOnLoad) vai interceptar esse carregamento automaticamente!
        SceneManager.LoadScene("telaInicial");
    }

    public void UpdateText(string textContent)
    {
        if (creditsTextMesh != null)
        {
            creditsTextMesh.text = textContent;
        }
    }
}