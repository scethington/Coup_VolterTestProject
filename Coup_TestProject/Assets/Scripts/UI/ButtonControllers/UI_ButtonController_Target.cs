using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ButtonController_Target : UI_ButtonController
{
    [SerializeField] protected PlayerController player;

    protected override void CheckState()
    {
        SetButtonState(!player.lostGame);
    }
}
