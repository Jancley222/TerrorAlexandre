using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableDoor : MonoBehaviour, IInteractable
{
    private bool _estaAberta = false;

    public void Interact()
    {
        AlternarPorta();
    }

    private void AlternarPorta()
    {
        _estaAberta = !_estaAberta;

        if (_estaAberta)
        {
            Debug.Log("[Cenário] A porta se abriu.");
            // Aplique sua rotação, animação ou transição de posição aqui
        }
        else
        {
            Debug.Log("[Cenário] A porta se fechou.");
        }
    }
}