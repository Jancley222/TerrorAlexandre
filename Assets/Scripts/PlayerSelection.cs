using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    [Header("Configurações de Seleção")]
    [SerializeField] private float maxDistance = 5f; // Distância máxima que a mira alcança
    [SerializeField] private LayerMask ObjetoDeInteracao; // Opcional: Camada dos objetos interativos

    private Outline currentOutline;

    void Update()
    {
        SelectionProcess();
    }

    void SelectionProcess()
    {
        // Cria um raio a partir do centro da tela (onde fica a retícula)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Executa o Raycast
        if (Physics.Raycast(ray, out hit, maxDistance, ObjetoDeInteracao))
        {
            // Tenta pegar o componente Outline no objeto atingido (ou em seus pais/filhos)
            Outline outline = hit.collider.GetComponent<Outline>();

            if (outline != null)
            {
                // Se olhamos para um objeto NOVO com outline
                if (outline != currentOutline)
                {
                    // Desativa o outline do objeto anterior (se houver)
                    if (currentOutline != null)
                    {
                        currentOutline.enabled = false;
                    }

                    // Ativa o outline do novo objeto
                    currentOutline = outline;
                    currentOutline.enabled = true;
                }
            }
            else
            {
                // Se o objeto atingido não tem Outline, desativa o anterior
                ClearSelection();
            }
        }
        else
        {
            // Se o raio não atingiu nada, desativa o anterior
            ClearSelection();
        }
    }

    void ClearSelection()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }
}