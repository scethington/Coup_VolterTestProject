using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager instance;

    public delegate void PlayersManagerEvent(string player);
    public static PlayersManagerEvent PlayerWon, PlayerOut;

    public PlayerController[] players;
    List<PlayerController> lostPlayers = new List<PlayerController>();

    private void Awake()
    {
        instance = this;
    }

    public PlayerController GetPlayerController(string playerName)
    {
        foreach(PlayerController pC in players)
        {
            if(pC.playerName == playerName)
                return pC;
        }

        return null;
    }

    public string GetRandomPlayerName(string excluding)
    {
        List<PlayerController> temp = new List<PlayerController>(players);
        foreach(PlayerController pC in players)
        {
            if (pC.lostGame) temp.Remove(pC);
            else
            {
                if (pC.playerName == excluding)
                    temp.Remove(pC);
            }
        }

        int randNum = Random.Range(0, temp.Count);
        string randName = temp[randNum].playerName;

        return randName;
    }

    void PlayerLost(PlayerController playerController)
    {
        lostPlayers.Add(playerController);
        PlayerOut?.Invoke(playerController.playerName);

        if(lostPlayers.Count == players.Length-1)
        {
            foreach(PlayerController pC in players)
            {
                if(!lostPlayers.Contains(pC))
                {
                    PlayerWon?.Invoke(pC.playerName);
                    break;
                }
            }
        }
    }

    private void OnEnable()
    {
        PlayerController.PlayerLost += PlayerLost;
    }

    private void OnDisable()
    {
        PlayerController.PlayerLost -= PlayerLost;
    }
}
