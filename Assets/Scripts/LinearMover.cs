using UnityEngine;

// S do SOLID: Esta classe tem apenas UMA responsabilidade: mover o RectTransform para cima.
// O do SOLID: Se você quiser um movimento em curvas ou acelerado no futuro, basta criar outra classe que implemente ICreditMover sem alterar esta.
public class LinearMover : ICreditMover
{
    public void Move(RectTransform rectTransform, float speed, float deltaTime)
    {
        if (rectTransform == null) return;

        // Move o componente de UI verticalmente
        rectTransform.anchoredPosition += new Vector2(0, speed * deltaTime);
    }
}