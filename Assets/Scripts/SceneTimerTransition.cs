using UnityEngine;

public class SceneTimerTransition : MonoBehaviour
{
    [Header("Configurações de Tempo")]
    [SerializeField] private float _delayInSeconds = 5f; // O "float" para definir os segundos
    [SerializeField] private string _targetSceneName = "GameScene"; // Nome da cena do jogo

    private ISceneLoader _sceneLoader;
    private float _timeElapsed = 0f; // Variável float que conta o tempo passado

    private void Awake()
    {
        // SOLID: Buscando a abstração (Inversão de Dependência)
        _sceneLoader = GetComponent<ISceneLoader>();

        if (_sceneLoader == null)
        {
            Debug.LogError($"[SceneTimerTransition] Erro: Falta um componente ISceneLoader (ex: UnitySceneLoader) no GameObject '{gameObject.name}'!");
        }
    }

    private void Update()
    {
        if (_sceneLoader == null) return;

        // Incrementa o float com o tempo passado desde o último frame
        _timeElapsed += Time.deltaTime;

        // Se o tempo decorrido passar dos segundos estipulados...
        if (_timeElapsed >= _delayInSeconds)
        {
            // Desativa o script para garantir que LoadScene só seja chamado uma vez
            enabled = false;

            // Carrega a cena através da abstração
            _sceneLoader.LoadScene(_targetSceneName);
        }
    }
}