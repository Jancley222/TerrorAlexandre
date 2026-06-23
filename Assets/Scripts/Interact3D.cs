using UnityEngine;

public class Interact3D : MonoBehaviour
{
    [Header("Configurações de interação")]
    public string botãoDeInteracao = "Interact"; // Configura nossa tecla para interagir
    public float DistanciaDeInteracao = 3f; // Configura a distancia maxima que o player consegue interagir com qualquer objeto
    public LayerMask ObjetoInteragivel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Nenhuma câmera principal encontrada! Marque sua câmera como MainCamera.");
        }
    }

    void Update()
    {
        // Detecta se o botão foi pressionado
        if (Input.GetButtonDown(botãoDeInteracao))
        {
            TentarInteragir();
        }
    }

    void TentarInteragir()
    {
        // Raycast a partir do centro da tela
        Ray raio = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(raio, out hit, DistanciaDeInteracao, ObjetoInteragivel))
        {
            // Tenta encontrar um script de interação no objeto
            IInteragivel interagivel = hit.collider.GetComponent<IInteragivel>();
            if (interagivel != null)
            {
                interagivel.Interagir();
            }
            else
            {
                Debug.Log("Objeto atingido não possui script de interação.");
            }
        }
        else
        {
            Debug.Log("Nenhum objeto interagível na frente.");
        }
    }
}

// Interface para objetos interagíveis
public interface IInteragivel
{
    void Interagir();
}
