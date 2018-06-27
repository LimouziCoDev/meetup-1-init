using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiPlayerLife : MonoBehaviour {

    [SerializeField]
    int playerNumber = 1;

    PlayerMove playerMove;
    Text text;

	void Start () {
        playerMove = GameObject.Find("player" + playerNumber).GetComponent<PlayerMove>();
        text = GetComponent<Text>();
	}
	
	void Update () {
        text.text = "x " + playerMove.PlayerLife;
	}
}
