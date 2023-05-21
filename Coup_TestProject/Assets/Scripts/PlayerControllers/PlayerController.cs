using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerEvent(PlayerController pC);
    public static PlayerEvent PlayerLost, PlayerLostInfluence;

    public string playerName;
    public TextMeshProUGUI nameText;

    public PlayerCoins coins;
    public PlayerCards cards;

    public bool lostGame;

    public Image playerMat;
    public Color playerTurnColor;
    public Color otherTurnColor;

    protected void Start()
    {
        nameText.text = playerName;
    }

    //Turn begun
    //check if player's turn
    protected void BeginTurn(string actingPlayer)
    {
        if (actingPlayer != playerName)
        {
            playerMat.color = otherTurnColor;
            return;
        }
        if (lostGame)
        {
            TurnManager.instance.NextPlayerTurn();
            return;
        }

        playerMat.color = playerTurnColor;
        ChooseAction();
    }

    protected virtual void ChooseAction()
    {
        //choose from ActionManager's list
    }

    //Another player has acted
    //check if player's action, otherwise it is a block opportunity
    protected void ActionPerformed(string actingPlayer, Action action) { if (actingPlayer != playerName && !lostGame) BlockOpportunity(action); }
    protected virtual void BlockOpportunity(Action action)
    {
        //chose to Challenge or Counteract
        //send result to TurnManager
    }

    //Another player has counteracted
    //check if player's counteraction, otherwise it is a block opportunity
    protected void CounteractionPerformed(string actingPlayer, Action action) { if (actingPlayer != playerName && !lostGame) BlockOpportunity_Counteraction(action); }
    protected virtual void BlockOpportunity_Counteraction(Action action)
    {
        //chose to Challenge
        //send result to TurnManager
    }

    //Player's action was challenged
    public virtual void GetChallenged()
    {
        //choose to reveal card or conceed;
    }

    public virtual void LoseInfluence()
    {
        //pick from 2 cards
        
    }

    public void LostCard()
    {
        PlayerLostInfluence?.Invoke(this);
        if (!cards.card1.active && !cards.card2.active)
        {
            lostGame = true;
            PlayerLost(this);
        }
    }

    protected void OnEnable()
    {
        TurnManager.NextTurn += BeginTurn;
        TurnManager.BlockableAction += ActionPerformed;
        TurnManager.BlockableCounteraction += CounteractionPerformed;
    }

    protected void OnDisable()
    {
        TurnManager.NextTurn -= BeginTurn;
        TurnManager.BlockableAction -= ActionPerformed;
        TurnManager.BlockableCounteraction -= CounteractionPerformed;
    }
}
