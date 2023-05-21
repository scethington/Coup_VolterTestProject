using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonController : MonoBehaviour
{
    protected Button button;

    //change button color
    protected virtual void CheckColor(){}
    protected void SetColor(Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = color;
        colors.pressedColor = color;
        colors.selectedColor = color;
        colors.disabledColor = ColorManager.instance.GetColor("Inactive");

        button.colors = colors;
    }

    //conditions for button to be interactive
    protected virtual void CheckState(){}
    protected void SetButtonState(bool state)
    {
        button.interactable = state;
    }

    protected void OnEnable()
    {
        if(!button) button = GetComponent<Button>();
        CheckColor();
        CheckState();
    }
}
