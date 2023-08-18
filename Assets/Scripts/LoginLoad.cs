using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginLoad : MonoBehaviour
{
    public LoadingScript instance;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Username"))
        {
            instance.LoadScene("MainScene");
        }
        else
        {
            instance.LoadScene("Login");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
