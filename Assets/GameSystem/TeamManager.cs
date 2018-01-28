﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TeamManager:MonoBehaviour
{
    public Team[] Teams = new Team[0];

    public void JoinTeam(Player player,Team team)
    {
        player.Team = team;
        team.Players.Add(player);
        team.PlayerUI[team.Players.Count - 1].SetActive(true);
        team.PlayerUI[team.Players.Count - 1].GetComponent<PlayerUI>().player = player;
    }
    public Team JoinTeam(Player player)
    {
        var teamJoin = Teams.OrderBy(team => team.Players.Count).First();
        JoinTeam(player, teamJoin);
        return teamJoin;
    }
}