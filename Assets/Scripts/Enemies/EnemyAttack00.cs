using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack00 : MonoBehaviour
{

    [SerializeField]
    private int enemyDmg;

    

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.BroadcastMessage("PlayerTakeDamage", enemyDmg);
        }

    }

}
