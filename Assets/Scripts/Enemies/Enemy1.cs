using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{

    public int EnemyHP;

    private enum EnemyState {
        Normal,
        Wounded,

    }

    private void Awake() {
        EnemyHP = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
