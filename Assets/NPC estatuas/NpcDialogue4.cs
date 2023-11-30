using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NpcDialogue4 : MonoBehaviour
{
    public string[] dialogueNpc;
    public int dialogueIndex;

    public GameObject dialoguePanel;
    public Text dialogueText;

    public Text nameNpc;
    public Image ImageNpc;
    public Sprite spriteNpc;

    public bool readyToSpeak;
    public bool startDialogue;


    private bool startedDialogue = false;

    void Start() {
        dialoguePanel.SetActive(false);
    }

    
    void Update() {
        if (Input.GetButtonDown("Interage") && readyToSpeak && startedDialogue) {
            if (!startDialogue) {
                //FindObjectOfType<PlayerMain>().speed = 0f;
                //StartDialogue();
            } else if (dialogueText.text == dialogueNpc[dialogueIndex]) {
                NextDialogue();
                
            }
        }
    }
    void NextDialogue() {
        dialogueIndex++;

        if (dialogueIndex < dialogueNpc.Length) {
            StartCoroutine(ShowDialogue());
        } else {
            dialoguePanel.SetActive(false);
            startDialogue = false;
            dialogueIndex = 0;
            FindObjectOfType<PlayerMain>().speed = 5f;
            SceneManager.LoadScene("level 02");
        }
    }
    void StartDialogue() {
        nameNpc.text = "O Deus da Carne";
        ImageNpc.sprite = spriteNpc;
        startDialogue = true;
        dialogueIndex = 0;
        dialoguePanel.SetActive(true);
        StartCoroutine(ShowDialogue());
    }
    IEnumerator ShowDialogue() {
        dialogueText.text = "";
        foreach (char letter in dialogueNpc[dialogueIndex]) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            startedDialogue = true;
            RenderFollow player = collision.gameObject.GetComponent<RenderFollow>();
            player.canMove = false;
           StartDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            readyToSpeak = false;
        }
    }
}
