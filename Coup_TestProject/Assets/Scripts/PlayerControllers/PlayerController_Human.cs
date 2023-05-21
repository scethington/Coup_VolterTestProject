using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Human : PlayerController
{
    public delegate void HumanPlayerEvent(string name);
    public static HumanPlayerEvent HumanAction, HumanBlockOpportunity, HumanBlockOpportunity_Counteraction, HumanRevealCard, HumanLoseInfluence;

    Action challengedAction;
    Action challengedCounteraction;

    //Turn begun
    protected override void ChooseAction()
    {
        //choose from ActionManager's list
        HumanAction?.Invoke(playerName);
    }

    //Another player has acted
    protected override void BlockOpportunity(Action action)
    {
        HumanBlockOpportunity?.Invoke(playerName);
    }

    //Another player has counteracted
    protected override void BlockOpportunity_Counteraction(Action action)
    {
        HumanBlockOpportunity_Counteraction?.Invoke(playerName);
    }

    //Player's action was challenged
    public override void GetChallenged()
    {
        HumanRevealCard?.Invoke(playerName);
    }

    public override void LoseInfluence()
    {
        //if a card was just revealed, choose that card to lose
        //HumanLoseInfluence?.Invoke(playerName);



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
