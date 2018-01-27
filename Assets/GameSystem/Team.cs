using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Team:MonoBehaviour
{
    public List<Player> Players = new List<Player>();
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
}