using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSystem : MonoBehaviour
{

    [HideInInspector]
    public int vida;

    [SerializeField]
    private int vidaMaxima;

    public Image[] coracao;
    public Sprite cheio;
    public Sprite vazio;

    public RenderFollow player;
    public PlayerMain playerMain;


    private void Awake() {
        vida = vidaMaxima;
        //player = GameObject.Find("Player Renderer").GetComponent<PlayerMain>();
    }

    

   
    void Update()
    {
        HealthLogic();

        if (vida <= 0) { Destroy(GameObject.Find("Player Renderer")); }

    }

    void HealthLogic()
    {
        if (vida > vidaMaxima)
        {
            vida = vidaMaxima;
        }
        for (int i = 0; i < coracao.Length; i++)
        {
            if (i < vida)
            {
                coracao[i].sprite = cheio;
            }
            else
            {
                coracao[i].sprite = vazio;
            }
            if (i < vidaMaxima)
            {
                coracao[i].enabled = true;
            }
            else
            {
                coracao[i].enabled = false;

            }
        }
    }

    void PlayerTakeDamage(int dmg) {
        if (vida > 0) {
            playerMain.playerState = PlayerMain.PlayerState.Wounded;
            vida -= dmg;
        }
    }



}


   