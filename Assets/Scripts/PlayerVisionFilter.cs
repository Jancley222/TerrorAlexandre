using UnityEngine;

public class PlayerVisionFilter : MonoBehaviour
{
    [Header("Configurações de Visão do Player")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private Renderer enemyRenderer;

    [Header("Configurações de Oclusão (SOLID)")]
    // POR QUE ISSO ESTÁ AQUI? Precisamos saber quais layers físicos representam paredes/estruturas 
    // para calcular se a visão do jogador foi interrompida no meio do caminho.
    [SerializeField] private LayerMask layerObstaculos;

   
    // Avalia se o inimigo está sendo observado diretamente pelo jogador, 
    // levando em consideração o campo de visão da câmera e obstáculos físicos.
   
    public bool IsBeingWatched()
    {
        // Validação preventiva para evitar erros de "NullReference" caso o desenvolvedor esqueça de preencher no Inspector.
        if (playerCam == null || enemyRenderer == null) return false;

        
        // PASSO 1: CHECAGEM DE FRUSTUM (O inimigo está na tela do jogador?)
     
        // O QUE FOI FEITO: Calculamos as 6 placas geométricas que formam a "pirâmide de visão" da câmera do jogador.
        // POR QUE FOI FEITO: Antes de gastar processamento com física (Raycast), verificamos se o corpo do anjo 
        // está matematicamente dentro dos limites da tela. Se não estiver na tela, ele com certeza não está sendo visto.
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);
        if (!GeometryUtility.TestPlanesAABB(planes, enemyRenderer.bounds))
        {
            return false;
        }

        
        // PASSO 2: CHECAGEM DE LINHA DE VISÃO (Existe algo tampando a visão do jogador?)
        // O QUE FOI FEITO: Definimos o ponto de origem (câmera do player) e o ponto de destino.
        // NOTA DE PRECISÃO: Usamos 'enemyRenderer.bounds.center' em vez de 'transform.position'.
        // POR QUE ISSO? O pivô de um modelo 3D costuma ficar nos pés. Se mirássemos nos pés, qualquer caixinha no chão 
        // enganaria o código. Mirando no centro geométrico do corpo (peito/barriga), a checagem fica muito mais precisa.
        Vector3 origemDoRaio = playerCam.transform.position;
        Vector3 destinoDoRaio = enemyRenderer.bounds.center;

        Vector3 direcao = (destinoDoRaio - origemDoRaio).normalized;
        float distanciaAteInimigo = Vector3.Distance(origemDoRaio, destinoDoRaio);

        // O QUE FOI FEITO: Disparamos um Raycast (laser invisível) que nasce nos olhos do jogador e vai até o anjo.
        // Passamos a 'layerObstaculos' para que esse laser ignore o próprio jogador ou outros gatilhos, focando apenas em paredes.
        if (Physics.Raycast(origemDoRaio, direcao, out RaycastHit hit, distanciaAteInimigo, layerObstaculos))
        {
            // POR QUE RETORNA FALSE AQUI? Se o Raycast colidiu com algo antes de atingir a distância total do anjo, 
            // significa que uma parede/pilar da layer Obstáculo cruzou a linha de visão do jogador.
            // Logo, o jogador está "olhando na direção do anjo", mas está vendo apenas a parede. O anjo está livre para se mover!
            return false;
        }

        // Se passou pelo teste da tela E nenhum obstáculo físico bloqueou o laser, o jogador está realmente vendo o anjo.
        return true;
    }
}