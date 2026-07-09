using UnityEngine;
using TMPro;

public class TextoPiscando : MonoBehaviour
{
         public float velocidade = 1.5f;
         public float alphaMin = 0.3f;
         public float alphaMax = 1f;

         private TextMeshProUGUI texto;
         private Color corOriginal;

    void Start()
        {
            texto = GetComponent<TextMeshProUGUI>();
            corOriginal = texto.color;
        }

    void Update()
        {
            float t = (Mathf.Sin(Time.time * velocidade) + 1f) / 2f;

            Color cor = corOriginal;
            cor.a = Mathf.Lerp(alphaMin, alphaMax, t);

            texto.color = cor;
        }
}