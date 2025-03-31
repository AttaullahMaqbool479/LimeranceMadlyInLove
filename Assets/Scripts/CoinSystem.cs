using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinSystem : MonoBehaviour
{
    public Text coinText;
    private int coins;
    
    void Start()
    {
        coins = 0;
    }

    public void AddCoin()
    {
        coins++;
        UpdateCoins();
    }

    public void UpdateCoins()
    {
        coinText.text = coins.ToString();
    }
}
