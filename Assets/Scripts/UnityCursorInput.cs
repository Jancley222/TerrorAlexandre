// UnityCursorInput.cs
using UnityEngine;

public class UnityCursorInput : MonoBehaviour, ICursorInput
{
    // SOLID (Responsabilidade Única): Esta classe cuida apenas de escutar o teclado/mouse.
    public bool IsUnlockPressed()
    {
        // Implementação real do input. Retorna true se apertar a tecla ESC.
        return Input.GetKeyDown(KeyCode.Escape);
    }
}