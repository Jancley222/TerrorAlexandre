using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    [Header("Configurações de Fluxo (Telas com Mouse Liberado)")]
    [Tooltip("Nome exato da cena do Menu Principal.")]
    [SerializeField] private string _menuSceneName = "MenuPrincipal";

    [Tooltip("Nome exato da cena de Vitória.")]
    [SerializeField] private string _victorySceneName = "VictoryScene";

    private static CursorManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Avalia o estado atual usando o nome da cena ativa
        EvaluateCursorState(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EvaluateCursorState(scene.name);
    }

    // SOLID (SRP): Responsabilidade única de ditar o estado do cursor comparando os nomes das cenas
    private void EvaluateCursorState(string sceneName)
    {
        // Se a cena atual for o Menu Principal OU for a Cena de Vitória, o mouse é LIBERADO
        if (sceneName == _menuSceneName || sceneName == _victorySceneName)
        {
            UnlockCursor();
            Debug.Log("[CursorManager] Mouse LIBERADO na cena: " + sceneName);
        }
        else
        {
            LockCursor();
            Debug.Log("[CursorManager] Mouse BLOQUEADO na cena: " + sceneName);
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}