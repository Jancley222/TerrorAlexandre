using System.Collections;
using UnityEngine;

public class FlashlightVisuals : MonoBehaviour
{
    [SerializeField] private Light flashlightLight;
    [Header("Game Juice (Flicker)")]
    [SerializeField] private float minFlickerDelay = 0.05f;
    [SerializeField] private float maxFlickerDelay = 0.2f;

    private bool isFlickering = false;

    // MUDANÇA 1: Guardamos a referência da coroutine para podermos pará-la
    private Coroutine activeFlickerCoroutine;

    public void SetLightState(bool state)
    {
        if (flashlightLight != null)
        {
            // MUDANÇA 2: Se recebemos a ordem de desligar a luz e ela estiver piscando, matamos a coroutine.
            if (!state && isFlickering)
            {
                if (activeFlickerCoroutine != null)
                {
                    StopCoroutine(activeFlickerCoroutine);
                }
                isFlickering = false; // Resetamos o estado
            }

            // MUDANÇA 3: Só bloqueamos a mudança de estado se tentarem LIGAR a luz durante um flicker.
            // Para desligar, o comando agora passa direto.
            if (state && isFlickering) return;

            flashlightLight.enabled = state;
        }
    }

    // Acionado pelo controller quando a bateria entra no estado crítico
    public void TriggerFlicker()
    {
        if (!isFlickering && flashlightLight.enabled)
        {
            // MUDANÇA 4: Salvamos a coroutine iniciada na variável
            activeFlickerCoroutine = StartCoroutine(FlickerCoroutine());
        }
    }

    private IEnumerator FlickerCoroutine()
    {
        isFlickering = true;

        // Desliga momentaneamente
        flashlightLight.enabled = false;
        yield return new WaitForSeconds(Random.Range(minFlickerDelay, maxFlickerDelay));

        // Liga de volta
        flashlightLight.enabled = true;
        yield return new WaitForSeconds(Random.Range(minFlickerDelay, maxFlickerDelay));

        isFlickering = false;
    }
}