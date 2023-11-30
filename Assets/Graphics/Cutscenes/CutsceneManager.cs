using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour {
    string[] textos = new string[9];
    public GameObject texto;

    void Start() {
        StartCoroutine(rotina());

        textos[0] = "A humanidade era primitiva e equilibrada. At� o misterioso deus serpente soprar a magia em suas almas...";

        textos[1] = "...mil�nios se passaram. O poderoso imp�rio de Toth se ergueu do sangue de seus escravos...";

        textos[2] = "...por um milagre desconhecido, uma escrava despertou do t�mulo...";

        textos[3] = "...sua fuga desesperada a levou a encontrar um antigo deus ca�do. Aquele que deseja. Aquele que ri e chora. E de l� um pacto foi selado...";

        textos[4] = "...munido de seu poder, a escrava iniciou a guerra pela liberta��o de seu povo...";

        textos[5] = "...tr�s santos empunharam o poder de sua salvadora durante os oitocentos anos de guerra...";

        textos[6] = "...da angustia do conflito, os escravos se consagraram salvadores. Jurando amor eterno ao deus ca�do. Pavimentando as terras de Semihazah...";

        textos[7] = "...mas o progresso de uma sociedade pac�fica fora capaz de criar seu pr�prio destruidor...";

        textos[8] = "...o novo conflito aniquilou as defesas de Semihazah. Fazendo os sobreviventes clamarem ajuda de seu deus.";

        texto = GameObject.Find("texto");
        texto.GetComponent<Text>().text = textos[0];
    }

    //int cont = 0;

    public IEnumerator rotina() {

        for (int i = 0; i < 9; i++) {
            GameObject img = GameObject.Find("img" + i);
            img.GetComponent<RawImage>().enabled = true;
            texto.GetComponent<Text>().text = textos[i];
            if (i < 9) {
                yield return new WaitForSeconds(6);
                img.GetComponent<RawImage>().enabled = false;
            }
        }

        yield return new WaitForSeconds(4);

        SceneManager.LoadScene("level 01");

        //GameObject img = GameObject.Find("img" + cont);
        //img.GetComponent<RawImage>().enabled = true;
        //texto.GetComponent<Text>().text = textos[cont];
        //cont++;
        //if (cont < 9) {
        //    yield return new WaitForSeconds(5);
        //    img.GetComponent <RawImage>().enabled = false;
        //    StartCoroutine(rotina());
        //} else {
        //    SceneManager.LoadScene("level 01");
        //}
    }
}
