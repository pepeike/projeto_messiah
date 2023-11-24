using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack02 : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision) {

        Vector2 collisionPoint = collision.ClosestPoint(transform.position);

        if (collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.BroadcastMessage("Damage", 4);
        }




        //Debug.Log(collisionPoint);
    }
}
