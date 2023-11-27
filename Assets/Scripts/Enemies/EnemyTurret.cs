using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Enemy2;

public class EnemyTurret : MonoBehaviour
{

    #region Variables

    private Enemy2 enemyMain;

    public GameObject attack;

    private Rigidbody2D rb;

    private Transform target;
    private Transform player;

    private bool isMoving;

    private EnemyState enemyState;

    #endregion

    private void Awake() {
        enemyMain = GetComponentInParent<Enemy2>();
        rb = GetComponent<Rigidbody2D>();



    }

    

}
