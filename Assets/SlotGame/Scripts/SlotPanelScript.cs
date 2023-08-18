using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPanelScript : MonoBehaviour
{
    [SerializeField]
    private GameObject MenuOpenBtn;
    [SerializeField]
    private GameObject MenuCloseBtn;
    [SerializeField]
    private GameObject MenuPanel;
    [SerializeField]
    private GameObject BountiquePanel;
    [SerializeField]
    private GameObject PointRankingPanel;
    [SerializeField]
    private GameObject ProfilePanel;
    [SerializeField]
    private GameObject ChatPanel;
    [SerializeField]
    private GameObject StorePanel;
    [SerializeField]
    private GameObject InboxPanel;
    [SerializeField]
    private GameObject RankingPanel;
    [SerializeField]
    private GameObject RankingSubScreenPanel;

    [SerializeField]
    private GameObject SlashPackPanel;
    [SerializeField]
    private GameObject SuperPackagePanel;
    [SerializeField]
    private GameObject SettingPanel;
    [SerializeField]
    private GameObject RechargePanel;

    [SerializeField]
    private GameObject MissionPassPanel;
    [SerializeField]
    private GameObject DailyMissionSubPanel;
    [SerializeField]
    private GameObject PayTablePanel;

    public void OpenMenu()
    {
        MenuPanel.SetActive(true);
        MenuOpenBtn.SetActive(false);
        MenuCloseBtn.SetActive(true);
    }
    public void CloseMenu()
    {
        MenuOpenBtn.SetActive(true);
        MenuPanel.SetActive(false);
        MenuCloseBtn.SetActive(false);
    }
    // for PayTable Panel
    public void OpenPayTable()
    {
        PayTablePanel.SetActive(true);
    }
    public void ClosePayTable()
    {
        PayTablePanel.SetActive(false);
    }

    // for BountiquePanel panel
    public void OpenBountique()
    {
        BountiquePanel.SetActive(true);
    }
    public void CloseBountique()
    {
        BountiquePanel.SetActive(false);
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
    }
    public void CloseStore()
    {
        StorePanel.SetActive(false);
    }
    //for point ranking panel
    public void OpenPointRank()
    {
        PointRankingPanel.SetActive(true);
    }
    public void ClosePointRank()
    {
        PointRankingPanel.SetActive(false);
    }
    // for subranking
    public void OpenRankingSubBtn()
    {
        RankingSubScreenPanel.SetActive(true);
    }
    public void CloseRankingSubBtn()
    {
        RankingSubScreenPanel.SetActive(false);
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
    //for inbox panel
    public void OpenInboxPanel()
    {
        InboxPanel.SetActive(true);
    }
    public void CloseInboxPanel()
    {
        InboxPanel.SetActive(false);
    }
    // for missionpass
    public void OpenMissionPass()
    {
        MissionPassPanel.SetActive(true);
    }
    public void CloseMissionPass()
    {
        MissionPassPanel.SetActive(false);
    }
    //DailyMission sub panel
    public void OpenDailyMission()
    {
        DailyMissionSubPanel.SetActive(true);
    }
    public void CloseDaillyMission()
    {
        DailyMissionSubPanel.SetActive(false);
    }
    // ranking panel
    public void OpenRankingPanel()
    {
        RankingPanel.SetActive(true);
    }
    public void CloseRankingPanel()
    {
        RankingPanel.SetActive(false);
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
    // SuperPackagePanel panel
    public void OpenSuperPackagePanel()
    {
        SuperPackagePanel.SetActive(true);
    }
    public void CloseSuperPackagePanel()
    {
        SuperPackagePanel.SetActive(false);
    }
    // SettingPanel panel
    public void OpenSettingPanel()
    {
        SettingPanel.SetActive(true);
        MenuPanel.SetActive(false);
        MenuPanel.SetActive(false);
    }
    public void CloseSettingPanel()
    {
        SettingPanel.SetActive(false);
    }
    // RechargePanel panel
    public void OpenRechargePanel()
    {
        RechargePanel.SetActive(true);
    }
    public void CloseRechargePanel()
    {
        RechargePanel.SetActive(false);
    }

    [Header("InAPP UI")]
    //InApp Store UI
    [SerializeField]
    private GameObject GoldPackUI;

    [SerializeField]
    private GameObject DiamondPackUI;

    [SerializeField]
    private GameObject PackagePackUI;

    [SerializeField]
    private GameObject GoldPackBtnOn;
    [SerializeField]
    private GameObject GoldPackBtnOff;
    [SerializeField]
    private GameObject DiamondPackBtnOn;
    [SerializeField]
    private GameObject DiamondPackBtnOff;
    [SerializeField]
    private GameObject PackagePackBtnOn;
    [SerializeField]
    private GameObject PackagePackBtnOff;

    public void OpenGoldPack()
    {
        GoldPackUI.SetActive(true);
        DiamondPackUI.SetActive(false);
        PackagePackUI.SetActive(false);

        GoldPackBtnOn.SetActive(true);
        GoldPackBtnOff.SetActive(false);
        DiamondPackBtnOn.SetActive(false);
        DiamondPackBtnOff.SetActive(true);
        PackagePackBtnOn.SetActive(false);
        PackagePackBtnOff.SetActive(true);
    }

    public void OpenDiamondPack()
    {
        GoldPackUI.SetActive(false);
        DiamondPackUI.SetActive(true);
        PackagePackUI.SetActive(false);

        GoldPackBtnOn.SetActive(false);
        GoldPackBtnOff.SetActive(true);
        DiamondPackBtnOn.SetActive(true);
        DiamondPackBtnOff.SetActive(false);
        PackagePackBtnOn.SetActive(false);
        PackagePackBtnOff.SetActive(true);

    }

    public void OpenPackagePack()
    {
        GoldPackUI.SetActive(false);
        DiamondPackUI.SetActive(false);
        PackagePackUI.SetActive(true);

        GoldPackBtnOn.SetActive(false);
        GoldPackBtnOff.SetActive(true);
        DiamondPackBtnOn.SetActive(false);
        DiamondPackBtnOff.SetActive(true);
        PackagePackBtnOn.SetActive(true);
        PackagePackBtnOff.SetActive(false);
    }

    [Header("GoldTableRanks UI")]
    // Gold Table Ranks UI
    [SerializeField]
    private GameObject TongitPackUI;
    [SerializeField]
    private GameObject PusoyPackUI;
    [SerializeField]
    private GameObject PokerPackUI;
    [SerializeField]
    private GameObject SlotPackUI;

    [SerializeField]
    private GameObject TongitBtnOn;
    [SerializeField]
    private GameObject TongitBtnOff;
    [SerializeField]
    private GameObject PusoyBtnOn;
    [SerializeField]
    private GameObject PusoyBtnOff;
    [SerializeField]
    private GameObject PokerBtnOn;
    [SerializeField]
    private GameObject PokerBtnOff;
    [SerializeField]
    private GameObject SlotBtnOn;
    [SerializeField]
    private GameObject SlotBtnOff;

    public void OpenTongit()
    {
        TongitPackUI.SetActive(true);
        PusoyPackUI.SetActive(false);
        PokerPackUI.SetActive(false);
        SlotPackUI.SetActive(false);

        TongitBtnOn.SetActive(true);
        TongitBtnOff.SetActive(false);
        PusoyBtnOn.SetActive(false);
        PusoyBtnOff.SetActive(true);
        PokerBtnOn.SetActive(false);
        PokerBtnOff.SetActive(true);
        SlotBtnOn.SetActive(false);
        SlotBtnOff.SetActive(true);
    }

    public void OpenPusoy()
    {
        TongitPackUI.SetActive(false);
        PusoyPackUI.SetActive(true);
        PokerPackUI.SetActive(false);
        SlotPackUI.SetActive(false);

        TongitBtnOn.SetActive(false);
        TongitBtnOff.SetActive(true);
        PusoyBtnOn.SetActive(true);
        PusoyBtnOff.SetActive(false);
        PokerBtnOn.SetActive(false);
        PokerBtnOff.SetActive(true);
        SlotBtnOn.SetActive(false);
        SlotBtnOff.SetActive(true);
    }

    public void OpenPoker()
    {
        TongitPackUI.SetActive(false);
        PusoyPackUI.SetActive(false);
        PokerPackUI.SetActive(true);
        SlotPackUI.SetActive(false);

        TongitBtnOn.SetActive(false);
        TongitBtnOff.SetActive(true);
        PusoyBtnOn.SetActive(false);
        PusoyBtnOff.SetActive(true);
        PokerBtnOn.SetActive(true);
        PokerBtnOff.SetActive(false);
        SlotBtnOn.SetActive(false);
        SlotBtnOff.SetActive(true);
    }
    public void OpenSlot()
    {
        TongitPackUI.SetActive(false);
        PusoyPackUI.SetActive(false);
        PokerPackUI.SetActive(false);
        SlotPackUI.SetActive(true);

        TongitBtnOn.SetActive(false);
        TongitBtnOff.SetActive(true);
        PusoyBtnOn.SetActive(false);
        PusoyBtnOff.SetActive(true);
        PokerBtnOn.SetActive(false);
        PokerBtnOff.SetActive(true);
        SlotBtnOn.SetActive(true);
        SlotBtnOff.SetActive(false);
    }

    [Header("DailyMissionSubPanel UI")]
    // DailyMissionSubPanel UI
    [SerializeField]
    private GameObject DailyMissionPackUI;
    [SerializeField]
    private GameObject WeeklyMissionPackUI;

    [SerializeField]
    private GameObject DailyMissionBtnOn;
    [SerializeField]
    private GameObject DailyMissionBtnOff;
    [SerializeField]
    private GameObject WeeklyMissionBtnOn;
    [SerializeField]
    private GameObject WeeklyMissionBtnOff;


    public void OpenDailyMissionPack()
    {
        DailyMissionPackUI.SetActive(true);
        WeeklyMissionPackUI.SetActive(false);

        DailyMissionBtnOn.SetActive(false);
        DailyMissionBtnOff.SetActive(true);
        WeeklyMissionBtnOn.SetActive(true);
        WeeklyMissionBtnOff.SetActive(false);

    }
    public void OpenWeeklyMissionPack()
    {
        DailyMissionPackUI.SetActive(false);
        WeeklyMissionPackUI.SetActive(true);

        DailyMissionBtnOn.SetActive(true);
        DailyMissionBtnOff.SetActive(false);
        WeeklyMissionBtnOn.SetActive(false);
        WeeklyMissionBtnOff.SetActive(true);

    }

    [Header("Ranking Activity Panel UI")]
    // Gold Table Ranks UI
    [SerializeField]
    private GameObject TotalGoldPackUI;
    [SerializeField]
    private GameObject StorePtsPackUI;
    [SerializeField]
    private GameObject ShowTimeBadgesPackUI;
    [SerializeField]
    private GameObject BetAmountPackUI;
    [SerializeField]
    private GameObject ProfitPackUI;

    [SerializeField]
    private GameObject TotalGoldBtnOn;
    [SerializeField]
    private GameObject TotalGoldBtnOff;
    [SerializeField]
    private GameObject StorePtsBtnOn;
    [SerializeField]
    private GameObject StorePtsBtnOff;
    [SerializeField]
    private GameObject ShowTimeBadgesBtnOn;
    [SerializeField]
    private GameObject ShowTimeBadgesBtnOff;
    [SerializeField]
    private GameObject BetAmountBtnOn;
    [SerializeField]
    private GameObject BetAmountBtnOff;
    [SerializeField]
    private GameObject ProfitBtnOn;
    [SerializeField]
    private GameObject ProfitBtnOff;

    public void OpenTotalGold()
    {
        TotalGoldPackUI.SetActive(true);
        StorePtsPackUI.SetActive(false);
        ShowTimeBadgesPackUI.SetActive(false);
        BetAmountPackUI.SetActive(false);
        ProfitPackUI.SetActive(false);

        TotalGoldBtnOn.SetActive(true);
        TotalGoldBtnOff.SetActive(false);
        StorePtsBtnOn.SetActive(false);
        StorePtsBtnOff.SetActive(true);
        ShowTimeBadgesBtnOn.SetActive(false);
        ShowTimeBadgesBtnOff.SetActive(true);
        BetAmountBtnOn.SetActive(false);
        BetAmountBtnOff.SetActive(true);
        ProfitBtnOn.SetActive(false);
        ProfitBtnOff.SetActive(true);
    }
    public void OpenStorePts()
    {
        TotalGoldPackUI.SetActive(false);
        StorePtsPackUI.SetActive(true);
        ShowTimeBadgesPackUI.SetActive(false);
        BetAmountPackUI.SetActive(false);
        ProfitPackUI.SetActive(false);

        TotalGoldBtnOn.SetActive(false);
        TotalGoldBtnOff.SetActive(true);
        StorePtsBtnOn.SetActive(true);
        StorePtsBtnOff.SetActive(false);
        ShowTimeBadgesBtnOn.SetActive(false);
        ShowTimeBadgesBtnOff.SetActive(true);
        BetAmountBtnOn.SetActive(false);
        BetAmountBtnOff.SetActive(true);
        ProfitBtnOn.SetActive(false);
        ProfitBtnOff.SetActive(true);
    }

    public void OpenShowTimeBadges()
    {
        TotalGoldPackUI.SetActive(false);
        StorePtsPackUI.SetActive(false);
        ShowTimeBadgesPackUI.SetActive(true);
        BetAmountPackUI.SetActive(false);
        ProfitPackUI.SetActive(false);

        TotalGoldBtnOn.SetActive(false);
        TotalGoldBtnOff.SetActive(true);
        StorePtsBtnOn.SetActive(false);
        StorePtsBtnOff.SetActive(true);
        ShowTimeBadgesBtnOn.SetActive(true);
        ShowTimeBadgesBtnOff.SetActive(false);
        BetAmountBtnOn.SetActive(false);
        BetAmountBtnOff.SetActive(true);
        ProfitBtnOn.SetActive(false);
        ProfitBtnOff.SetActive(true);
    }

    public void OpenBetAmount()
    {
        TotalGoldPackUI.SetActive(false);
        StorePtsPackUI.SetActive(false);
        ShowTimeBadgesPackUI.SetActive(false);
        BetAmountPackUI.SetActive(true);
        ProfitPackUI.SetActive(false);

        TotalGoldBtnOn.SetActive(false);
        TotalGoldBtnOff.SetActive(true);
        StorePtsBtnOn.SetActive(false);
        StorePtsBtnOff.SetActive(true);
        ShowTimeBadgesBtnOn.SetActive(false);
        ShowTimeBadgesBtnOff.SetActive(true);
        BetAmountBtnOn.SetActive(true);
        BetAmountBtnOff.SetActive(false);
        ProfitBtnOn.SetActive(false);
        ProfitBtnOff.SetActive(true);
    }
    public void OpenProfit()
    {
        TotalGoldPackUI.SetActive(false);
        StorePtsPackUI.SetActive(false);
        ShowTimeBadgesPackUI.SetActive(false);
        BetAmountPackUI.SetActive(false);
        ProfitPackUI.SetActive(true);

        TotalGoldBtnOn.SetActive(false);
        TotalGoldBtnOff.SetActive(true);
        StorePtsBtnOn.SetActive(false);
        StorePtsBtnOff.SetActive(true);
        ShowTimeBadgesBtnOn.SetActive(false);
        ShowTimeBadgesBtnOff.SetActive(true);
        BetAmountBtnOn.SetActive(false);
        BetAmountBtnOff.SetActive(true);
        ProfitBtnOn.SetActive(true);
        ProfitBtnOff.SetActive(false);
    }




    void Start()
    {
        superPackIndex = 0;
        slashPackIndex = 0;
        rechargePackindex = 0;

    }


    void Update()
    {
        // SlashPackagePanel
        if (slashPackIndex >= slashPackSlideBG.Length)
            slashPackIndex = slashPackSlideBG.Length;

        if (slashPackIndex < 0)
            slashPackIndex = 0;

        if (slashPackIndex == 0)
        {
            slashPackSlideBG[0].gameObject.SetActive(true);
            PreviousSlashBtn.gameObject.SetActive(false);
        }
        if (slashPackIndex == slashPackSlideBG.Length - 1)
        {
            NextSlashBtn.gameObject.SetActive(false);

        }

        // SuperPackagePanel
        if (superPackIndex >= superPackSlideBG.Length)
            superPackIndex = superPackSlideBG.Length;

        if (superPackIndex < 0)
            superPackIndex = 0;

        if (superPackIndex == 0)
        {
            superPackSlideBG[0].gameObject.SetActive(true);
            PreviousSuperBtn.gameObject.SetActive(false);

        }
        if (superPackIndex == superPackSlideBG.Length - 1)
        {
            NextSuperBtn.gameObject.SetActive(false);

        }

        // Recharge Pack
        if (rechargePackindex >= rechargePackSlideBG.Length)
            rechargePackindex = rechargePackSlideBG.Length;

        if (rechargePackindex < 0)
            rechargePackindex = 0;

        if (rechargePackindex == 0)
        {
            rechargePackSlideBG[0].gameObject.SetActive(true);
            PreviousRechargeBtn.gameObject.SetActive(false);
        }
        if (rechargePackindex == rechargePackSlideBG.Length - 1)
        {
            NextRechargeBtn.gameObject.SetActive(false);

        }

        //Pay Table

        if (tablePackindex >= tablePackSlideBG.Length)
            tablePackindex = tablePackSlideBG.Length;

        if (tablePackindex < 0)
            tablePackindex = 0;

        if (tablePackindex == 0)
        {
            tablePackSlideBG[0].gameObject.SetActive(true);
            PreviousTableBtn.gameObject.SetActive(false);
        }
        if (tablePackindex == tablePackSlideBG.Length - 1)
        {
            NextTableBtn.gameObject.SetActive(false);

        }

    }

    [Header("SlashPackage Panel UI")]
    // SlashPackagePanel UI
    public GameObject[] slashPackSlideBG;
    public GameObject NextSlashBtn;
    public GameObject PreviousSlashBtn;
    int slashPackIndex;
    public void NextSlashPack()
    {
        slashPackIndex += 1;
        PreviousSlashBtn.gameObject.SetActive(true);

        for (int i = 0; i < slashPackSlideBG.Length; i++)
        {
            slashPackSlideBG[i].gameObject.SetActive(false);
            slashPackSlideBG[slashPackIndex].gameObject.SetActive(true);
        }
        Debug.Log(slashPackIndex);
    }

    public void PreviousSlashPack()
    {
        slashPackIndex -= 1;
        NextSlashBtn.gameObject.SetActive(true);

        for (int i = 0; i < slashPackSlideBG.Length; i++)
        {
            slashPackSlideBG[i].gameObject.SetActive(false);
            slashPackSlideBG[slashPackIndex].gameObject.SetActive(true);
        }
        Debug.Log(slashPackIndex);
    }

    //SuperPackagePanel UI
    [Header("SlashPackage Panel UI")]
    public GameObject[] superPackSlideBG;
    public GameObject NextSuperBtn;
    public GameObject PreviousSuperBtn;
    int superPackIndex;

    public void NextSuperPack()
    {
        superPackIndex += 1;
        PreviousSuperBtn.gameObject.SetActive(true);

        for (int i = 0; i < superPackSlideBG.Length; i++)
        {
            superPackSlideBG[i].gameObject.SetActive(false);
            superPackSlideBG[superPackIndex].gameObject.SetActive(true);

        }
        Debug.Log(superPackIndex);

    }

    public void PreviousSuperPack()
    {
        superPackIndex -= 1;
        NextSuperBtn.gameObject.SetActive(true);

        for (int i = 0; i < superPackSlideBG.Length; i++)
        {
            superPackSlideBG[i].gameObject.SetActive(false);
            superPackSlideBG[superPackIndex].gameObject.SetActive(true);
        }
        Debug.Log(superPackIndex);
    }

    //RechargePanel UI
    public GameObject[] rechargePackSlideBG;
    public GameObject NextRechargeBtn;
    public GameObject PreviousRechargeBtn;
    int rechargePackindex;

    public void NextRechargePack()
    {
        rechargePackindex += 1;
        PreviousRechargeBtn.gameObject.SetActive(true);

        for (int i = 0; i < rechargePackSlideBG.Length; i++)
        {
            rechargePackSlideBG[i].gameObject.SetActive(false);
            rechargePackSlideBG[rechargePackindex].gameObject.SetActive(true);
        }
        Debug.Log(rechargePackindex);
    }

    public void PreviousRechargePack()
    {
        rechargePackindex -= 1;
        NextRechargeBtn.gameObject.SetActive(true);

        for (int i = 0; i < rechargePackSlideBG.Length; i++)
        {
            rechargePackSlideBG[i].gameObject.SetActive(false);
            rechargePackSlideBG[rechargePackindex].gameObject.SetActive(true);
        }
        Debug.Log(rechargePackindex);
    }

    // Pay Table UI
    public GameObject[] tablePackSlideBG;
    public GameObject NextTableBtn;
    public GameObject PreviousTableBtn;
    int tablePackindex;

    public void NextTablePack()
    {
        tablePackindex += 1;
        PreviousTableBtn.gameObject.SetActive(true);

        for (int i = 0; i < tablePackSlideBG.Length; i++)
        {
            tablePackSlideBG[i].gameObject.SetActive(false);
            tablePackSlideBG[tablePackindex].gameObject.SetActive(true);
        }
        Debug.Log(tablePackindex);
    }

    public void PreviousTablePack()
    {
        tablePackindex -= 1;
        NextTableBtn.gameObject.SetActive(true);

        for (int i = 0; i < tablePackSlideBG.Length; i++)
        {
            tablePackSlideBG[i].gameObject.SetActive(false);
            tablePackSlideBG[tablePackindex].gameObject.SetActive(true);
        }
        Debug.Log(tablePackindex);
    }
}
