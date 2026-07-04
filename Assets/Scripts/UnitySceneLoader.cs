// UnitySceneLoader.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitySceneLoader : MonoBehaviour, ISceneLoader
{
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("[UnitySceneLoader] O nome da cena está vazio ou nulo!");
        }
    }
}