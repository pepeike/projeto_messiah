using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack01 : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D collision) {

        Vector2 collisionPoint = collision.ClosestPoint(transform.position);

        if (collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.BroadcastMessage("Damage", 2);
        }





        //Debug.Log(collisionPoint);
    }
}
