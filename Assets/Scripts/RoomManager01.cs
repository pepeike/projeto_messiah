using UnityEngine;

public class RoomManager01 : MonoBehaviour {

    [SerializeField]
    private BoxCollider2D col;

    [SerializeField]
    private float camSize;

    [SerializeField]
    private GameObject enemy1;
    [SerializeField]
    private GameObject enemy2;

    private LevelManager levelManager;

    public Transform[] enemy1Spawns;
    public Transform[] enemy2Spawns;
    private GameObject[] enemies;

    //[SerializeField]
    //private Camera roomCam;

    private Camera cam;
    private CameraMovement camMove;

    private GameObject player;
    private PlayerMain playerScript;

    private void Awake() {
        col = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerMain>();
        cam = Camera.main;
        camMove = cam.GetComponent<CameraMovement>();
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, col.size);


    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            camMove.room = gameObject.transform;
            cam.orthographicSize = camSize;
            cam.BroadcastMessage("NewRoom");

            if (enemy1 != null) {
                for (int i = enemy1Spawns.Length - 1; i >= 0; i--) {
                    GameObject enemy = Instantiate(enemy1, enemy1Spawns[i]);
                    levelManager.enemies.Add(enemy);

                    //enemies.Add(enemy);
                }
            }

            if (enemy2 != null) {
                for (int i = enemy2Spawns.Length - 1; i >= 0; i--) {
                    Instantiate(enemy2, enemy2Spawns[i]);
                }
            }

            //roomCam.gameObject.SetActive(true);
            //playerScript.cam = roomCam;
            Debug.Log("Room Entered");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {

            if (enemy1Spawns != null) {
                for (int i = enemy1Spawns.Length - 1; i >= 0; --i) {
                    if (enemy1Spawns[i].childCount > 0) {
                        Destroy(enemy1Spawns[i].GetChild(0).gameObject);
                    }
                }
            }


            //    roomCam.gameObject.SetActive(false);
            //    Debug.Log("Room Left");
        }
    }


}
