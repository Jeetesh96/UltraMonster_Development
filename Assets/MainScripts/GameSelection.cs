using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelection : MonoBehaviour
{
    public List<GameObject> PanelToSelect;
    private int currentSGIndex;

    void Start()
    {
       // PlayerPrefs.DeleteAll();
    }
    public void SelectGamesPanel(int index)
    {

        for (int i = 0; i < PanelToSelect.Count; i++)
        {
            if (i == index)
            {
                PanelToSelect[i].SetActive(true);
                currentSGIndex = i;
                PanelScript.Instance.OFFGameIConPanel();
            }
            else
            {
                PanelToSelect[i].SetActive(false);
            }
        }
    }

    public void BackFromGameSelection()
    {
        PanelToSelect[currentSGIndex].SetActive(false);
        PanelScript.Instance.GameIcon_Panel();
    }

    public void LoadScene(string levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }    

    public void clearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
