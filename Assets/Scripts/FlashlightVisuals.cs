using System.Collections;
using UnityEngine;

public class FlashlightVisuals : MonoBehaviour
{
    [SerializeField] private Light flashlightLight;
    [Header("Game Juice (Flicker)")]
    [SerializeField] private float minFlickerDelay = 0.05f;
    [SerializeField] private float maxFlickerDelay = 0.2f;

    private bool isFlickering = false;

    public void SetLightState(bool state)
    {
        if (flashlightLight != null && !isFlickering)
        {
            flashlightLight.enabled = state;
        }
    }

    // Acionado pelo controller quando a bateria entra no estado crítico
    public void TriggerFlicker()
    {
        if (!isFlickering && flashlightLight.enabled)
        {
            StartCoroutine(FlickerCoroutine());
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