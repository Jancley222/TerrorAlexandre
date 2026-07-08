using UnityEngine;
using UnityEngine.SceneManagement;

// Criando a interface que faltava para cumprir a Inversão de Dependência (D do SOLID)
public interface ISceneLoader
{
    void LoadScene(string nomeDaCena);
}

public class UnitySceneLoader : MonoBehaviour, ISceneLoader
{
    [SerializeField] private string cenaAlvoPadrao;

    // Alterado para implementar a interface ISceneLoader exigida pelo SceneTimerTransition
    public void LoadScene(string nomeDaCena)
    {
        if (string.IsNullOrEmpty(nomeDaCena))
        {
            Debug.LogError("[SceneLoader] O nome da cena não pode ser vazio ou nulo!");
            return;
        }

        Debug.Log($"[SceneLoader] Carregando a cena: {nomeDaCena}");
        SceneManager.LoadScene(nomeDaCena);
    }

    // Mantendo suporte ao método antigo que o VictorySceneLoader usa
    public void CarregarCena(string nomeDaCena)
    {
        LoadScene(nomeDaCena);
    }

    public void CarregarCenaPadrao()
    {
        LoadScene(cenaAlvoPadrao);
    }

    public void ReiniciarCenaAtual()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;
        LoadScene(cenaAtual);
    }
}