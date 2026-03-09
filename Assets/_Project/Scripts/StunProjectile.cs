using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float stunDuration = 3f;
    public float lifeTime = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("StunProjectile: manca Rigidbody!");
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
        if (rb != null)
            rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.ApplyStun(stunDuration);
        }

        Destroy(gameObject);
    }
}