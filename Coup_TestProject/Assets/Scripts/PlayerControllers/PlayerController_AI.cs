using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_AI : PlayerController
{
    Action challengedAction;
    Action challengedCounteraction;

    //Turn begun
    protected override void ChooseAction(){StartCoroutine("WaitTo_ChooseAction");}
    IEnumerator WaitTo_ChooseAction()
    {
        yield return new WaitForSeconds(1);

        //choose from ActionManager's list
        Action chosen = null;
        if (coins.coins < 10)
        {
            int rand = 0;
            while (chosen == null)
            {
                rand = Random.Range(0, ActionManager.instance.actions.Count);
                if (coins.coins >= ActionManager.instance.actions[rand].minCoinsRequired)
                    chosen = ActionManager.instance.actions[rand];
                yield return null;
            }
        }
        else chosen = ActionManager.instance.GetActionByName("Coup");

        string target = chosen.requiresTarget ? PlayersManager.instance.GetRandomPlayerName(playerName) : "";
        ActionManager.instance.ChooseAction(chosen, playerName, target);
    }

    //Another player has acted
    protected override void BlockOpportunity(Action action)
    {
        challengedAction = action;
        StartCoroutine("WaitTo_BlockOpportunity");
    }
    IEnumerator WaitTo_BlockOpportunity()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3.5f));

        bool canChallange = !TurnManager.instance.currentAction.isUnchallengeable;
        bool canCounteract = TurnManager.instance.currentAction.CounteractableBy.Count > 0;
        int rand = Random.Range(0, 4);

        //chose to Challenge or Counteract
        //send result to TurnManager
        if (canChallange && rand == 0)
            TurnManager.instance.Block_Action(playerName, "Challenge");
        else if (canCounteract && rand == 1)
            TurnManager.instance.Block_Action(playerName, "Counteraction");
        else challengedAction = null;
    }

    //Another player has counteracted
    protected override void BlockOpportunity_Counteraction(Action action)
    {
        challengedCounteraction = action;
        StartCoroutine("WaitTo_BlockOpportunity_Counteraction");
    }
    IEnumerator WaitTo_BlockOpportunity_Counteraction()
    {
        yield return new WaitForSeconds(1);

        //chose to Challenge
        //send result to TurnManager
        if (Random.Range(0, 2) == 0)
            TurnManager.instance.Block_Counteraction(playerName);
        else challengedCounteraction = null;
    }

    //Player's action was challenged
    public override void GetChallenged() { StartCoroutine("WaitTo_ResolveChallenge"); }
    IEnumerator WaitTo_ResolveChallenge()
    {
        yield return new WaitForSeconds(1);
        
        //choose an unrevealed card to reveal
        if (cards.card1.revealed && !cards.card2.revealed)
            RevealAndReport(cards.card2);
        else if (cards.card2.revealed && !cards.card1.revealed)
            RevealAndReport(cards.card1);
        else
        {
            if (Random.Range(0, 2) == 0)
                RevealAndReport(cards.card1);
            else RevealAndReport(cards.card2);
        }

        void RevealAndReport(CardController card)
        {
            card.revealed = true;
            TurnManager.instance.RevealCard(playerName, card.cardName);
        }
    }

    public override void LoseInfluence() { StartCoroutine("WaitTo_LoseInfluence"); }
    IEnumerator WaitTo_LoseInfluence()
    {
        yield return new WaitForSeconds(.5f);

        //if a card was just revealed, choose that card to lose
        if (cards.card1.revealed && cards.card1.active) cards.card1.active = false;
        else if (cards.card2.revealed && cards.card2.active) cards.card2.active = false;
        else
        {
            //is card 1 already out? choose card 2, and vice versa
            if (!cards.card1.active && cards.card2.active)
            {
                cards.card2.active = false;
            }
            else if (!cards.card2.active && cards.card1.active)
            {
                cards.card1.active = false;
            }
            else
            {
                //if both cards are active, randomly pick one
                //make sure chosen card is not a card that could have given the player success
                bool cardChosen = false;
                if (Random.Range(0, 2) == 0)
                {
                    if (challengedAction != null)
                    {
                        if (challengedAction.performableBy != cards.card1.cardName)
                        {
                            cards.card1.active = false;
                            cardChosen = true;
                        }
                    }
                    else if (challengedCounteraction != null)
                    {
                        if (!challengedCounteraction.CounteractableBy.Contains(cards.card1.cardName))
                        {
                            cards.card1.active = false;
                            cardChosen = true;
                        }
                    }
                }
                if (!cardChosen)
                    cards.card2.active = false;
            }
        }
        LostCard();
    }
}
