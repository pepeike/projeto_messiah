using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public float tickPeriod;
    private bool passTick = false;
    private bool timerActive = false;
    public GameObject[] enemies;

    private void FixedUpdate() {

        if (!timerActive) {
            StartCoroutine(Tick());
        }

        if (passTick) {
            for (int i = enemies.Length - 1; i >= 0; i--) {
                enemies[i].SendMessageUpwards("PassTick");
            }
            //Debug.Log("tick");
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
