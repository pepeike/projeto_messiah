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

    public AudioSource[] musics;
    private int currentMusicIndex;
    public bool changeMusic = false;
    public bool changeMusicSpecific = false;
    public int targetMusicIndex;

    //[SerializeField]
    //private Camera roomCam;

    private Camera cam;
    private CameraMovement camMove;

    private GameObject player;
    private PlayerMain playerScript;

    private void Awake() {
        
        col = GetComponent<BoxCollider2D>();
        cam = Camera.main;
        camMove = cam.GetComponent<CameraMovement>();
        levelManager = FindAnyObjectByType<LevelManager>();
        musics = cam.GetComponents<AudioSource>();
        currentMusicIndex = 0;
    }

    private void Start() {
        player = levelManager.player;
        playerScript = player.GetComponentInChildren<PlayerMain>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, col.size);


    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            camMove.room = gameObject.transform;
            cam.orthographicSize = camSize;
            cam.BroadcastMessage("NewRoom", col.size.y);

            if (enemy1 != null) {
                for (int i = enemy1Spawns.Length - 1; i >= 0; i--) {
                    GameObject enemy = Instantiate(enemy1, enemy1Spawns[i].transform.position, Quaternion.identity, enemy1Spawns[i]);
                    levelManager.enemies.Add(enemy);

                    //enemies.Add(enemy);
                }
            }

            if (enemy2 != null) {
                for (int i = enemy2Spawns.Length - 1; i >= 0; i--) {
                    GameObject enemy = Instantiate(enemy2, enemy2Spawns[i].transform.position, Quaternion.identity, enemy2Spawns[i]);
                    levelManager.enemies.Add(enemy);
                }
            }

            Debug.Log(musics[0].ToString() + " " + musics[1].ToString());

            if (changeMusic) {

                PlayNextSong();
            }

            if (changeMusicSpecific) {
                PlaySong(targetMusicIndex);
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

            if (enemy2Spawns != null) {
                for (int i = enemy2Spawns.Length - 1; i >= 0; --i) {
                    if (enemy2Spawns[i].childCount > 0) {
                        Destroy(enemy2Spawns[i].GetChild(0).gameObject);
                    }
                }
            }

            

            //    roomCam.gameObject.SetActive(false);
            //    Debug.Log("Room Left");
        }
    }

    void PlayNextSong() {
        if (currentMusicIndex < musics.Length - 1) {
            musics[currentMusicIndex].Stop();
            currentMusicIndex++;
            musics[currentMusicIndex].Play();
        }
    }

    void PlaySong(int songIndex) {
        musics[currentMusicIndex].Stop();
        musics[songIndex].Play();
        currentMusicIndex = songIndex;
    }


}
