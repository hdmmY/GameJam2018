using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TMPro;

public class Team:MonoBehaviour
{
    public List<Player> Players = new List<Player>();
    public Material[] PlayerSkins = new Material[0];
    public GameObject[] PlayerUI = new GameObject[0];
    public GameObject ScoreBoard;
    
    public string Name;
    public int Score
    {
        get
        {
            var score = 0;
            foreach(var player in Players)
            {
                score += player.Score;
                
            }
            return score;
        }
    }

    public Material AvailableSkin
    {
        get
        {
            return PlayerSkins[(Players.Count - 1) % PlayerSkins.Length];
        }
    }

    public Spawner Spawner;

    private void Update()
    {
        if(ScoreBoard)
            ScoreBoard.GetComponent<TextMeshProUGUI>().text = "Total: " + Score.ToString();
    }
}