using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesPanel : MonoBehaviour
{
    public GameObject ControlContent, GameR1Content, GameR2Content, MermaidContent, PayoutContent, MultiplierContent; 
    public GameObject ControlUnsel, GameRUnsel, MermaidUnsel, PayoutUnsel, MultiplierUnsel; 
    public GameObject ControlSel, GameRSel, MermaidSel, PayoutSel, MultiplierSel; 
    public void Control()
    {
        ControlContent.SetActive(true);
        GameR1Content.SetActive(false);
        GameR2Content.SetActive(false);
        MermaidContent.SetActive(false);
        PayoutContent.SetActive(false);
        MultiplierContent.SetActive(false);
        
        ControlSel.SetActive(true);
        GameRSel.SetActive(false);
        MermaidSel.SetActive(false);
        PayoutSel.SetActive(false);
        MultiplierSel.SetActive(false);
        
        ControlUnsel.SetActive(false);
        GameRUnsel.SetActive(true);
        MermaidUnsel.SetActive(true);
        PayoutUnsel.SetActive(true);
        MultiplierUnsel.SetActive(true);
        
    }

    public void GameRules()
    {
        ControlContent.SetActive(false);
        GameR1Content.SetActive(true);
        GameR2Content.SetActive(false);
        MermaidContent.SetActive(false);
        PayoutContent.SetActive(false);
        MultiplierContent.SetActive(false);
        
        ControlSel.SetActive(false);
        GameRSel.SetActive(true);
        MermaidSel.SetActive(false);
        PayoutSel.SetActive(false);
        MultiplierSel.SetActive(false);
        
        ControlUnsel.SetActive(true);
        GameRUnsel.SetActive(false);
        MermaidUnsel.SetActive(true);
        PayoutUnsel.SetActive(true);
        MultiplierUnsel.SetActive(true);
        
    }

     public void GameRulesChange2()
    {
        ControlContent.SetActive(false);
        GameR1Content.SetActive(false);
        GameR2Content.SetActive(true);
        MermaidContent.SetActive(false);
        PayoutContent.SetActive(false);
        MultiplierContent.SetActive(false);
        
        ControlSel.SetActive(false);
        GameRSel.SetActive(true);
        MermaidSel.SetActive(false);
        PayoutSel.SetActive(false);
        MultiplierSel.SetActive(false);
        
        ControlUnsel.SetActive(true);
        GameRUnsel.SetActive(false);
        MermaidUnsel.SetActive(true);
        PayoutUnsel.SetActive(true);
        MultiplierUnsel.SetActive(true);
        
    }

    public void Mermaid()
    {
        ControlContent.SetActive(false);
        GameR1Content.SetActive(false);
        GameR2Content.SetActive(false);
        MermaidContent.SetActive(true);
        PayoutContent.SetActive(false);
        MultiplierContent.SetActive(false);
        
        ControlSel.SetActive(false);
        GameRSel.SetActive(false);
        MermaidSel.SetActive(true);
        PayoutSel.SetActive(false);
        MultiplierSel.SetActive(false);
        
        ControlUnsel.SetActive(true);
        GameRUnsel.SetActive(true);
        MermaidUnsel.SetActive(false);
        PayoutUnsel.SetActive(true);
        MultiplierUnsel.SetActive(true);
        
    }

    public void Payout()
    {
        ControlContent.SetActive(false);
        GameR1Content.SetActive(false);
        GameR2Content.SetActive(false);
        MermaidContent.SetActive(false);
        PayoutContent.SetActive(true);
        MultiplierContent.SetActive(false);
        
        ControlSel.SetActive(false);
        GameRSel.SetActive(false);
        MermaidSel.SetActive(false);
        PayoutSel.SetActive(true);
        MultiplierSel.SetActive(false);
        
        ControlUnsel.SetActive(true);
        GameRUnsel.SetActive(true);
        MermaidUnsel.SetActive(true);
        PayoutUnsel.SetActive(false);
        MultiplierUnsel.SetActive(true);
        
    }

    public void Multiply()
    {
        ControlContent.SetActive(false);
        GameR1Content.SetActive(false);
        GameR2Content.SetActive(false);
        MermaidContent.SetActive(false);
        PayoutContent.SetActive(false);
        MultiplierContent.SetActive(true);
        
        ControlSel.SetActive(false);
        GameRSel.SetActive(false);
        MermaidSel.SetActive(false);
        PayoutSel.SetActive(false);
        MultiplierSel.SetActive(true);
        
        ControlUnsel.SetActive(true);
        GameRUnsel.SetActive(true);
        MermaidUnsel.SetActive(true);
        PayoutUnsel.SetActive(true);
        MultiplierUnsel.SetActive(false);
        
    }

}
