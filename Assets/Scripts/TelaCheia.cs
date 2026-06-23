using UnityEngine;

public class TelaCheia : MonoBehaviour
{
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreenMode = isFullscreen
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;

        Debug.Log("O botão foi clicado! Valor: " + isFullscreen);
    }
}