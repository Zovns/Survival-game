using UnityEngine;

public class DogAI_RigidbodyOnly : MonoBehaviour
{
    private float damage = 5f;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 3f;
    public float attackCooldown = 1.5f;

    public Transform player;
    private PlayerController playerController;
    private Rigidbody rb;
    private float lastAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = player.gameObject.GetComponent<PlayerController>();
       // GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
       // if (playerObj != null)
           // player = playerObj.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange && distance > attackRange)
        {
            ChasePlayer();
        }
        else if (distance <= attackRange)
        {
            TryAttackPlayer();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep on ground level

        Vector3 targetPos = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);

        // Smoothly face the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
    }

    void TryAttackPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        playerController.TakeDamge(damage);
        rb.AddForce(player.position - transform.position + new Vector3(0,2,0),ForceMode.Impulse);
        
        Debug.Log("Dog attacked the player!");

        // Damage logic goes here
        // TODO: DamagePlayer();
    }
}
