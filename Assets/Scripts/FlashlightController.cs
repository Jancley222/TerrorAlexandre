using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    //referência para o componente de luz da lanterna
    [SerializeField] private Light flashlightLight;

    //variável para controlar se a lanterna está ligada ou desligada
    private bool isOn = true;

    void Start()
    {
        //garante que a lanterna comece no estado correto
        if (flashlightLight != null)
        {
            flashlightLight.enabled = isOn;
        }
    }

    void Update()
    {
        if (PauseMenu.isGamePaused) return;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ToggleFlashlight();
        }
    }

    private void ToggleFlashlight()
    {
        if (flashlightLight != null)
        {
            isOn = !isOn;

            flashlightLight.enabled = isOn;
        }
    }
}