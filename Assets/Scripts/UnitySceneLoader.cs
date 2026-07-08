using UnityEngine;
using UnityEngine.SceneManagement;

// REMOVA a declaração da interface "public interface ISceneLoader" que estava aqui em cima!

public class UnitySceneLoader : MonoBehaviour, ISceneLoader
{
    [SerializeField] private string cenaAlvoPadrao;

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