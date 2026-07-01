// Importa o sistema de coleções para permitir o uso de Coroutines (rotinas de tempo)
using System.Collections;
// Importa a biblioteca base da Unity para gerenciar GameObjects e componentes
using UnityEngine;
// Importa a biblioteca do TextMeshPro para podermos manipular o componente de texto moderno
using TMPro;

// Cria a classe TextFader que herda de MonoBehaviour e implementa a interface IFader
public class TextFader : MonoBehaviour, IFader
{
    // Expõe um campo privado no Inspector para arrastares o teu componente de texto
    [SerializeField] private TMP_Text textComponent;

    // Expõe um campo privado no Inspector para definir a duração do efeito em segundos
    [SerializeField] private float fadeDuration = 2f;

    // Implementa o método FadeOut exigido pela interface IFader
    public void FadeOut()
    {
        // Inicia a Coroutine que vai diminuir o alpha do texto gradualmente frame a frame
        StartCoroutine(FadeOutRoutine());
    }

    // Cria a Coroutine responsável pelo cálculo matemático do esmaecimento
    private IEnumerator FadeOutRoutine()
    {
        // Guarda a cor original do texto para manter o Vermelho, Verde e Azul intactos
        Color startColor = textComponent.color;

        // Inicializa uma variável de controle para contar o tempo decorrido do efeito
        float elapsedTime = 0f;

        // Cria um loop que continuará rodando até que o tempo decorrido alcance a duração definida
        while (elapsedTime < fadeDuration)
        {
            // Soma o tempo que passou desde o último frame à nossa variável de controle
            elapsedTime += Time.deltaTime;

            // Calcula o novo valor de Alpha interpolando linearmente de 1 (visível) a 0 (invisível)
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            // Aplica a nova cor ao texto, mantendo o RGB original mas atualizando o canal Alpha
            textComponent.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            // Pausa a execução da Coroutine aqui e espera até o próximo frame para continuar o loop
            yield return null;
        }

        // Garante de forma limpa que o Alpha final termine exatamente em zero ao sair do loop
        textComponent.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }
}