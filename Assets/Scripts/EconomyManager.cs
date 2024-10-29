using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text coinText;
    private int currentCoins = 0;

    public void UpdateCurrentCoins() {
        currentCoins += 1;

        if (coinText == null) {
            coinText = GameObject.Find("Coin Amount Text").GetComponent<TMP_Text>();
        }

        coinText.text = currentCoins.ToString("D3");
    }
}
