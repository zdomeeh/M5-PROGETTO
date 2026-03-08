using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunGun : MonoBehaviour
{
    [Header("Stun Settings")]
    public float stunDuration = 3f;      // Durata dello stun sul nemico
    public float stunRange = 8f;         // Distanza massima dello stun
    public LayerMask enemyMask;          // Layer dei nemici

    [Header("Cooldown")]
    public float fireCooldown = 1f;      // Tempo tra uno stun e l'altro
    private float timer = 0f;

    [Header("References")]
    public Transform firePoint;          // Punto da cui parte il "raggio"

    void Update()
    {
        // Aggiorna cooldown
        timer -= Time.deltaTime;

        // Tasto E per stunnare
        if (Input.GetKeyDown(KeyCode.R) && timer <= 0f)
        {
            ShootStun();
            timer = fireCooldown;
        }
    }

    void ShootStun()
    {
        // Raycast dal FirePoint in avanti
        Ray ray = new Ray(firePoint.position, firePoint.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, stunRange, enemyMask))
        {
            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.ApplyStun(stunDuration);
                Debug.Log("Nemico stunnato: " + enemy.name);
            }
        }

        // Effetto visivo debug (opzionale)
        Debug.DrawRay(firePoint.position, firePoint.forward * stunRange, Color.cyan, 0.5f);
    }
}