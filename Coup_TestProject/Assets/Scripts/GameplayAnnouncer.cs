using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayAnnouncer : MonoBehaviour
{
    public TextMeshProUGUI announcementText0;
    public TextMeshProUGUI announcementText1;
    public TextMeshProUGUI announcementText2;

    string text0;
    string text2;
    string text1;

    void SetText(string newText)
    {
        text2 = text1;
        text1 = text0;
        text0 = newText;

        announcementText0.text = text0;
        announcementText1.text = text1;
        announcementText2.text = text2;

        print(newText);
    }

    void ActionBegun(Action action, string performer, string target)
    {
        string against = target == "" ? "" : " against " + target;
        SetText(performer + " attempts " + action.actionName + against);
    }

    void Counteraction(string blocker)
    {
        SetText(blocker + " counteracts ");
    }

    void Challenge(string blocker)
    {
        SetText(blocker + " challenges ");
    }

    void Reveal(string performer)
    {
        SetText(performer + " reveals correct card");
    }

    void ActionSuccess(string performer)
    {
        SetText(performer + " succeeds! ");
    }

    void PlayerLied(string performer)
    {
        SetText(performer + " reveals wrong card; Fails Action ");
    }

    void PlayerBlocked(string performer)
    {
        SetText(performer + " blocked; Fails Action ");
    }

    void PlayerLost(string loser)
    {
        SetText(loser + " is out");
    }

    void PlayerWon(string winner)
    {
        SetText(winner + " wins!!!");
    }

    void PlayerLostInluence(PlayerController playerController)
    {
        SetText(playerController.playerName + " lost influence");
    }

    private void OnEnable()
    {
        Action.ActionBegun += ActionBegun;
        TurnManager.PlayerSucceeded += ActionSuccess;
        TurnManager.PlayerLied += PlayerLied;
        TurnManager.PlayerBlocked += PlayerBlocked;
        TurnManager.CounteractionIssued += Counteraction;
        TurnManager.ChallengeIssued += Challenge;
        TurnManager.CardReveal += Reveal;
        PlayersManager.PlayerOut += PlayerLost;
        PlayersManager.PlayerWon += PlayerWon;
        PlayerController.PlayerLostInfluence += PlayerLostInluence;
    }

    private void OnDisable()
    {
        Action.ActionBegun -= ActionBegun;
        TurnManager.PlayerSucceeded -= ActionSuccess;
        TurnManager.PlayerLied -= PlayerLied;
        TurnManager.PlayerBlocked -= PlayerBlocked;
        TurnManager.CounteractionIssued -= Counteraction;
        TurnManager.ChallengeIssued -= Challenge;
        TurnManager.CardReveal -= Reveal;
        PlayersManager.PlayerOut -= PlayerLost;
        PlayersManager.PlayerWon -= PlayerWon;
        PlayerController.PlayerLostInfluence -= PlayerLostInluence;
    }
}
