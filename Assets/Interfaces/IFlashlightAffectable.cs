using UnityEngine;

public interface IFlashlightAffectable
{
    // Interface para garantir que qualquer inimigo possa ser afetado pela lanterna de forma desacoplada.
    void onFlashlightHit(Vector3 flashlightPosition); 
}
