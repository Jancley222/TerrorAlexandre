using UnityEngine;


[RequireComponent(typeof(Collider))]
public class InteractableDoor : MonoBehaviour, IInteractable
{
    [Header("Configurações da Porta")]
    [SerializeField] private bool estaAberta = false;

    // Você pode substituir isso por um disparo de componente Animator futuramente!
    [SerializeField] private Vector3 rotacaoAberta = new Vector3(0, 90, 0);
    [SerializeField] private Vector3 rotacaoFechada = Vector3.zero;
    [SerializeField] private float velocidadeAbertura = 5f;

    private Vector3 _rotacaoAlvo;

    private void Start()
    {
        // Define o estado inicial da rotação baseado na variável booleana
        _rotacaoAlvo = estaAberta ? rotacaoAberta : vecRotacaoFechada();
    }

    private void Update()
    {
        // Suaviza o movimento de rotação da porta em direção ao alvo
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(_rotacaoAlvo), Time.deltaTime * velocidadeAbertura);
    }

    public void Interact()
    {
        estaAberta = !estaAberta; // Inverte o estado da porta
        _rotacaoAlvo = estaAberta ? rotacaoAberta : rotacaoFechada;

        Debug.Log($"[Porta] Nova rotação definida. Status Aberta: {estaAberta}");
    }

    private Vector3 vecRotacaoFechada() => rotacaoFechada;
}