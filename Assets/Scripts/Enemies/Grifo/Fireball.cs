using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    #region Variables

    public GameObject target;

    private Vector3 playerPos;
    private Vector3 targetDir;
    

    private Rigidbody2D rb;

    [SerializeField] private float speed;
    [SerializeField] private float dmg;



    #endregion

    private void Start() {
        target = GameObject.FindGameObjectWithTag("Player");
        playerPos = target.transform.position;
        rb = GetComponent<Rigidbody2D>();
        targetDir = (playerPos - transform.position).normalized;
        
        StartCoroutine(DespawnTimer());
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(targetDir.x, targetDir.y) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player Hurtbox")) {
            collision.gameObject.GetComponent<HeartSystem>().BroadcastMessage("PlayerTakeDamage", dmg);
            Destroy(transform.gameObject);
        }
    }

    IEnumerator DespawnTimer() {
        yield return new WaitForSeconds(2);
        Destroy(transform.gameObject);
    }

}
