using UnityEngine;
using UnityEngine.AI;

public class PatrolSystem : MonoBehaviour
{
    public enum ModoPatrulha { PontoMaisProximo, AleatorioNoNavMesh }

    // Altere o modificador de acesso do campo 'modoAtual' de 'private' para 'public'
    [Header("Configura��es de Patrulha")]
    [SerializeField] public ModoPatrulha modoAtual = ModoPatrulha.PontoMaisProximo;
    [SerializeField] private Transform[] pontosDePatrulha;
    [SerializeField] private float raioBuscaAleatoria = 20f;


    // Fornece o próximo destino com base no modo de patrulha selecionado.
    public Vector3 GetNextPatrolPoint(Vector3 posicaoAtual)
    {
        if (modoAtual == ModoPatrulha.PontoMaisProximo && pontosDePatrulha.Length > 0)
        {
            return ObterPontoMaisProximo(posicaoAtual);
        }

        return ObterPosicaoAleatoriaNavMesh(posicaoAtual);
    }

    private Vector3 ObterPontoMaisProximo(Vector3 posicaoAtual)
    {
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
        // Gera uma direção aleatória multiplicada pelo raio estabelecido
        Vector3 direcaoAleatoria = Random.insideUnitSphere * raioBuscaAleatoria;
        direcaoAleatoria += posicaoAtual;

        // Procura o ponto estático mais próximo na malha do NavMesh
        if (NavMesh.SamplePosition(direcaoAleatoria, out NavMeshHit hit, raioBuscaAleatoria, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return posicaoAtual; // Retorna a própria posição como contingência se falhar
    }
}