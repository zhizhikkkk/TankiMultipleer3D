using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Tank").transform;
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("Нет NavMeshAgent у " + gameObject.name);
            return;
        }

        agent.speed = 3.5f;
    }

    void Update()
    {
        if (player)
        {
            agent.SetDestination(player.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        TankHealth tankHealth = other.GetComponentInParent<TankHealth>();
        if (tankHealth != null)
        {
            tankHealth.TakeDamage(damage);
            Transform rootObject = transform.root;
            Destroy(rootObject.gameObject);
            
        }
    }
}
