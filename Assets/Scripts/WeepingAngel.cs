// WeepingAngelBrain.cs
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class WeepingAngel : MonoBehaviour
{
    private enum EstadoAI { Patrulhando, Perseguindo, Jumpscare }

    [Header("Componentes de Dependência")]
    [SerializeField] private Transform cabecaOlhos;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerVisionFilter visionFilter;
    [SerializeField] private EnemyTargetDetector targetDetector;
    [SerializeField] private PatrolSystem patrolSystem;

    [Header("Configurações de Atuação")]
    [SerializeField] private float velocidadeMovimento = 5f;
    [SerializeField] private float distanciaAtaque = 1.5f;
    [SerializeField] private float tempoJumpscare = 2f;
    [SerializeField] private string cenaMorte = "GameOver";
    [SerializeField] private Camera jumpscareCam;

    private EstadoAI _estadoAtual = EstadoAI.Patrulhando;
    private Transform _alvoAtual;
    private bool _pontoDefinido = false;

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        agent.speed = velocidadeMovimento;
        MudarEstado(EstadoAI.Patrulhando);
    }

    private void Update()
    {
        // REGRA DE OURO DO ANJO: Se o jogador estiver olhando, congela imediatamente!
        if (visionFilter.IsBeingWatched())
        {
            agent.speed = 0;
            agent.SetDestination(transform.root.position); // Força parada absoluta de eixos
            return; // Interrompe o Update aqui; a IA não executa nenhuma ação física
        }

        // Se o jogador piscou ou desviou o olhar, o anjo retoma sua velocidade normal
        agent.speed = velocidadeMovimento;

        // Executa a máquina de estados padrão
        ProcessarEstados();
    }

    private void ProcessarEstados()
    {
        switch (_estadoAtual)
        {
            case EstadoAI.Patrulhando:
                ExecutarPatrulha();
                break;

            case EstadoAI.Perseguindo:
                ExecutarPerseguicao();
                break;
        }
    }

    private void ExecutarPatrulha()
    {
        // Se encontrarmos o jogador durante a patrulha, mudamos o estado
        _alvoAtual = targetDetector.DetectTarget(cabecaOlhos);
        if (_alvoAtual != null)
        {
            _pontoDefinido = false;
            MudarEstado(EstadoAI.Perseguindo);
            return;
        }

        // Se o agente chegou perto do destino de patrulha atual, pede um novo ponto
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            Vector3 proximoPonto = patrolSystem.GetNextPatrolPoint(transform.position);
            agent.SetDestination(proximoPonto);
        }
    }

    private void ExecutarPerseguicao()
    {
        // Verifica se o jogador fugiu do campo de visão geral da IA
        _alvoAtual = targetDetector.DetectTarget(cabecaOlhos);

        if (_alvoAtual == null)
        {
            // Perdeu o jogador! Volta a patrulhar calculando o ponto a partir de onde parou
            MudarEstado(EstadoAI.Patrulhando);
            Vector3 pontoPosPerseguicao = patrolSystem.GetNextPatrolPoint(transform.position);
            agent.SetDestination(pontoPosPerseguicao);
            return;
        }

        agent.SetDestination(_alvoAtual.position);

        // Checa proximidade física para matar o jogador
        if (Vector3.Distance(transform.position, _alvoAtual.position) <= distanciaAtaque)
        {
            MudarEstado(EstadoAI.Jumpscare);
        }
    }

    private void MudarEstado(EstadoAI novoEstado)
    {
        _estadoAtual = novoEstado;

        if (_estadoAtual == EstadoAI.Jumpscare)
        {
            StartCoroutine(RotinaMorte());
        }
    }

    private IEnumerator RotinaMorte()
    {
        agent.isStopped = true;

        if (_alvoAtual != null) _alvoAtual.gameObject.SetActive(false);
        if (jumpscareCam != null) jumpscareCam.gameObject.SetActive(true);

        yield return new WaitForSeconds(tempoJumpscare);
        SceneManager.LoadScene(cenaMorte);
    }
}