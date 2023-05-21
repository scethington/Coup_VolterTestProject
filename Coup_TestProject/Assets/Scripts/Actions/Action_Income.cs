using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Income : Action
{
    public override void PerformAction(string performer, string target)
    {
        performerController = PlayersManager.instance.GetPlayerController(performer);
        performerController.coins.UpdateCoins(1);
        base.PerformAction(performer, target);
    }
}
