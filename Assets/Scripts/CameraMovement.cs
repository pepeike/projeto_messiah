using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    #region Variables

    public Transform room;

    public LevelManager levelManager;

    private Transform playerToFollow;

    private Camera _cam;

    [SerializeField] private float roomX;
    [SerializeField] private float roomY;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    
    private Vector3 targetPos;

    #endregion


    private void Awake() => _cam = Camera.main;

    private void Start() {

        
            
        
        

        float vertExtent = _cam.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        minX = (horzExtent - roomX / 2) + room.position.x;
        maxX = (roomX / 2 - horzExtent) + room.position.x;
        minY = (vertExtent - roomY / 2) + room.position.y;
        maxY = (roomY / 2 - vertExtent) + room.position.y;

    }





    private void FixedUpdate() {

        

        transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);

    }

    private void LateUpdate() {

        if (playerToFollow == null) {
            StartCoroutine(WaitForPlayer());
        }

        Vector3 v3 = transform.position;
        v3.x = Mathf.Clamp(v3.x, minX, maxX);
        v3.y = Mathf.Clamp(v3.y, minY, maxY);
        v3.z = transform.position.z;
        transform.position = v3;

        if (playerToFollow != null) {
            FollowPlayer();
        }
        //FollowPlayer();



    }

    IEnumerator WaitForPlayer() {
        if (playerToFollow == null) {
            yield return new WaitForSeconds(1);
            playerToFollow = levelManager.player.transform;
        }
    }

    void NewRoom() {
        float vertExtent = _cam.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        minX = (horzExtent - roomX / 2) + room.position.x;
        maxX = (roomX / 2 - horzExtent) + room.position.x;
        minY = (vertExtent - roomY / 2) + room.position.y;
        maxY = (roomY / 2 - vertExtent) + room.position.y;
    }

    void FollowPlayer() {
        targetPos = new Vector2(
            playerToFollow.position.x,
            playerToFollow.position.y
            );
    }


}
