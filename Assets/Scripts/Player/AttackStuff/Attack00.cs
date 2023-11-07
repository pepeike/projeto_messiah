using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack00 : MonoBehaviour
{

    //public ContactFilter2D contactFilter;

    private void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        Vector2 collisionPoint = collision.ClosestPoint(transform.position);

        if (collision.gameObject.CompareTag("Enemy")) {

        }

        


        //Debug.Log(collisionPoint);
    }

    



}
