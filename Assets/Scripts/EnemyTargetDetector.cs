using UnityEngine;

public class EnemyTargetDetector : MonoBehaviour
{
    [Header("Configurações de Detecção")]
    [SerializeField] private float raioDeteccao = 15f;
    [Range(0, 360)]
    [SerializeField] private float anguloVisao = 180f;
    [SerializeField] private LayerMask layerDoJogador;
    [SerializeField] private LayerMask layerObstaculos;

    public Transform DetectTarget(Transform cabecaOlhos)
    {
        // 1. Procura por colisores do jogador dentro do raio de detecção
        Collider[] alvosNoRaio = Physics.OverlapSphere(cabecaOlhos.position, raioDeteccao, layerDoJogador);

        if (alvosNoRaio.Length > 0)
        {
            Transform jogador = alvosNoRaio[0].transform;
            Vector3 direcaoParaJogador = (jogador.position - cabecaOlhos.position).normalized;

            // 2. Verifica se o jogador está dentro do ângulo de visão frontal do anjo
            if (Vector3.Angle(cabecaOlhos.forward, direcaoParaJogador) < anguloVisao / 2f)
            {
                float distanciaAteJogador = Vector3.Distance(cabecaOlhos.position, jogador.position);

                // 3. O PULO DO GATO: Checagem de Obstáculos (Linha de Visão)
                // Dispara um raio laser da cabeça do inimigo em direção ao jogador.
                // O raio só se estende até a distância exata onde o jogador está.
                if (Physics.Raycast(cabecaOlhos.position, direcaoParaJogador, distanciaAteJogador, layerObstaculos))
                {
                    // Se o raio colidir com a layerObstaculos no caminho, a visão está bloqueada!
                    return null;
                }

                // Se o raio não encontrou nenhum obstáculo, o anjo consegue ver o jogador livremente
                return jogador;
            }
        }

        // Nenhum jogador detectado no raio ou visível
        return null;
    }

    // Desenha o raio de detecção no Editor para ajudar nos testes
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioDeteccao);
    }
}