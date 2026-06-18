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
        // NOVO: Se o jogo estiver pausado, não permite ligar/desligar a lanterna
        if (PauseMenu.isGamePaused) return;

        //verifica o clique do botão esquerdo no Novo Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ToggleFlashlight();
        }
    }

    private void ToggleFlashlight()
    {
        if (flashlightLight != null)
        {
            //inverte o estado atual (se true vira false, se false vira true)
            isOn = !isOn;

            //liga ou desliga o componente de luz
            flashlightLight.enabled = isOn;
        }
    }
}