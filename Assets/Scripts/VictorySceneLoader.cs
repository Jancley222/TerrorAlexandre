using UnityEngine;
using UnityEngine.SceneManagement;

public class VictorySceneLoader : MonoBehaviour
{
    [SerializeField] private string nomeCenaVitoria = "VictoryScene";

    private void OnEnable()
    {
        VictoryManager.OnVictoryAchieved += CarregarCenaVitoria;
    }

    private void OnDisable()
    {
        VictoryManager.OnVictoryAchieved -= CarregarCenaVitoria;
    }

    private void CarregarCenaVitoria()
    {
        SceneManager.LoadScene(nomeCenaVitoria);
    }
}
