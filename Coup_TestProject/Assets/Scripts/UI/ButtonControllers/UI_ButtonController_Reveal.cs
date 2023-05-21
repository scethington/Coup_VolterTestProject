using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ButtonController_Reveal : UI_ButtonController
{
    [SerializeField] protected CardController card;

    protected override void CheckState()
    {
        SetButtonState(card.active);
    }
}
