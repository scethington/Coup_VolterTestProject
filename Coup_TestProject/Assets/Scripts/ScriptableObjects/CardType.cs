using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardType", menuName = "ScriptableObjects/CardType_ScriptableObject")]
public class CardType : ScriptableObject
{
    public string cardName;
    public string cardEffect;
    public string cardCounteraction;

    private Color _cardColor;
    [HideInInspector] public Color cardColor
    {
        private set { _cardColor = value; }
        get { return ColorManager.instance.GetColor(cardName); }
    }
}
