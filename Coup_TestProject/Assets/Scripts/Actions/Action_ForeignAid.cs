using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_ForeignAid : Action
{
    public override void PerformAction(string performer, string target)
    {
        performerController = PlayersManager.instance.GetPlayerController(performer);
        performerController.coins.UpdateCoins(2);
        base.PerformAction(performer, target);
    }

    public override void Failed(bool getRefund)
    {
        performerController.coins.UpdateCoins(-2);
        base.Failed(getRefund);
    }
}
