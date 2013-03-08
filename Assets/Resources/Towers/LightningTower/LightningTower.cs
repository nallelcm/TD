using UnityEngine;
using System.Collections;

public class LightningTower : Tower {
    /*void FixedUpdate()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, Range);
        int minimumWaypoint = int.MaxValue;
        GameObject targetEnemy = null;
        foreach (Collider enemyCollider in enemiesInRange)
        {
            GameObject enemy = enemyCollider.gameObject;
            if (enemy.tag == "Enemy")
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript.currentWaypoint < minimumWaypoint)
                {
                    targetEnemy = enemy;
                    minimumWaypoint = enemyScript.currentWaypoint;
                }

            }
        }
        if ((Time.time >= nextAttack) && (targetEnemy != null) && (transform.childCount < 1))
        {
            Shoot(targetEnemy);
        }

    }*/
}
