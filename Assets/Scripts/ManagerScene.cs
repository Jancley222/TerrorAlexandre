using UnityEngine;
using UnityEngine.SceneManagement;
public class ManagerScene : MonoBehaviour
{
    public void StartScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void MenuSpace()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
