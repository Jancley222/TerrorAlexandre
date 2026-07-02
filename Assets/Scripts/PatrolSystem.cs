using UnityEngine;
using UnityEngine.AI;

public class PatrolSystem : MonoBehaviour
{
    public enum ModoPatrulha { PontoMaisProximo, AleatorioNoNavMesh }

    [Header("Configurações de Patrulha")]
    [SerializeField] private ModoPatrulha modoAtual = ModoPatrulha.PontoMaisProximo;
    [SerializeField] private Transform[] pontosDePatrulha;
    [SerializeField] private float raioBuscaAleatoria = 20f;

    // Fornece o próximo destino com base no modo de patrulha selecionado.
    public Vector3 GetNextPatrolPoint(Vector3 posicaoAtual)
    {
        if (modoAtual == ModoPatrulha.PontoMaisProximo && pontosDePatrulha != null && pontosDePatrulha.Length > 0)
        {
            return ObterPontoMaisProximo(posicaoAtual);
        }

        return ObterPosicaoAleatoriaNavMesh(posicaoAtual);
    }

    // Permite que consumidores alterem o modo de patrulha de forma controlada (DIP)
    public void SetModoPatrulha(ModoPatrulha novoModo)
    {
        modoAtual = novoModo;
    }

    // Obtém um ponto aleatório no NavMesh (sem alterar modoInterno)
    public Vector3 GetRandomPatrolPoint(Vector3 posicaoAtual)
    {
        return ObterPosicaoAleatoriaNavMesh(posicaoAtual);
    }

    // Seleciona um "ponto seguro" dentro dos pontos específicos.
    // Estratégia: retorna o ponto de patrulha que estiver mais distante da posição atual (heurística simples).
    public Vector3 GetSafePatrolPoint(Vector3 posicaoAtual)
    {
        if (pontosDePatrulha == null || pontosDePatrulha.Length == 0)
        {
            // fallback para ponto aleatório no NavMesh
            return ObterPosicaoAleatoriaNavMesh(posicaoAtual);
        }

        Transform pontoSeguro = pontosDePatrulha[0];
        float maiorDist = -Mathf.Infinity;

        foreach (Transform p in pontosDePatrulha)
        {
            float d = Vector3.Distance(posicaoAtual, p.position);
            if (d > maiorDist)
            {
                maiorDist = d;
                pontoSeguro = p;
            }
        }

        // Garantir que o ponto retornado esteja no NavMesh (amostra local)
        if (NavMesh.SamplePosition(pontoSeguro.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return pontoSeguro.position;
    }

    private Vector3 ObterPontoMaisProximo(Vector3 posicaoAtual)
    {
        if (pontosDePatrulha == null || pontosDePatrulha.Length == 0) return posicaoAtual;

        Transform pontoMaisProximo = pontosDePatrulha[0];
        float menorDistancia = Mathf.Infinity;

        foreach (Transform ponto in pontosDePatrulha)
        {
            float distancia = Vector3.Distance(posicaoAtual, ponto.position);
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                pontoMaisProximo = ponto;
            }
        }
        return pontoMaisProximo.position;
    }

    private Vector3 ObterPosicaoAleatoriaNavMesh(Vector3 posicaoAtual)
    {
        Vector3 direcaoAleatoria = Random.insideUnitSphere * raioBuscaAleatoria;
        direcaoAleatoria += posicaoAtual;

        if (NavMesh.SamplePosition(direcaoAleatoria, out NavMeshHit hit, raioBuscaAleatoria, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return posicaoAtual;
    }
}