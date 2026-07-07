using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    [Header("Configurações de Fluxo")]
    [Tooltip("O índice da cena do menu principal no Build Settings (geralmente é 0).")]
    [SerializeField] private int _menuSceneIndex = 0;

    private static CursorManager _instance;

    private void Awake()
    {
        // Padrão Singleton simples para evitar duplicatas do gerenciador ao voltar para o menu
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Força a checagem assim que o jogo inicia ou o objeto acorda
        EvaluateCursorState(SceneManager.GetActiveScene().buildIndex);
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
        EvaluateCursorState(scene.buildIndex);
    }

    // SOLID (SRP): Responsabilidade única de ditar o estado do cursor pelo índice da cena
    private void EvaluateCursorState(int sceneIndex)
    {
        if (sceneIndex == _menuSceneIndex)
        {
            UnlockCursor();
            Debug.Log("[CursorManager] Mouse LIBERADO na cena de índice: " + sceneIndex);
        }
        else
        {
            LockCursor();
            Debug.Log("[CursorManager] Mouse BLOQUEADO na cena de índice: " + sceneIndex);
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