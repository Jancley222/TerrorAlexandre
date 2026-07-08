using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    // Outros scripts podem verificar se o jogo está pausado
    public static bool isGamePaused = false;
    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                Continuar();
            else
                Pausar();
        }
    }
    public void Pausar()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }
    public void Continuar()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    public void SairJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}