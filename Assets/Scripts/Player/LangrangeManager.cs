using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangrangeManager : MonoBehaviour
{

    public GameObject[] lPoints;

    

    private void Start() {
        
        foreach (GameObject lPoint in lPoints) {
            float _randX = Random.Range(-.4f, .4f);
            float _randY = Random.Range(-.4f, .4f);
            lPoint.transform.position = new Vector2(lPoint.transform.position.x + _randX, lPoint.transform.position.y + _randY);
        }

    }

}
