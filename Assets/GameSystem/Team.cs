using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TMPro;

public class Team:MonoBehaviour
{
    public List<Player> Players = new List<Player>();
    public TextMeshPro ScoreBoard;
    
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

    public Spawner Spawner;

    private void Update()
    {
        ScoreBoard.text = "Total: " + Score.ToString();
    }
}