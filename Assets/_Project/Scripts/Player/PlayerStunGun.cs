using UnityEngine;

public class PlayerStunGun : MonoBehaviour
{
    public GameObject stunProjectilePrefab;
    public Transform firePoint;
    public float fireCooldown = 1f;

    private float timer = 0f;

    void Update()
    {
        // Aggiorna il timer
        timer -= Time.deltaTime;

        // Se il player preme R e il cooldown è finito, spara
        if (Input.GetKeyDown(KeyCode.R) && timer <= 0f)
        {
            ShootStun();
            timer = fireCooldown;  // resetta il cooldown
        }
    }

    void ShootStun()
    {
        // Controllo prefab e firePoint
        if (stunProjectilePrefab == null || firePoint == null)
        {
            Debug.LogError("PlayerStunGun: assegna prefab e firePoint!");
            return;
        }

        // Posizione di spawn leggermente davanti al firePoint
        Vector3 spawnPos = firePoint.position + firePoint.forward * 1f;
        GameObject projectile = Instantiate(stunProjectilePrefab, spawnPos, firePoint.rotation);

        // Ignora collisione tra proiettile e player
        Collider projectileCollider = projectile.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>();
        if (projectileCollider != null && playerCollider != null)
            Physics.IgnoreCollision(projectileCollider, playerCollider);
    }
}