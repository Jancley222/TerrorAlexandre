using UnityEngine;
using UnityEngine.UI;

public class SensibiidadeMenu : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider.minValue = 0.2f;
        slider.maxValue = 5f;

        // Carrega o valor salvo
        slider.value = PlayerPrefs.GetFloat("Sensibilidade", 1f);

        // Sempre que mover o slider, salva o valor
        slider.onValueChanged.AddListener(SalvarSensibilidade);
    }

    void SalvarSensibilidade(float valor)
    {
        PlayerPrefs.SetFloat("Sensibilidade", valor);
        PlayerPrefs.Save();
    }
}