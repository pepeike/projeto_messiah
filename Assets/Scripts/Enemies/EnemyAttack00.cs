using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack00 : MonoBehaviour
{

    [SerializeField]
    private int enemyDmg;

    

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.gameObject.CompareTag("Player Hurtbox")) {
            //Debug.Log(collision.gameObject.name);
            collision.gameObject.GetComponent<HeartSystem>().BroadcastMessage("PlayerTakeDamage", enemyDmg);
        }
    }

}
