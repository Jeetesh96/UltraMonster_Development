using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private Text coinstext;


    // Update is called once per frame
    void Update()
    {
        int coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        coinstext.text = coins.ToString();
    }
}
