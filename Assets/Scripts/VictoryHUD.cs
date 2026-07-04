using UnityEngine;
using TMPro;

public class VictoryHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoProgresso;

    private void OnEnable()
    {
        VictoryManager.OnProgressChanged += AtualizarHUD;
    }

    private void OnDisable()
    {
        VictoryManager.OnProgressChanged -= AtualizarHUD;
    }

    private void AtualizarHUD(int coletados, int total)
    {
        if (textoProgresso != null)
            textoProgresso.text = $"Itens coletados: {coletados} / {total}\nRestantes: {total - coletados}";
    }
}
