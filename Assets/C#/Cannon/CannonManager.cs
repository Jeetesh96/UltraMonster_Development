using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> canons = new List<GameObject>();

    public void ManageBetweenPlayerAndBot(GameObject playerCannon)
    {
        foreach(GameObject g in canons)
        {
            if(g == playerCannon)
            {
                g.GetComponent<CanonBot>().enabled = false;
            }
            else
            {
                g.GetComponent<CanonBot>().enabled = true;
            }
        }
    }
}
