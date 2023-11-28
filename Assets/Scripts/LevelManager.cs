using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public float tickPeriod;
    private bool passTick = false;
    private bool timerActive = false;
    public GameObject playerPrefab;
    public GameObject player;
    //lista
    public List<GameObject> enemies;

    public Transform activeSpawn;

    public Transform[] checkpoints;

    public Transform[] lPoints;

    public LifeBarControl lifebar;

    private HeartSystem heartSystem;

    private void Awake() {

        //canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

    }

    private void Start() {

        player = GameObject.Find("Player Renderer 1(Clone)");
        GetPlayer();

    }

    private void FixedUpdate() {
        
        if (player == null) {
            GetPlayer();
        }

        if (!timerActive) {
            StartCoroutine(Tick());
        }

        if (passTick) {
            for (int i = enemies.Count - 1; i >= 0; i--) {
                if (enemies[i] != null) {
                    enemies[i].SendMessageUpwards("PassTick");
                }
            }
            //Debug.Log("tick");
        }



    }

    private void GetPlayer() {

        if (player == null) {
            player = Instantiate(playerPrefab, activeSpawn.transform.position, Quaternion.identity);
            
        }
        

        GameObject[] _points = player.GetComponentInChildren<LangrangeManager>().lPoints;

        for (int i = 0; i < _points.Length; i++) {
            lPoints[i] = _points[i].transform;
        }

        heartSystem = player.GetComponentInChildren<HeartSystem>();

        for (int i = 0; i < lifebar.hearts.Length; i++) {
            heartSystem.coracao[i] = lifebar.hearts[i];
        }

    }


    IEnumerator Tick() {
        timerActive = true;
        yield return new WaitForSeconds(tickPeriod);
        passTick = true;
        yield return new WaitForSeconds(.02f);
        passTick = false;
        timerActive = false;
    }



}
