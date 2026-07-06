using UnityEngine;
using TMPro;

public class CreditsController : MonoBehaviour
{
    [Header("Configurações de UI")]
    [SerializeField] private RectTransform creditsTextRect;
    [SerializeField] private TextMeshProUGUI creditsTextMesh;

    [Header("Configurações de Movimento")]
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float startPositionY = -600f; // Geralmente abaixo da tela
    [SerializeField] private float endPositionY = 1200f;   // Geralmente acima da tela

    private ICreditMover _creditMover;
    private bool _isScrolling = false;

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

        // Executa o movimento delegando a responsabilidade para o mover correspondente
        _creditMover.Move(creditsTextRect, scrollSpeed, Time.deltaTime);

        // Verifica se os créditos passaram do limite final
        if (creditsTextRect.anchoredPosition.y >= endPositionY)
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
    }

    private void EndCredits()
    {
        _isScrolling = false;
        Debug.Log("Créditos finalizados!");
        // Aqui você pode carregar a cena do menu, dar fade out, etc.
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