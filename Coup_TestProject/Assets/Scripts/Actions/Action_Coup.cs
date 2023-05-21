using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Coup : Action
{
    public override void PerformAction(string performer, string target)
    {
        targetController = PlayersManager.instance.GetPlayerController(target);
        performerController = PlayersManager.instance.GetPlayerController(performer);
        performerController.coins.UpdateCoins(-7);
        targetController.LoseInfluence();
        base.PerformAction(performer, target);
    }
}
