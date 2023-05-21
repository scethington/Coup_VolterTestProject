using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public delegate void TurnEvent(string player);
    public static TurnEvent NextTurn, ChallengeIssued, ActionPerformed,
        CounteractionIssued, PlayerLied, PlayerConceeded, PlayerBlocked, PlayerSucceeded, CardReveal;

    public delegate void TurnActionEvent(string player, Action action);
    public static TurnActionEvent BlockableAction, BlockableCounteraction;

    public float totalBlockTime;
    public float currentBlockTime;

    int currentPlayerTurn = -1;
    int phaseNum;

    public PlayerController currentPlayer { get; private set; }
    public Action currentAction { get; private set; }

    public PlayerController currentChallenger { get; private set; }
    public PlayerController currentCounteractor { get; private set; }

    bool actionBlockAttempted;
    bool counteractionBlockAttempted;
    bool gameOver;
    bool waitingForInfluenceLoss;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        NextPlayerTurn();
    }

    //phase 1///////////////////////////////////////////
    void ActionBegun(Action action, string performer, string target)
    {
        phaseNum = 1;

        ActionPerformed?.Invoke(performer);
        currentAction = action;
        currentPlayer = PlayersManager.instance.GetPlayerController(performer);
        if (action.isUnchallengeable && action.CounteractableBy.Count == 0) ActionSuccess(); ////need time delay here
        else
        {
            BlockableAction?.Invoke(performer, action);
            StartCoroutine("BlockTime");
        }
    }
    ///////////////////////////////////////////

    //phase 2///////////////////////////////////////////
    //Block phase of the action
    public void Block_Action(string blocker, string blockType)
    {
        phaseNum = 2;

        if (actionBlockAttempted) return;
        actionBlockAttempted = true;

        CancelBlockTime();

        if(blockType == "Challenge")
        {
            //reveal a card or conceed

            currentPlayer.GetChallenged();
            currentChallenger = PlayersManager.instance.GetPlayerController(blocker);
            ChallengeIssued?.Invoke(blocker);
        }
        else if(blockType == "Counteraction")
        {
            CounteractionBegun(blocker);
            CounteractionIssued?.Invoke(blocker);
        }
    }
    ///////////////////////////////////////////

    //phase 3///////////////////////////////////////////
    void CounteractionBegun(string blocker)
    {
        currentChallenger = null;
        phaseNum = 3;
        
        currentCounteractor = PlayersManager.instance.GetPlayerController(blocker);

        BlockableCounteraction?.Invoke(blocker, currentAction);
        StartCoroutine("BlockTime");
    }

    void CounteractionSuccess()
    {
        PlayerBlocked?.Invoke(currentPlayer.playerName);
        ActionFailed(false);
    }
    ///////////////////////////////////////////

    //phase 4///////////////////////////////////////////
    //Block phase of the counteraction
    public void Block_Counteraction(string blocker)
    {
        phaseNum = 4;

        if (counteractionBlockAttempted) return;
        counteractionBlockAttempted = true;

        CancelBlockTime();

        currentCounteractor.GetChallenged();
        currentChallenger = PlayersManager.instance.GetPlayerController(blocker);
        ChallengeIssued?.Invoke(blocker);
    }
    ///////////////////////////////////////////


    //None phase specific stuff//////////////////////////////
    void ActionSuccess(){StartCoroutine("WaitTo_ActionSuccess");}
    IEnumerator WaitTo_ActionSuccess()
    {
        yield return new WaitForSeconds(.5f);
        currentAction.Success();
        PlayerSucceeded?.Invoke(currentPlayer.playerName);
        //yield return new WaitForSeconds(.5f);
        //NextPlayerTurn();
    }

    void ActionFailed(bool getRefund){StartCoroutine("WaitTo_ActionFailed", getRefund);}
    IEnumerator WaitTo_ActionFailed(bool getRefund)
    {
        yield return new WaitForSeconds(.5f);
        currentAction.Failed(getRefund);
        //yield return new WaitForSeconds(.5f);
        //NextPlayerTurn();
    }

    public void NextPlayerTurn() { StartCoroutine("WaitTo_NextPlayerTurn"); }
    IEnumerator WaitTo_NextPlayerTurn()
    {
        yield return new WaitForSeconds(1);

        currentPlayerTurn++;
        if (currentPlayerTurn >= PlayersManager.instance.players.Length)
            currentPlayerTurn = 0;

        currentChallenger = null;
        currentCounteractor = null;
        actionBlockAttempted = false;
        counteractionBlockAttempted = false;

        if(!gameOver)
            NextTurn?.Invoke(PlayersManager.instance.players[currentPlayerTurn].playerName);
    }

    void CancelBlockTime()
    {
        StopCoroutine("BlockTime");
        currentBlockTime = 0;
    }
    
    IEnumerator BlockTime()
    {
        while(currentBlockTime < totalBlockTime)
        {
            yield return null;
            currentBlockTime += Time.deltaTime;
        }

        currentBlockTime = 0;

        if (phaseNum == 1) ActionSuccess();
        else if(phaseNum == 3) CounteractionSuccess();
    }

    public void RevealCard(string player, string cardName)
    {
        if(phaseNum == 2)
        {
            if (player != currentPlayer.playerName) return;

            if (currentAction.performableBy == cardName)
            {
                CardReveal?.Invoke(player);
                currentChallenger.LoseInfluence();
                waitingForInfluenceLoss = true;
                ActionSuccess();
            }
            else
            {
                PlayerLied?.Invoke(currentPlayer.playerName);
                currentPlayer.LoseInfluence();
                waitingForInfluenceLoss = true;
                ActionFailed(true);
            }
        }
        else if(phaseNum == 4)
        {
            if (player != currentCounteractor.playerName) return;

            if (currentAction.CounteractableBy.Contains(cardName))
            {
                CardReveal?.Invoke(currentCounteractor.playerName);
                PlayerBlocked?.Invoke(currentPlayer.playerName);
                currentChallenger.LoseInfluence();
                waitingForInfluenceLoss = true;
                ActionFailed(false);
            }
            else
            {
                PlayerLied?.Invoke(currentCounteractor.playerName);
                currentCounteractor.LoseInfluence();
                waitingForInfluenceLoss = true;
                ActionSuccess();
            }
        }
    }

    //player has chosen who to lose, and game can continue
    void PlayerLostInfluence(PlayerController player)
    {
        //if (!waitingForInfluenceLoss) return;
        //waitingForInfluenceLoss = false;
        //NextPlayerTurn();
    }

    public void ActionComplete()
    {
        //if(!waitingForInfluenceLoss) NextPlayerTurn();
        NextPlayerTurn();
    }

    void Winner(string playerName)
    {
        gameOver = true;
    }

    private void OnEnable()
    {
        Action.ActionBegun += ActionBegun;
        PlayersManager.PlayerWon += Winner;
        PlayerController.PlayerLostInfluence += PlayerLostInfluence;
    }

    private void OnDisable()
    {
        Action.ActionBegun -= ActionBegun;
        PlayersManager.PlayerWon -= Winner;
        PlayerController.PlayerLostInfluence -= PlayerLostInfluence;
    }
}
