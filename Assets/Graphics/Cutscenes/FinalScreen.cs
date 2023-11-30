using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalScreen : MonoBehaviour {
    
    public Animator anim;
    public Text text;

    //private 

    void Start() {
        text.enabled = false;
        anim = GameObject.Find("Cortina").GetComponent<Animator>();
        StartCoroutine(rotina());
    }

    public void Update() {

        if (Input.GetKeyDown(KeyCode.E)) {
            End();
        }

    }

    void End() {
        text.enabled=false;
        StartCoroutine(end());
    }

    public IEnumerator rotina() {

        

        yield return new WaitForSeconds(2);
        text.enabled=true;
        

        
    }

    public IEnumerator end() {
        anim.SetTrigger("end");
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("menu upando");
    }

}
