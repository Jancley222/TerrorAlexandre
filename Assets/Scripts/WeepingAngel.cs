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
        // PROTEÇÃO: Se o jumpscare já ativou, o anjo para de pensar. Fim de jogo.
        if (_estadoAtual == EstadoAI.Jumpscare) return;

        // REGRA DE OURO DO ANJO: Se o jogador estiver olhando, congela imediatamente!
        if (visionFilter.IsBeingWatched())
        {
            agent.speed = 0;
            agent.SetDestination(transform.root.position);
            return;
        }

        // Se o jogador piscou ou desviou o olhar, o anjo retoma sua velocidade normal
        agent.speed = velocidadeMovimento;

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
        _alvoAtual = targetDetector.DetectTarget(cabecaOlhos);
        if (_alvoAtual != null)
        {
            _pontoDefinido = false;
            MudarEstado(EstadoAI.Perseguindo);
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            Vector3 proximoPonto = patrolSystem.GetNextPatrolPoint(transform.position);
            agent.SetDestination(proximoPonto);
        }
    }

    private void ExecutarPerseguicao()
    {
        // 1. PRIMEIRO: Verifica a distância para atacar. 
        // Usamos uma distância em "2D" (ignorando o Y) para que a altura do anjo não atrapalhe o cálculo.
        float distanciaAteAlvo = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(_alvoAtual.position.x, 0, _alvoAtual.position.z)
        );

        // Se estiver perto o suficiente, dá o bote imediatamente!
        if (distanciaAteAlvo <= distanciaAtaque)
        {
            MudarEstado(EstadoAI.Jumpscare);
            return; // Importante: Dá o return para não continuar executando o código abaixo
        }

        // 2. SÓ DEPOIS: Checa se o jogador fugiu da visão
        Transform alvoVisto = targetDetector.DetectTarget(cabecaOlhos);

        if (alvoVisto == null)
        {
            MudarEstado(EstadoAI.Patrulhando);
            Vector3 pontoPosPerseguicao = patrolSystem.GetNextPatrolPoint(transform.position);
            agent.SetDestination(pontoPosPerseguicao);
            return;
        }

        agent.SetDestination(_alvoAtual.position);
    }

    private void MudarEstado(EstadoAI novoEstado)
    {
        // Evita que o mesmo estado seja chamado duas vezes seguidas
        if (_estadoAtual == novoEstado) return;

        _estadoAtual = novoEstado;

        if (_estadoAtual == EstadoAI.Jumpscare)
        {
            StartCoroutine(RotinaMorte());
        }
    }

    private IEnumerator RotinaMorte()
    {
        // Trava o NavMesh para ele não deslizar durante o susto
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // Troca as câmeras
        if (_alvoAtual != null) _alvoAtual.gameObject.SetActive(false);
        if (jumpscareCam != null) jumpscareCam.gameObject.SetActive(true);

        yield return new WaitForSeconds(tempoJumpscare);
        SceneManager.LoadScene(cenaMorte);
    }
}