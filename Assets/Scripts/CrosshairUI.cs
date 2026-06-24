using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [Header("Configurações da Mira")]
    [SerializeField] private Sprite spriteMira;
    [SerializeField] private Vector2 tamanhoMira = new Vector2(32f, 32f);
    [SerializeField] private Color corMira = Color.white;

    private void Awake()
    {
        ConstruirMira();
    }

    private void ConstruirMira()
    {
        // Garante que o objeto tenha os componentes de UI necessários
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[UI] O CrosshairUI precisa estar dentro de um Canvas");
            return;
        }

        Image imagemMira = GetComponent<Image>();
        if (imagemMira == null)
        {
            imagemMira = gameObject.AddComponent<Image>();
        }

        // Configura o Sprite e comportamento visual (Estilo Minecraft)
        imagemMira.sprite = spriteMira;
        imagemMira.color = corMira;

        // Centraliza perfeitamente no meio da tela usando as âncoras do RectTransform
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = tamanhoMira;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
    }
}