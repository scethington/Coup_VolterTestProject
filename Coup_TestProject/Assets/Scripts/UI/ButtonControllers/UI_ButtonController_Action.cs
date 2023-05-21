using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ButtonController_Action : UI_ButtonController
{
    [SerializeField] protected string actionColorName;
    [SerializeField] protected int coinMin;
    [SerializeField] protected int coinMax;

    protected override void CheckColor()
    {
        SetColor(ColorManager.instance.GetColor(actionColorName));
    }

    protected override void CheckState()
    {
        PlayerController currentPlayer = PlayersManager.instance.GetPlayerController(UI_ActionMenu.instance.currentPlayer);
        int currentCoins = currentPlayer.coins.coins;
        SetButtonState(currentCoins >= coinMin && currentCoins <= coinMax);
    }
}
