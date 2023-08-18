using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ProfileData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playername;
    [SerializeField] private TextMeshProUGUI playercoins;

    private void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        CheckData();
    }
    void CheckData()
    {
        playername.text = PlayerPrefs.GetString("Username", "No name");
        int coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        Debug.Log("Coins: " + coins);
        playercoins.text = coins.ToString();
    }
}
