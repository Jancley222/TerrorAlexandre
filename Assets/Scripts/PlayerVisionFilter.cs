// PlayerVisionFilter.cs
using UnityEngine;

public class PlayerVisionFilter : MonoBehaviour
{
    [Header("Configurações de Visão")]
    [Tooltip("Câmera principal do jogador que observa o anjo")]
    [SerializeField] private Camera playerCam;

    [Tooltip("O Renderer do próprio anjo para calcular os limites de renderização")]
    [SerializeField] private Renderer enemyRenderer;

    private void Awake()
    {
        // Busca automática caso não seja arrastado no Inspetor
        if (playerCam == null) playerCam = Camera.main;
        if (enemyRenderer == null) enemyRenderer = GetComponentInChildren<Renderer>();
    }

 
    // Retorna verdadeiro se o Anjo estiver dentro do Frustum de projeção da câmera do jogador.
    public bool IsBeingWatched()
    {
        if (playerCam == null || enemyRenderer == null) return false;

        // Calcula os planos de visão da câmera do jogador
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);

        // Testa se os limites (Bounds) do modelo estão cruzando esses planos
        return GeometryUtility.TestPlanesAABB(planes, enemyRenderer.bounds);
    }
}