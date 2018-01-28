using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour {
    public Player player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (player)
            transform.Find("Score").GetComponent<TextMeshProUGUI>().text = "Score: " + player.Score;
	}
}
