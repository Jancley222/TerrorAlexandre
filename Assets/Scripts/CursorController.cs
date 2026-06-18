using UnityEngine;

public class CursorController : MonoBehaviour
{
    void Start()
    {
        // Trava o mouse no centro da tela e o esconde assim que o jogo inicia
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //opcional: Se você apertar a tecla esc, o mouse volta a aparecer
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}