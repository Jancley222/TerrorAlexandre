
using UnityEngine;

public class EnemyTargetDetector : MonoBehaviour
{
    [Header("Configurações de Detecção")]
    [SerializeField] private float raioDeteccao = 15f;
    [Range(0, 360)][SerializeField] private float anguloVisao = 120f;
    [SerializeField] private LayerMask layerDoJogador;
    [SerializeField] private LayerMask layerObstaculos;

   
    // Procura pelo jogador dentro do raio e ângulo configurados, checando se há paredes no caminho.
    public Transform DetectTarget(Transform olhosInimigo)
    {
        // Coleta colisores no raio usando OverlapSphere (otimizado para performance)
        Collider[] colliders = Physics.OverlapSphere(olhosInimigo.position, raioDeteccao, layerDoJogador);

        if (colliders.Length > 0)
        {
            Transform potencialAlvo = colliders[0].transform;
            Vector3 direcaoParaAlvo = (potencialAlvo.position - olhosInimigo.position).normalized;

            // Verifica se o alvo está dentro do ângulo de visão frontal da IA
            if (Vector3.Angle(olhosInimigo.forward, direcaoParaAlvo) < (anguloVisao * 0.5f))
            {
                float distanciaParaAlvo = Vector3.Distance(olhosInimigo.position, potencialAlvo.position);

                // Dispara um raio para garantir que não há paredes bloqueando a visão da IA
                if (!Physics.Raycast(olhosInimigo.position, direcaoParaAlvo, distanciaParaAlvo, layerObstaculos))
                {
                    return potencialAlvo; // Jogador detectado com sucesso!
                }
            }
        }

        return null; // Nenhum alvo visível
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha o raio do OverlapSphere no Editor para depuração
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDeteccao);
    }
}