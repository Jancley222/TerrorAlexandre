using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems; // Necessário para detectar eventos de mouse/ponteiro da UI


// Ele implementa interfaces nativas da Unity para detectar quando o mouse entra e sai do elemento.
public class EffecOfButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Variável para definir o tamanho que o botão alcançará ao passar o mouse.Altere no Inspector se necessário.
    [SerializeField] private Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);

    // Variável para armazenar a escala original do botão (normalmente 1, 1, 1).
    private Vector3 originalScale;

    private void Awake()
    {
        // Guardamo a escala inicial do RectTransform assim que o jogo começa para sabermos para qual tamanho retornar.
        originalScale = transform.localScale;
    }

    // Este método é chamado automaticamente pela Unity quando o mouse PASSA POR CIMA DO BOTÃO.
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Alteramos a escala do transform do próprio botão para o tamanho final desejado (efeito de aumentar).
        transform.localScale = targetScale;
    }

    // Este método é chamado automaticamente pela Unity quando o mouse SAI de cima do botão.
    public void OnPointerExit(PointerEventData eventData)
    {
        // Retornamos a escala do transform do botão para o seu tamanho original.
        transform.localScale = originalScale;
    }

}
