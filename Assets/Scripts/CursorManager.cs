// CursorManager.cs
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private ICursorInput _cursorInput;

    private void Awake()
    {
        // Inversão de Dependência: Buscamos a abstração, não a classe concreta
        _cursorInput = GetComponent<ICursorInput>();

        if (_cursorInput == null)
        {
            Debug.LogError($"[CursorManager] Falta um componente que implemente ICursorInput no GameObject {gameObject.name}!");
        }
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        // Se a interface disser que foi pressionado, destrancamos
        if (_cursorInput != null && _cursorInput.IsUnlockPressed())
        {
            UnlockCursor();
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}