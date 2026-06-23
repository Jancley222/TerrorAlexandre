using UnityEngine;

public class Door : MonoBehaviour, IInteragivel
{
    private bool aberta = false;

    public void Interagir()
    {
        aberta = !aberta;
        Debug.Log(aberta ? "Porta aberta!" : "Porta fechada!");
        // Aqui você pode colocar animação, som, etc.
    }
}