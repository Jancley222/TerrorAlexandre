using UnityEngine;

using UnityEngine.InputSystem;



public class CameraPauseController : MonoBehaviour

{

    [SerializeField] private GameObject playerFollowCamera;



    void Update()

    {

        if (PauseMenu.isGamePaused)

        {

            //se o jogo for pausado, desativa o objeto que lê o mouse da câmera

            if (playerFollowCamera != null && playerFollowCamera.activeSelf)

            {

                playerFollowCamera.SetActive(false);

            }

        }

        else

        {

            //quando o jogo voltar, a câmera é reativada

            if (playerFollowCamera != null && !playerFollowCamera.activeSelf)

            {

                playerFollowCamera.SetActive(true);

            }

        }

    }

}