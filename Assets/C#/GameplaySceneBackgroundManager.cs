using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySceneBackgroundManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> backgrounds = new List<GameObject>();
    [SerializeField] private GameObject transitionObject;

    private void Start()
    {
        StartCoroutine(SetRandomBackground());
    }

    IEnumerator SetRandomBackground()
    {
        
        yield return new WaitUntil(() => GameManager.Instance.gameStarted);

        //float t = 0;
        while(true)
        {
            if (GameManager.Instance.gameEnded)
                break;

            //Timer.Timer t = new Timer.Timer(20);

            yield return new WaitForSeconds(60);

            transitionObject.SetActive(true);

            for(int i = 0; i < backgrounds.Count; i++)
            {
                if(backgrounds[i].activeInHierarchy)
                {
                    Debug.Log(i);
                    backgrounds[i].SetActive(false);
                    if (i == backgrounds.Count - 1)
                    {
                        Debug.Log("In if");
                        backgrounds[0].SetActive(true);
                    }
                    else
                    {
                        Debug.Log("In else");
                        backgrounds[i + 1].SetActive(true);
                    }
                    break;
                }
            }
            yield return null;
        }
    }


}
