using UnityEngine;

public class ControleMusicaLeao : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public AudioSource musicaFundo;

    [Header("Configurações")]
    public float distanciaParar = 10f;

    private bool musicaParada = false;

    void Update()
    {
        float distancia = Vector3.Distance(player.position, transform.position);

        if (distancia <= distanciaParar)
        {
            if (!musicaParada)
            {
                musicaFundo.Pause();
                musicaParada = true;
            }
        }
        else
        {
            if (musicaParada)
            {
                musicaFundo.UnPause();
                musicaParada = false;
            }
        }
    }
}