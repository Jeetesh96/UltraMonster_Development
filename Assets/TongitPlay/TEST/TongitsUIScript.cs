using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TongitsUIScript : MonoBehaviour
{
    [SerializeField] private GameObject MenuBtn;
    [SerializeField] private GameObject CloseBtn;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject ChatPanel;
    [SerializeField] private GameObject ProfilePanel;
    [SerializeField] private GameObject StorePanel;
    [SerializeField] private GameObject SlashPackPanel;

    // for open menu
    public void OpenMenu()
    {
        CloseBtn.SetActive(true);
        MenuPanel.SetActive(true);
        MenuBtn.SetActive(false);
    }
    public void CloseMenu()
    {
        CloseBtn.SetActive(false);
        MenuPanel.SetActive(false);
        MenuBtn.SetActive(true);
    }

    // for back button
    public void BackBt()
    {
        CloseBtn.SetActive(false);
        MenuPanel.SetActive(false);
        MenuBtn.SetActive(true);
    }

    // for chat panel
    public void OpenChat()
    {
        ChatPanel.SetActive(true);
    }
    public void CloseChatPanel()
    {
        ChatPanel.SetActive(false);
    }
    
    // for profile panel
    public void ProfileOpen()
    {
        ProfilePanel.SetActive(true);
    }
    public void ProfileClose()
    {
        ProfilePanel.SetActive(false);
    }

    // for storepanel
    public void OpenStore()
    {
        StorePanel.SetActive(true);
        CloseBtn.SetActive(false);
        MenuPanel.SetActive(false);
        MenuBtn.SetActive(false);
    }
    public void CloseStore()
    {
        StorePanel.SetActive(false);
        CloseBtn.SetActive(false);
        MenuPanel.SetActive(false);
        MenuBtn.SetActive(false);
    }
    // SlashPackPanel panel
    public void OpenSlashPackPanel()
    {
        SlashPackPanel.SetActive(true);
    }
    public void CloseSlashPackPanel()
    {
        SlashPackPanel.SetActive(false);
    }
    // Return to main scene
    public void ReturnToMainScene()
    {        
        SceneManager.LoadScene("MainScene");
    }
}
