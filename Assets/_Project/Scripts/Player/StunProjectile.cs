using UnityEngine;

public class StunProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float stunDuration = 3f;
    public float lifeTime = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Prende il Rigidbody
        if (rb == null)
            Debug.LogError("StunProjectile: manca Rigidbody!");
    }

    void Start()
    {
        // Distrugge il proiettile dopo lifeTime secondi
        Destroy(gameObject, lifeTime);

        // Imposta la velocità del proiettile
        if (rb != null)
            rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return; // Ignora il player

        // Controlla se ha colpito un nemico
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            // Applica lo stun al nemico
            enemy.ApplyStun(stunDuration);
        }

        // Distrugge il proiettile al contatto
        Destroy(gameObject);
    }
}