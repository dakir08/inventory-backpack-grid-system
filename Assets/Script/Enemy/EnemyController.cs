using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int health = 100;
    public GameObject goldPrefab; // Assign a prefab for the gold drop

    private Rigidbody rb;
    private Transform player;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (health > 0 && player != null)
        {
            MoveTowardsPlayer();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(10);
        }
    }

    private void MoveTowardsPlayer()
    {
        // Calculate direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

        // Rotate towards the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        DropGold();
        Destroy(gameObject); // Destroy the enemy object
    }

    private void DropGold()
    {
        if (goldPrefab != null)
        {
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        }
    }
}
