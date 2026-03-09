using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunGun : MonoBehaviour
{
    public GameObject stunProjectilePrefab;
    public Transform firePoint;
    public float fireCooldown = 1f;

    private float timer = 0f;

    void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R) && timer <= 0f)
        {
            ShootStun();
            timer = fireCooldown;
        }
    }

    void ShootStun()
    {
        if (stunProjectilePrefab == null || firePoint == null)
        {
            Debug.LogError("PlayerStunGun: assegna prefab e firePoint!");
            return;
        }

        Vector3 spawnPos = firePoint.position + firePoint.forward * 1f;
        GameObject projectile = Instantiate(stunProjectilePrefab, spawnPos, firePoint.rotation);

        // Ignora collisione con il player
        Collider projectileCollider = projectile.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>();
        if (projectileCollider != null && playerCollider != null)
            Physics.IgnoreCollision(projectileCollider, playerCollider);
    }
}