using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCapture : MonoBehaviour
{
    public EnemyController enemyController; // assegnare EnemyRoot nel Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Se il nemico è stunnato, non catturare
            if (enemyController != null && enemyController.IsStunned())
                return;

            GameManager.Instance.PlayerCaught();
        }
    }
}