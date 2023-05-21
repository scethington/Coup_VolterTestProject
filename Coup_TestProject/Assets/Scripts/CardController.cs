using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardController : MonoBehaviour
{
    public string cardName { get; private set; }
    public string cardEffect { get; private set; }
    public string cardCounteraction { get; private set; }
    public Color cardColor { get; private set; }

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI counteractionText;

    public Image cardImage;

    [SerializeField] bool _shown;
    public bool shown
    {
        set
        {
            _shown = value;

            if (value)
            {
                nameText.text = cardName;
                effectText.text = cardEffect;
                counteractionText.text = cardCounteraction;

                if (active)
                    cardImage.color = cardColor;
            }
        }
        get { return _shown; }
    }

    [SerializeField] bool _revealed;
    public bool revealed
    {
        set
        {
            _revealed = value;

            if (!value)
            {
                if (!shown)
                {
                    nameText.text = "???";
                    effectText.text = "???";
                    counteractionText.text = "???";
                    cardImage.color = ColorManager.instance.GetColor("Unrevealed");
                }
            }
            else 
            {
                nameText.text = cardName;
                effectText.text = cardEffect;
                counteractionText.text = cardCounteraction;

                if (active)
                {
                    cardImage.color = cardColor;
                    StartCoroutine("WaitTo_GetNewCard");
                }
            }
        }
        get { return _revealed; }
    }

    [SerializeField] bool _active;
    public bool active
    {
        set
        {
            _active = value;

            if (!value)
            {
                cardImage.color = ColorManager.instance.GetColor("Inactive");
                StopCoroutine("WaitTo_GetNewCard");
                revealed = true;
            }
            else
            {
                if (!revealed)
                    cardImage.color = ColorManager.instance.GetColor("Unrevealed");
            }
        }
        get { return _active; }
    }

    public void SetCard(CardType newCardType)
    {
        if (newCardType == null) return;
        revealed = false;
        active = true;
        cardName = newCardType.cardName;
        cardEffect = newCardType.cardEffect;
        cardCounteraction = newCardType.cardCounteraction;
        cardColor = newCardType.cardColor;
        shown = shown;
    }

    public void GetNewCard()
    {
        SetCard(CardDealer.instance.GetRandomCard());
    }

    IEnumerator WaitTo_GetNewCard()
    {
        yield return new WaitForSeconds(1);
        GetNewCard();
    }
}
