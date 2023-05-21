using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardDealer : MonoBehaviour
{
    public static CardDealer instance;

    public CardType contessa;
    public CardType duke;
    public CardType ambassador;
    public CardType captain;
    public CardType assassin;

    public List<CardType> cardTypes;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cardTypes = new List<CardType> { contessa, duke, ambassador, captain, assassin };

        foreach (PlayerController player in PlayersManager.instance.players)
        {
            player.cards.card1.SetCard(GetRandomCard());
            player.cards.card2.SetCard(GetRandomCard());
        }
    }

    public CardType GetRandomCard()
    {
        return cardTypes[Random.Range(0, cardTypes.Count())];
    }
}
