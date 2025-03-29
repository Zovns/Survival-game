using UnityEngine;
using UnityEngine.AI;

public class SheepAI : DamageableObject {
   
    private float walkSpeed = 150f;
    private float runSpeed = 450f;
    private float chaseRange = 10f;
    private Transform player;
    public Animator animator;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private float timer;
    private bool isScared = false;
    private float rotationSpeed = 5f;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
       
    }

    void FixedUpdate()
    {
        float distance =  Vector3.Distance(player.position, transform.position);
        if (distance > chaseRange)
        {
            
            return;
        }
        Vector3 direction;
        if (!isScared)
        {
            direction = player.position - transform.position;
            direction.y = 0;
            rb.linearVelocity = direction.normalized * walkSpeed * Time.deltaTime;
        }
        else
        {
            direction = transform.position - player.position;
            direction.y = 0;
            rb.linearVelocity = direction.normalized * runSpeed * Time.deltaTime;
        }

        transform.forward = Vector3.Lerp(transform.forward, -direction, Time.deltaTime * rotationSpeed);
      
    }

    public void Scare()
    {
        isScared = true;
        
    }

    /*public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }*/
}
