using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    [SerializeField] private PlayerInput playerInput;

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

        if (playerInput != null)
        {
            playerInput.enabled = true;
        }

        Debug.Log("Resumed");
    }

    public void Pause()
    {
        isGamePaused = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerInput != null)
        {
            playerInput.enabled = false;
        }

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