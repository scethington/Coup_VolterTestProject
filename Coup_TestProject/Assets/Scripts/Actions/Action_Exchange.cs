using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Exchange : Action
{
    public override void PerformAction(string performer, string target)
    {
        performerController = PlayersManager.instance.GetPlayerController(performer);
        base.PerformAction(performer, target);
    }

    public override void Success()
    {
        CardController card1 = performerController.cards.card1;
        CardController card2 = performerController.cards.card2;

        if (card1.active)
            card1.GetNewCard();

        if (card2.active)
            card2.GetNewCard();

        base.Success();
    }
}
