using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Steal : Action
{
    int coinsTaken;
    public override void PerformAction(string performer, string target)
    {
        targetController = PlayersManager.instance.GetPlayerController(target);

        coinsTaken = targetController.coins.coins >= 2 ? 2 : targetController.coins.coins;
        targetController.coins.UpdateCoins(-coinsTaken);

        performerController = PlayersManager.instance.GetPlayerController(performer);
        performerController.coins.UpdateCoins(coinsTaken);
        
        base.PerformAction(performer, target);
    }

    public override void Success()
    {
        base.Success();
        coinsTaken = 0;
    }

    public override void Failed(bool getRefund)
    {
        performerController.coins.UpdateCoins(-coinsTaken);
        targetController.coins.UpdateCoins(coinsTaken);
        base.Failed(getRefund);
        coinsTaken = 0;
    }
}
