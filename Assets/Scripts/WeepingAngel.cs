using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class WeepingAngel : MonoBehaviour, IFlashlightAffectable
{
    private Vector3 pontoSeguroNavMesh; // Ponto seguro para onde o anjo foge
    private bool fugindoDaLanterna = false;

    [Header("Fuga / Lanterna")]
    [SerializeField] private float distanciaFuga = 10f;
    [SerializeField] private float multiplicadorVelocidadeFuga = 1.2f;
    [SerializeField] private float toleranciaChegada = 0.1f;

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
        if (agent != null) agent.speed = velocidadeMovimento;
        MudarEstado(EstadoAI.Patrulhando);
    }

    // Ao ser atingido pela lanterna: muda para patrulha por pontos específicos e vai até um ponto seguro.
    public void onFlashlightHit(Vector3 flashlightPosition)
    {
        if (agent == null || patrolSystem == null) return;

        // Define modo de patrulha para pontos específicos (encapsulado em PatrolSystem)
        patrolSystem.SetModoPatrulha(PatrolSystem.ModoPatrulha.PontoMaisProximo);

        // Escolhe um ponto "seguro" fornecido pelo PatrolSystem
        pontoSeguroNavMesh = patrolSystem.GetSafePatrolPoint(transform.position);

        // Marca fuga e aplica velocidade aumentada
        fugindoDaLanterna = true;
        _pontoDefinido = true;
        agent.isStopped = false;
        agent.speed = velocidadeMovimento * multiplicadorVelocidadeFuga;
        agent.SetDestination(pontoSeguroNavMesh);

        // Mantém estado de patrulha enquanto foge para o ponto seguro
        MudarEstado(EstadoAI.Patrulhando);
    }

    private void Update()
    {
        if (agent == null) return;

        // Se estiver fugindo da lanterna, verifica se chegou ao ponto seguro
        if (fugindoDaLanterna && _pontoDefinido)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + toleranciaChegada)
            {
                // Ao chegar: volta para modo aleatório e retoma busca aleatória no NavMesh
                fugindoDaLanterna = false;
                _pontoDefinido = false;

                agent.speed = velocidadeMovimento;
                agent.isStopped = false;

                if (patrolSystem != null)
                {
                    patrolSystem.SetModoPatrulha(PatrolSystem.ModoPatrulha.AleatorioNoNavMesh);
                    Vector3 destinoAleatorio = patrolSystem.GetRandomPatrolPoint(transform.position);
                    agent.SetDestination(destinoAleatorio);
                }
            }
            return;
        }

        if (_estadoAtual == EstadoAI.Jumpscare) return;

        if (visionFilter != null && visionFilter.IsBeingWatched())
        {
            if (!agent.isStopped)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
            return;
        }

        agent.isStopped = false;
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
        if (targetDetector == null || cabecaOlhos == null || agent == null) return;

        _alvoAtual = targetDetector.DetectTarget(cabecaOlhos);
        if (_alvoAtual != null)
        {
            _pontoDefinido = false;
            MudarEstado(EstadoAI.Perseguindo);
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            if (patrolSystem != null)
            {
                Vector3 proximoPonto = patrolSystem.GetNextPatrolPoint(transform.position);
                agent.SetDestination(proximoPonto);
            }
        }
    }

    private void ExecutarPerseguicao()
    {
        if (_alvoAtual == null)
        {
            MudarEstado(EstadoAI.Patrulhando);
            if (patrolSystem != null && agent != null)
            {
                Vector3 pontoPosPerseguicao = patrolSystem.GetNextPatrolPoint(transform.position);
                agent.SetDestination(pontoPosPerseguicao);
            }
            return;
        }

        float distanciaAteAlvo = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(_alvoAtual.position.x, 0, _alvoAtual.position.z)
        );

        if (distanciaAteAlvo <= distanciaAtaque)
        {
            MudarEstado(EstadoAI.Jumpscare);
            return;
        }

        Transform alvoVisto = targetDetector != null ? targetDetector.DetectTarget(cabecaOlhos) : null;

        if (alvoVisto == null)
        {
            MudarEstado(EstadoAI.Patrulhando);
            if (patrolSystem != null && agent != null)
            {
                Vector3 pontoPosPerseguicao = patrolSystem.GetNextPatrolPoint(transform.position);
                agent.SetDestination(pontoPosPerseguicao);
            }
            return;
        }

        agent.SetDestination(_alvoAtual.position);
    }

    private void MudarEstado(EstadoAI novoEstado)
    {
        if (_estadoAtual == novoEstado) return;

        _estadoAtual = novoEstado;

        if (_estadoAtual == EstadoAI.Jumpscare)
        {
            StartCoroutine(RotinaMorte());
        }
    }

    private IEnumerator RotinaMorte()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        if (_alvoAtual != null) _alvoAtual.gameObject.SetActive(false);
        if (jumpscareCam != null) jumpscareCam.gameObject.SetActive(true);

        yield return new WaitForSeconds(tempoJumpscare);
        SceneManager.LoadScene(cenaMorte);
    }
}