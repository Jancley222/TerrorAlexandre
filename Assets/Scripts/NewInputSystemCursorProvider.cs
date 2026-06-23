// agora nos usamos o novo sistema de inputs da Unity
using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputSystemCursorProvider : MonoBehaviour, ICursorInput
{
    public bool IsUnlockPressed()
    {
        // Retorna verdadeiro apenas no frame em que o Esc foi pressionado
        return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
    }
}