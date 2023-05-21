using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ButtonController_Block : UI_ButtonController
{
    [SerializeField] private string blockType;

    protected override void CheckState()
    {
        if(blockType == "Challenge")
        {
            bool canChallange = !TurnManager.instance.currentAction.isUnchallengeable;
            SetButtonState(canChallange);
        }
        else if(blockType == "Counteraction")
        {
            bool canCounteract = TurnManager.instance.currentAction.CounteractableBy.Count > 0;
            SetButtonState(canCounteract);
        }
    }
}
