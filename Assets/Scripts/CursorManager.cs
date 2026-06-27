// CursorManager.cs
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private ICursorInput _cursorInput;

    private void Awake()
    {
        // SOLID (Inversão de Dependência): Buscamos a abstração.
        _cursorInput = GetComponent<ICursorInput>();

        if (_cursorInput == null)
        {
            Debug.LogError($"[CursorManager] Erro resolvido criando e anexando uma classe concreta (ex: UnityCursorInput) no GameObject '{gameObject.name}'!");
        }
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
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