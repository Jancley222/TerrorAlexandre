using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Configurações de Raio")]
    [SerializeField] private float distanciaInteracao = 4f; // Distância estilo Minecraft (4 blocos)
    [SerializeField] private LayerMask camadasInterativas;

    [Header("Componentes")]
    [SerializeField] private Camera cameraJogador;

    [Header("Input")]
    [SerializeField] private string botaoInteracao = "Interact";

    private IInteractable _interactavelAtual;
    private Outline _outlineAtual;

    private void Start()
    {
        if (cameraJogador == null)
        {
            cameraJogador = Camera.main;
        }

        // Prende o mouse no centro da tela para jogos em primeira pessoa
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ProcessarMecanicaMira();
        ProcessarInput();
    }

    private void ProcessarMecanicaMira()
    {
        // LÓGICA MINECRAFT: Transforma o centro exato da tela (0.5, 0.5) em um raio físico no mundo 3D
        Ray raioDaMira = cameraJogador.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(raioDaMira, out RaycastHit hit, distanciaInteracao, camadasInterativas))
        {
            IInteractable interactavel = hit.collider.GetComponentInParent<IInteractable>() ?? hit.collider.GetComponent<IInteractable>();

            if (interactavel != null)
            {
                // Se a mira mudou de um objeto para outro
                if (interactavel != _interactavelAtual)
                {
                    DesativarOutlineAnterior();

                    _interactavelAtual = interactavel;
                    _outlineAtual = hit.collider.GetComponentInParent<Outline>() ?? hit.collider.GetComponent<Outline>();

                    if (_outlineAtual != null)
                    {
                        _outlineAtual.enabled = true; // Ativa o contorno do QuickOutline
                    }
                }
                return; // Objeto focado com sucesso, para a execução do método
            }
        }

        // Se a mira saiu de cima do collider, desliga o contorno imediatamente
        DesativarOutlineAnterior();
    }

    private void ProcessarInput()
    {
        if (Input.GetButtonDown(botaoInteracao) && _interactavelAtual != null)
        {
            _interactavelAtual.Interact();

            // Força a atualização da mira caso o objeto olhado tenha sido destruído/coletado
            _interactavelAtual = null;
            _outlineAtual = null;
        }
    }

    private void DesativarOutlineAnterior()
    {
        if (_outlineAtual != null)
        {
            _outlineAtual.enabled = false;
        }

        _interactavelAtual = null;
        _outlineAtual = null;
    }
}