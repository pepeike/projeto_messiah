using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static PlayerMain;


public class PlayerAttacks : MonoBehaviour {

    PlayerMain player;
    [SerializeField]
    public GameObject[] attacks;
    private int attackPhase = 0;

    [SerializeField] private Vector3 attackArea00;
    //[SerializeField] private Vector3 attackArea01;
    //[SerializeField] private Vector3 attackArea02;




    public LayerMask enemies;
    public ContactFilter2D enemyFilter;
    //public ContactPoint2D[] playerHits;

    private void Awake() {

        player = GetComponent<PlayerMain>();



        foreach (GameObject attack in attacks) {

            attack.SetActive(false);

        }

    }

    private void FixedUpdate() {

        

    }

    #region ATTACKS

    public IEnumerator Attack00() {
        player.playerState = PlayerState.Attacking;
        attackPhase++;
        yield return new WaitForSeconds(.2f);
        attacks[0].SetActive(true);
        AttackHit(attacks[0]);
        yield return new WaitForSeconds(.4f);
        attacks[0].SetActive(false);
        yield return new WaitForSeconds(.2f);
        player.playerState = PlayerMain.PlayerState.Normal;
        attackPhase = 0;
    }

    public IEnumerator Attack01() {
        player.playerState = PlayerState.Attacking;
        attacks[0].SetActive(false);
        attackPhase++;
        yield return new WaitForSeconds(.1f);
        attacks[1].SetActive(true);
        yield return new WaitForSeconds(.5f);
        attacks[1].SetActive(false);
        yield return new WaitForSeconds(.3f);
        player.playerState = PlayerMain.PlayerState.Normal;
        attackPhase = 0;
    }

    public IEnumerator Attack02() {
        player.playerState = PlayerState.Attacking;
        attacks[1].SetActive(false);
        attackPhase++;
        yield return new WaitForSeconds(.3f);
        attacks[2].SetActive(true);
        yield return new WaitForSeconds(.6f);
        attacks[2].SetActive(false);
        yield return new WaitForSeconds(.5f);
        player.playerState = PlayerMain.PlayerState.Normal;
        attackPhase = 0;
    }

    #endregion


    private void AttackHit(GameObject attack) {
        //RaycastHit2D hit = Physics2D.BoxCast(attack.transform.position, attackArea00, transform.rotation.z, Vector2.right, 0.2f, enemies);
        
        

        //Debug.Log(hit);
    }

    

    void OnFire0() {
        //Debug.Log("Click");
        if (player.playerState != PlayerState.Sprinting) {

            

            if (attackPhase == 0) {
                StartCoroutine(Attack00());
                
                //AttackHit(attacks[0], hitboxes[0]);
                //PlayerAttack(attacks[0].);
            } //else if (attackPhase == 1) {
                //StopCoroutine(Attack00());
                //StopAllCoroutines();
                //StartCoroutine(Attack01());
            //} //else if (attackPhase == 2) {
                //StopCoroutine(Attack01());
                //StopAllCoroutines();
                //StartCoroutine(Attack02());
            //}

        }


        if (attackPhase > 3) { attackPhase = 0; }



    }

}
