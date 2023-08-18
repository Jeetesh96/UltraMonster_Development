using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelScript : MonoBehaviour
{
    public static PanelScript Instance;
    public GameObject LobbyPanel;
    public GameObject gameIconPanel;
    public GameObject slotMenuPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void LobbyPanel_activate()
    {
        LobbyPanel.SetActive(true);
    }

    public void GameIcon_Panel()
    {
        gameIconPanel.SetActive(true);
        LobbyPanel.SetActive(false);
    }
    public void OFFGameIConPanel()
    {
        gameIconPanel.SetActive(false);
        LobbyPanel.SetActive(false);
        slotMenuPanel.SetActive(false);

    }

    public void BackFromGameMode()
    {
        LobbyPanel.SetActive(true);
        gameIconPanel.SetActive(false);
        slotMenuPanel.SetActive(false);
    }

    public void SlotMenuPanel()
    {
        slotMenuPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        gameIconPanel.SetActive(false);
    }


}
