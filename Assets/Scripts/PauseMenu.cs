using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        isGamePaused = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Resumed");
    }

    public void Pause()
    {
        isGamePaused = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        InputSystem.ResetHaptics();
        foreach (var device in InputSystem.devices)
        {
            if (device is Keyboard || device is Mouse)
            {
                InputSystem.ResetDevice(device);
            }
        }

        Debug.Log("Paused");
    }
}