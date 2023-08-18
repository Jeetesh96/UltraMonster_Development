using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesPooling : ObjectPooling
{
    [SerializeField] List<GameObject> bubbles = new List<GameObject>();
    private void Start()
    {
        StartCoroutine(DynamicPosition());

    }

    IEnumerator DynamicPosition()
    {
        while(true)
        {
            int random = Random.Range(1, 4);
            //Debug.Log("Random " + random);
            for(int j = 0; j < random; j++)
            {
                int i = Random.Range(0, bubbles.Count);
                if (!bubbles[i].activeInHierarchy)
                {
                    bubbles[i].SetActive(true);
                }
                else
                {
                    foreach (GameObject g in bubbles)
                    {
                        if (!g.activeInHierarchy)
                        {
                            g.SetActive(true);
                            break;
                        }
                    }
                }
            }

            yield return new WaitForSeconds(5);
            foreach (GameObject g in bubbles)
            {
                g.SetActive(false);
            }
        }
    }
}
