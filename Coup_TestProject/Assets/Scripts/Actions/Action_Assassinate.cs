using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Assassinate : Action
{
    public override void PerformAction(string performer, string target)
    {
        targetController = PlayersManager.instance.GetPlayerController(target);
        performerController = PlayersManager.instance.GetPlayerController(performer);
        performerController.coins.UpdateCoins(-3);
        base.PerformAction(performer, target);
    }

    public override void Success()
    {
        if(!targetController.lostGame)targetController.LoseInfluence();
        base.Success();
    }

    public override void Failed(bool getRefund)
    {
        if (getRefund) performerController.coins.UpdateCoins(3);
        base.Failed(getRefund);
    }
}
