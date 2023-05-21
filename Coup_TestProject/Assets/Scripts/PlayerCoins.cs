using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    public delegate void CoinEvent(int count);
    public static CoinEvent CountUpdated;

    public int startCoins = 2;
    public Transform coinParent;
    public GameObject coinObject;

    private int _coins;
    public int coins
    {
        get{return _coins; }
        set{
            _coins = value < 0 ? 0 : value;
            CountUpdated?.Invoke(_coins);
            UpdateList();
        }
    }

    List<GameObject> coinObjects = new List<GameObject>();

    private void Start()
    {
        coins = startCoins;
    }

    public void UpdateCoins(int mod)
    {
        coins += mod;
    }

    void UpdateList()
    {
        for(int i = 0; i < coinObjects.Count;)
        {
            GameObject coin = coinObjects[0];
            coinObjects.Remove(coin);
            Destroy(coin);
        }

        coinObjects = new List<GameObject>();

        for (int j = 0; j < coins; j++)
        {
            coinObjects.Add(Instantiate(coinObject, coinParent));
        }
    }
}
