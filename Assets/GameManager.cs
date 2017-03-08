using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;

public class GameManager : TrueSyncBehaviour
{
    public List<GameObject> players;
    public List<GameObject> teamOneSpawns;
    public List<GameObject> teamTwoSpawns;

    void Start()
    {
        print(GameObject.FindGameObjectsWithTag("Player").Length);
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }
        ResetSpawns();
    }

    public void ResetSpawns()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (i % 2 == 0)
            {
                players[i].tag = "TeamOne";
                players[i].GetComponent<Renderer>().material.color = Color.blue;
                players[i].transform.position = teamOneSpawns[i].transform.position;
            }
            else
            {
                players[i].tag = "TeamTwo";
                players[i].GetComponent<Renderer>().material.color = Color.red;
                players[i].transform.position = teamTwoSpawns[i - 1].transform.position;
            }
        }
    }
}
