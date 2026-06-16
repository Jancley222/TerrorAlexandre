using UnityEngine;
using UnityEngine.AI;
public class NPCchase : MonoBehaviour
{
    public Transform player; //o player é a referencia
    private NavMeshAgent _agent; //referencia o NavMesh

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //pega o NavMeshAgent
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //seta o destino do agente ao player
            _agent.SetDestination(player.position);
        }
    }
}
