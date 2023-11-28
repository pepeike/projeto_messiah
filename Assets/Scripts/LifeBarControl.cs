using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarControl : MonoBehaviour
{

    #region Variables

    [SerializeField] private Vector2 lifeBarPos;
    Vector3 newPos;
    
    private GameObject[] hearts;

    private HeartSystem heartSystem;

    private Camera cam;

    #endregion

    private void Awake() {
        
        heartSystem = GameObject.Find("Player Renderer").GetComponent<HeartSystem>();
        cam = Camera.main;
        

    }

    private void FixedUpdate() {

        newPos = cam.ViewportToWorldPoint(lifeBarPos);
        newPos.z = transform.position.z;
        transform.position = newPos;

    }

    private void OnDrawGizmos() {

        //Gizmos.DrawWireCube(newPos, new Vector3(2, 1, 0));

    }




}
