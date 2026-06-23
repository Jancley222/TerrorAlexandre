using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Configurações de Raio")]
    [SerializeField] private float distanciaInteracao = 3f;
    [SerializeField] private LayerMask camadasInterativas;

    [Header("Input")]
    [Tooltip("Nome exato do botão configurado no Input Manager da Unity (ex: 'Interact')")]
    [SerializeField] private string botaoInteracao = "Interact";

    // Caches para evitar a chamada repetida de GetComponent toda fração de segundo (Otimização)
    private IInteractable _interactavelAtual;
    private Outline _outlineAtual;

    private void Update()
    {
        ProcessarChecagemVisao();
        ProcessarInputInteracao();
    }


    private void ProcessarChecagemVisao()
    {
        Ray raio = new Ray(transform.position, transform.forward);

        // Se o raio atingir algo na camada configurada
        if (Physics.Raycast(raio, out RaycastHit hit, distanciaInteracao, camadasInterativas))
        {
            // Tenta obter o componente interativo através da interface
            IInteractable interactavel = hit.collider.GetComponent<IInteractable>();

            if (interactavel != null)
            {
                // Se mudamos de objeto ou começamos a olhar para um agora
                if (interactavel != _interactavelAtual)
                {
                    DesativarOutlineAtual(); // Limpa o objeto anterior

                    _interactavelAtual = interactavel;

                    // Tenta capturar o componente do pacote 'Quick Outline'
                    _outlineAtual = hit.collider.GetComponent<Outline>();
                    if (_outlineAtual != null)
                    {
                        _outlineAtual.enabled = true; // Liga o contorno visual
                    }
                }
                return; // Mantém o objeto focado, sai do método.
            }
        }

        // Se o raio não atingiu nada válido, limpa o feedback visual
        DesativarOutlineAtual();
    }

    
    // Escuta o comando do teclado/controle para disparar a ação do objeto focado.
    
    private void ProcessarInputInteracao()
    {
        // Solicitação do usuário: Uso estrito de GetButtonDown
        if (Input.GetButtonDown(botaoInteracao) && _interactavelAtual != null)
        {
            // POLIMORFISMO: O interactor não sabe se é porta ou item, ele apenas executa.
            _interactavelAtual.Interact();
        }
    }

 
    // Desliga o componente Quick Outline do objeto que paramos de olhar.
 
    private void DesativarOutlineAtual()
    {
        if (_outlineAtual != null)
        {
            _outlineAtual.enabled = false;
        }

        _interactavelAtual = null;
        _outlineAtual = null;
    }
}