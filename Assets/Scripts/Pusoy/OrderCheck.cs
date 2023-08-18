using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class OrderCheck : MonoBehaviour
{
    public new List<Image> front = new List<Image>();
    public new List<Image> middle = new List<Image>();
    public new List<Image> back = new List<Image>();
    [SerializeField] private TextMeshProUGUI frontvalue;
    [SerializeField] private TextMeshProUGUI middlevalue;
    [SerializeField] private TextMeshProUGUI backvalue;
    [SerializeField] private PusoyGameManager gameManager;
    int f1, f2, f3, m1, m2, m3, m4, m5, b1, b2, b3, b4, b5;
    int ftype1, ftype2, ftype3, mtype1, mtype2, mtype3, mtype4, mtype5, btype1, btype2, btype3, btype4, btype5;
    //back bool
    bool hcard1false = true;
    bool hcard2false = true;
    bool onepairfalse = true;
    bool twopairfalse = true;
    bool threeofkindfalse = true;
    bool straightcheck = true;
    bool flushseqcheck = true;
    bool flushchecker = true;
    bool fullhousefalse = true;
    bool fourofkindfalse = true;
    bool straightflushfalse = true;
    bool royalflushfalse = true;
    //middle bool
    bool midhcard1false = true;
    bool midhcard2false = true;
    bool midonepairfalse = true;
    bool midtwopairfalse = true;
    bool midthreeofkindfalse = true;
    bool midstraightcheck = true;
    bool midflushseqcheck = true;
    bool midflushchecker = true;
    bool midfullhousefalse = true;
    bool midfourofkindfalse = true;
    bool midstraightflushfalse = true;
    bool midroyalflushfalse = true;
    //front bool
    bool frnthcard1false = true;
    bool frnthcard2false = true;
    bool frntonepairfalse = true;
    public int frontorder = 0;
    public int middleorder = 0;
    public int backorder = 0;
    public List<int> cardSpritelist = new List<int>();
    //private SoundManager soundManager;

    private void Start()
    {
       // soundManager = GameObject.FindWithTag("GameMusic").GetComponent<SoundManager>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckOrder();
    }
    void SetValue()
    {
        f1 = front[0].GetComponent<CardIndex>().index;
        f2 = front[1].GetComponent<CardIndex>().index;
        f3 = front[2].GetComponent<CardIndex>().index;
        ftype1 = front[0].GetComponent<CardIndex>().cardType;
        ftype2 = front[1].GetComponent<CardIndex>().cardType;
        ftype3 = front[2].GetComponent<CardIndex>().cardType;

        m1 = middle[0].GetComponent<CardIndex>().index;
        m2 = middle[1].GetComponent<CardIndex>().index;
        m3 = middle[2].GetComponent<CardIndex>().index;
        m4 = middle[3].GetComponent<CardIndex>().index;
        m5 = middle[4].GetComponent<CardIndex>().index;
        mtype1 = middle[0].GetComponent<CardIndex>().cardType;
        mtype2 = middle[1].GetComponent<CardIndex>().cardType;
        mtype3 = middle[2].GetComponent<CardIndex>().cardType;
        mtype4 = middle[3].GetComponent<CardIndex>().cardType;
        mtype5 = middle[4].GetComponent<CardIndex>().cardType;

        b1 = back[0].GetComponent<CardIndex>().index;
        b2 = back[1].GetComponent<CardIndex>().index;
        b3 = back[2].GetComponent<CardIndex>().index;
        b4 = back[3].GetComponent<CardIndex>().index;
        b5 = back[4].GetComponent<CardIndex>().index;
        btype1 = back[0].GetComponent<CardIndex>().cardType;
        btype2 = back[1].GetComponent<CardIndex>().cardType;
        btype3 = back[2].GetComponent<CardIndex>().cardType;
        btype4 = back[3].GetComponent<CardIndex>().cardType;
        btype5 = back[4].GetComponent<CardIndex>().cardType;
    }
    void CheckOrder()
    {
        SetValue();
        HighCard();
        OnePair();
        TwoPair();
        ThreeOfKind();
        Straight();
        CheckFlush();
        FullHouse();
        FourOfKind();
        StraightFlush();
        RoyalFlush();
    }
    void HighCard()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        backcards.Add(b1);
        backcards.Add(b2);
        backcards.Add(b3);
        backcards.Add(b4);
        backcards.Add(b5);
        List<int> backtype = new List<int>();
        backtype.Add(btype1);
        backtype.Add(btype2);
        backtype.Add(btype3);
        backtype.Add(btype4);
        backtype.Add(btype5);

     //  Debug.Log("BackcardsCount:" + backcards.Count);
        int typecheck = 0;
        bool checker = false;
        bool checker2 = false;

        for (int i = 0; i < backcards.Count; i++)
        {
            for (int j = i + 1; j < backcards.Count; j++)
            {
                if (backcards[i] == backcards[j])
                {
                    checker = true;
                    break;
                }
            }
            if (checker == true)
            {
                hcard2false = true;
                break;
            }
            else
                hcard1false = false;
        }

        if (checker == false)
        {
            for (int i = 0; i < backtype.Count; i++)
            {
                for (int j = i + 1; j < backtype.Count; j++)
                {
                    if (backtype[i] == backtype[j])
                    {
                        typecheck += 1;
                    }
                    if (typecheck == 5)
                    {
                        checker2 = true;
                        break;
                    }
                }
                if (checker2 == true)
                {
                    break;
                }
                else
                    hcard2false = false;
            }

        }
        #endregion

        #region middle
        List<int> middlecards = new List<int>();
        middlecards.Add(m1);
        middlecards.Add(m2);
        middlecards.Add(m3);
        middlecards.Add(m4);
        middlecards.Add(m5);
        List<int> middletype = new List<int>();
        middletype.Add(mtype1);
        middletype.Add(mtype2);
        middletype.Add(mtype3);
        middletype.Add(mtype4);
        middletype.Add(mtype5);

        //  Debug.Log("BackcardsCount:" + backcards.Count);
        int midtypecheck = 0;
        bool midchecker = false;
        bool midchecker2 = false;

        for (int i = 0; i < middlecards.Count; i++)
        {
            for (int j = i + 1; j < middlecards.Count; j++)
            {
                if (middlecards[i] == middlecards[j])
                {
                    midchecker = true;
                    break;
                }
            }
            if (midchecker == true)
            {
                midhcard2false = true;
                break;
            }
            else
                midhcard1false = false;
        }

        if (midchecker == false)
        {
            for (int i = 0; i < middletype.Count; i++)
            {
                for (int j = i + 1; j < middletype.Count; j++)
                {
                    if (middletype[i] == middletype[j])
                    {
                        midtypecheck += 1;
                    }
                    if (midtypecheck == 5)
                    {
                        midchecker2 = true;
                        break;
                    }
                }
                if (midchecker2 == true)
                {
                    break;
                }
                else
                    midhcard2false = false;
            }

        }
        #endregion

        #region front
        List<int> frontcards = new List<int>();
        frontcards.Add(f1);
        frontcards.Add(f2);
        frontcards.Add(f3);

        List<int> fronttype = new List<int>();
        fronttype.Add(ftype1);
        fronttype.Add(ftype2);
        fronttype.Add(ftype3);

        //  Debug.Log("BackcardsCount:" + backcards.Count);
        int frnttypecheck = 0;
        bool frntchecker = false;
        bool frntchecker2 = false;

        for (int i = 0; i < frontcards.Count; i++)
        {
            for (int j = i + 1; j < frontcards.Count; j++)
            {
                if (frontcards[i] == frontcards[j])
                {
                    frntchecker = true;
                    break;
                }
            }
            if (frntchecker == true)
            {
                frnthcard2false = true;
                break;
            }
            else
                frnthcard1false = false;
        }

        if (frntchecker == false)
        {
            for (int i = 0; i < fronttype.Count; i++)
            {
                for (int j = i + 1; j < fronttype.Count; j++)
                {
                    if (fronttype[i] == fronttype[j])
                    {
                        frnttypecheck += 1;
                    }
                    if (frnttypecheck == 4)
                    {
                        frntchecker2 = true;
                        break;
                    }
                }
                if (frntchecker2 == true)
                {
                    break;
                }
                else
                    frnthcard2false = false;
            }

        }
        #endregion
        CardTextCheck();
    }

    void OnePair()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        backcards.Add(b1);
        backcards.Add(b2);
        backcards.Add(b3);
        backcards.Add(b4);
        backcards.Add(b5);

        int cardcheck = 0;

        for (int i = 0; i < backcards.Count; i++)
        {
            for (int j = i + 1; j < backcards.Count; j++)
            {
                if (backcards[i] == backcards[j])
                {
                    cardcheck += 1;
                }
            }
        }
        if (cardcheck >= 2 || cardcheck == 0)
        {
            onepairfalse = true;
        }
        if (cardcheck == 1)
            onepairfalse = false;
        #endregion

        #region middle
        List<int> middlecards = new List<int>();
        middlecards.Add(m1);
        middlecards.Add(m2);
        middlecards.Add(m3);
        middlecards.Add(m4);
        middlecards.Add(m5);

        int midcardcheck = 0;

        for (int i = 0; i < middlecards.Count; i++)
        {
            for (int j = i + 1; j < middlecards.Count; j++)
            {
                if (middlecards[i] == middlecards[j])
                {
                    midcardcheck += 1;
                }
            }
        }
        if (midcardcheck >= 2 || midcardcheck == 0)
        {
            midonepairfalse = true;
        }
        if (midcardcheck == 1)
            midonepairfalse = false;
        #endregion

        #region front
        List<int> frontcards = new List<int>();
        frontcards.Add(f1);
        frontcards.Add(f2);
        frontcards.Add(f3);

        int frntcardcheck = 0;

        for (int i = 0; i < frontcards.Count; i++)
        {
            for (int j = i + 1; j < frontcards.Count; j++)
            {
                if (frontcards[i] == frontcards[j])
                {
                    frntcardcheck += 1;
                }
            }
        }
        if (frntcardcheck >= 2 || frntcardcheck == 0)
        {
            frntonepairfalse = true;
        }
        if (frntcardcheck == 1)
            frntonepairfalse = false;
        #endregion
        CardTextCheck();
    }
    void TwoPair()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        backcards.Add(b1);
        backcards.Add(b2);
        backcards.Add(b3);
        backcards.Add(b4);
        backcards.Add(b5);

        int cardcheck = 0;

        for (int i = 0; i < backcards.Count; i++)
        {
            for (int j = i + 1; j < backcards.Count; j++)
            {
                if (backcards[i] == backcards[j])
                {
                    cardcheck += 1;
                }
            }
        }
        if (cardcheck > 2 || cardcheck <= 1)
        {
            twopairfalse = true;
        }
        if (cardcheck == 2)
            twopairfalse = false;
        #endregion

        #region middle
        List<int> middlecards = new List<int>();
        middlecards.Add(m1);
        middlecards.Add(m2);
        middlecards.Add(m3);
        middlecards.Add(m4);
        middlecards.Add(m5);

        int midcardcheck = 0;

        for (int i = 0; i < middlecards.Count; i++)
        {
            for (int j = i + 1; j < middlecards.Count; j++)
            {
                if (middlecards[i] == middlecards[j])
                {
                    midcardcheck += 1;
                }
            }
        }
        if (midcardcheck > 2 || midcardcheck <= 1)
        {
            midtwopairfalse = true;
        }
        if (midcardcheck == 2)
            midtwopairfalse = false;
        #endregion
        CardTextCheck();
    }

    void ThreeOfKind()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        backcards.Add(b1);
        backcards.Add(b2);
        backcards.Add(b3);
        backcards.Add(b4);
        backcards.Add(b5);

        int cardcheck = 0;

        for(int i =0; i < backcards.Count; i++)
        {
            for(int j = i+1; j< backcards.Count; j++)
            {
                for(int k = j+1; k < backcards.Count; k++)
                {
                    if(backcards[i] == backcards[j] && backcards[j] == backcards[k])
                    {
                        cardcheck += 1;
                    }
                }
            }
        }
        if (cardcheck == 1)
            threeofkindfalse = false;
        else
            threeofkindfalse = true;
        #endregion

        #region middle
        List<int> middlecards = new List<int>();
        middlecards.Add(m1);
        middlecards.Add(m2);
        middlecards.Add(m3);
        middlecards.Add(m4);
        middlecards.Add(m5);

        int midcardcheck = 0;

        for (int i = 0; i < middlecards.Count; i++)
        {
            for (int j = i + 1; j < middlecards.Count; j++)
            {
                for (int k = j + 1; k < middlecards.Count; k++)
                {
                    if (middlecards[i] == middlecards[j] && middlecards[j] == middlecards[k])
                    {
                        midcardcheck += 1;
                    }
                }
            }
        }
        if (midcardcheck == 1)
            midthreeofkindfalse = false;
        else
            midthreeofkindfalse = true;
        #endregion
        CardTextCheck();
    }

    void Straight()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        backcards.Add(b1);
        backcards.Add(b2);
        backcards.Add(b3);
        backcards.Add(b4);
        backcards.Add(b5);

        backcards.Sort();
        bool scheck = false;
        if (backcards[0] == 1 && backcards[1] == 10)
        {
            List<int> backseqchecklist = new List<int>();
            backseqchecklist.Add(backcards[1]);
            backseqchecklist.Add(backcards[2]);
            backseqchecklist.Add(backcards[3]);
            backseqchecklist.Add(backcards[4]);
            IsSequential(backseqchecklist);
            scheck = IsSequential(backseqchecklist);
        }
        else
        {
            IsSequential(backcards);
            scheck = IsSequential(backcards);
        }
       // Debug.Log("sequential" + scheck);

        if (scheck)
            straightcheck = false;
        else
            straightcheck = true;
        #endregion

        #region middle
        List<int> middlecards = new List<int>();
        middlecards.Add(m1);
        middlecards.Add(m2);
        middlecards.Add(m3);
        middlecards.Add(m4);
        middlecards.Add(m5);

        middlecards.Sort();
        bool midscheck = false;
        if (middlecards[0] == 1 && middlecards[1] == 10)
        {
            List<int> midseqchecklist = new List<int>();
            midseqchecklist.Add(middlecards[1]);
            midseqchecklist.Add(middlecards[2]);
            midseqchecklist.Add(middlecards[3]);
            midseqchecklist.Add(middlecards[4]);
            IsSequential(midseqchecklist);
            scheck = IsSequential(midseqchecklist);
        }
        else
        {
            IsSequential(middlecards);
            midscheck = IsSequential(middlecards);
        }


        if (midscheck)
            midstraightcheck = false;
        else
            midstraightcheck = true;
        #endregion
    }
    bool IsSequential(List<int> backcards)
    {
        return backcards.Zip(backcards.Skip(1), (a, b) => (a + 1) == b).All(x => x);
    }
    void CheckFlush()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        backcards.Add(b1);
        backcards.Add(b2);
        backcards.Add(b3);
        backcards.Add(b4);
        backcards.Add(b5);
        List<int> backtype = new List<int>();
        backtype.Add(btype1);
        backtype.Add(btype2);
        backtype.Add(btype3);
        backtype.Add(btype4);
        backtype.Add(btype5);

        backcards.Sort();
        IsSequential(backcards);
        bool scheck = IsSequential(backcards);
       // Debug.Log("sequential" + scheck);
        bool flushistrue = false;

        if (scheck)
            flushseqcheck = false;
        else
            flushseqcheck = true;

        if(flushseqcheck)
        {
            for(int i = 0; i< backtype.Count; i++)
            {
                for(int j = i + 1; j< backtype.Count; j++)
                {
                    for (int k = j + 1; k < backtype.Count; k++)
                    {
                        for (int l = k + 1; l < backtype.Count; l++)
                        {
                            for (int m = l + 1; m < backtype.Count; m++)
                            {
                                if(backtype[i] == backtype[j] && backtype[j] == backtype[k] && backtype[k] == backtype[l] && backtype[l] == backtype[m])
                                {
                                    flushistrue = true;
                                }
                                break;
                            }
                            break;
                        }
                        break;
                    }
                    break;
                }
                break;
            }
            if (flushistrue)
                flushchecker = false;
            else
                flushchecker = true;
        }
        #endregion

        #region middle
        List<int> middlecards = new List<int>();
        middlecards.Add(m1);
        middlecards.Add(m2);
        middlecards.Add(m3);
        middlecards.Add(m4);
        middlecards.Add(m5);
        List<int> middletype = new List<int>();
        middletype.Add(mtype1);
        middletype.Add(mtype2);
        middletype.Add(mtype3);
        middletype.Add(mtype4);
        middletype.Add(mtype5);

        middlecards.Sort();
        IsSequential(middlecards);
        bool midscheck = IsSequential(middlecards);
        bool midflushistrue = false;

        if (midscheck)
            midflushseqcheck = false;
        else
            midflushseqcheck = true;

        if (midflushseqcheck)
        {
            for (int i = 0; i < middletype.Count; i++)
            {
                for (int j = i + 1; j < middletype.Count; j++)
                {
                    for (int k = j + 1; k < middletype.Count; k++)
                    {
                        for (int l = k + 1; l < middletype.Count; l++)
                        {
                            for (int m = l + 1; m < middletype.Count; m++)
                            {
                                if (middletype[i] == middletype[j] && middletype[j] == middletype[k] && middletype[k] == middletype[l] && middletype[l] == middletype[m])
                                {
                                    midflushistrue = true;
                                }
                                break;
                            }
                            break;
                        }
                        break;
                    }
                    break;
                }
                break;
            }
            if (midflushistrue)
                midflushchecker = false;
            else
                midflushchecker = true;
        }
        #endregion
    }
    void FullHouse()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        backcards.Add(b1);
        backcards.Add(b2);
        backcards.Add(b3);
        backcards.Add(b4);
        backcards.Add(b5);

        backcards.Sort();
        List<int> times = new List<int>();
        Dictionary<int, int> oGroups = new Dictionary<int, int>();

        foreach (int iCurrentValue in backcards)
        {
            if (oGroups.ContainsKey(iCurrentValue))
                oGroups[iCurrentValue]++;
            else
                oGroups.Add(iCurrentValue, 1);
        }

        foreach (KeyValuePair<int, int> oGroup in oGroups)
        {
         //   Debug.Log($"Value {oGroup.Key} appears {oGroup.Value} times.");
            times.Add(oGroup.Value);
        }
        times.Sort();
        if (times[0] == 2 && times[1] == 3)
            fullhousefalse = false;
        else
            fullhousefalse = true;
        #endregion

        #region middle
        List<int> middlecards = new List<int>();
        middlecards.Add(m1);
        middlecards.Add(m2);
        middlecards.Add(m3);
        middlecards.Add(m4);
        middlecards.Add(m5);

        middlecards.Sort();
        List<int> midtimes = new List<int>();
        Dictionary<int, int> midoGroups = new Dictionary<int, int>();

        foreach (int iCurrentValue in middlecards)
        {
            if (midoGroups.ContainsKey(iCurrentValue))
                midoGroups[iCurrentValue]++;
            else
                midoGroups.Add(iCurrentValue, 1);
        }

        foreach (KeyValuePair<int, int> midoGroup in midoGroups)
        {
            //   Debug.Log($"Value {oGroup.Key} appears {oGroup.Value} times.");
            midtimes.Add(midoGroup.Value);
        }
        midtimes.Sort();
        if (midtimes[0] == 2 && midtimes[1] == 3)
            midfullhousefalse = false;
        else
            midfullhousefalse = true;
        #endregion
    }
    void FourOfKind()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        backcards.Add(b1);
        backcards.Add(b2);
        backcards.Add(b3);
        backcards.Add(b4);
        backcards.Add(b5);

        int cardcheck = 0;

        for (int i = 0; i < backcards.Count; i++)
        {
            for (int j = i + 1; j < backcards.Count; j++)
            {
                for (int k = j + 1; k < backcards.Count; k++)
                {
                    for (int l = k + 1; l < backcards.Count; l++)
                    {
                        if (backcards[i] == backcards[j] && backcards[j] == backcards[k] && backcards[k] == backcards[l])
                        {
                            cardcheck += 1;
                        }
                    }
                }
            }
        }
        if (cardcheck == 1)
            fourofkindfalse = false;
        else
            fourofkindfalse = true;
        #endregion

        #region middle
        List<int> middlecards = new List<int>();
        middlecards.Add(m1);
        middlecards.Add(m2);
        middlecards.Add(m3);
        middlecards.Add(m4);
        middlecards.Add(m5);

        int midcardcheck = 0;

        for (int i = 0; i < middlecards.Count; i++)
        {
            for (int j = i + 1; j < middlecards.Count; j++)
            {
                for (int k = j + 1; k < middlecards.Count; k++)
                {
                    for (int l = k + 1; l < middlecards.Count; l++)
                    {
                        if (middlecards[i] == middlecards[j] && middlecards[j] == middlecards[k] && middlecards[k] == middlecards[l])
                        {
                            midcardcheck += 1;
                        }
                    }
                }
            }
        }
        if (midcardcheck == 1)
            midfourofkindfalse = false;
        else
            midfourofkindfalse = true;
        #endregion
        CardTextCheck();
    }
    void StraightFlush()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        backcards.Add(b1);
        backcards.Add(b2);
        backcards.Add(b3);
        backcards.Add(b4);
        backcards.Add(b5);

        backcards.Sort();

        List<int> backtype = new List<int>();
        backtype.Add(btype1);
        backtype.Add(btype2);
        backtype.Add(btype3);
        backtype.Add(btype4);
        backtype.Add(btype5);

        IsSequential(backcards);
        bool strflush = false;
        bool scheck = IsSequential(backcards);
        // Debug.Log("sequential" + scheck);

        if (scheck)
        {
            for (int i = 0; i < backtype.Count; i++)
            {
                for (int j = i + 1; j < backtype.Count; j++)
                {
                    for (int k = j + 1; k < backtype.Count; k++)
                    {
                        for (int l = k + 1; l < backtype.Count; l++)
                        {
                            for (int m = l + 1; m < backtype.Count; m++)
                            {
                                if (backtype[i] == backtype[j] && backtype[j] == backtype[k] && backtype[k] == backtype[l] && backtype[l] == backtype[m])
                                {
                                    strflush = true;
                                }
                                break;
                            }
                            break;
                        }
                        break;
                    }
                    break;
                }
                break;
            }
        }
        if (strflush)
            straightflushfalse = false;
        else
            straightflushfalse = true;

        #endregion

        #region middle
        List<int> middlecards = new List<int>();
        middlecards.Add(m1);
        middlecards.Add(m2);
        middlecards.Add(m3);
        middlecards.Add(m4);
        middlecards.Add(m5);
        List<int> middletype = new List<int>();
        middletype.Add(mtype1);
        middletype.Add(mtype2);
        middletype.Add(mtype3);
        middletype.Add(mtype4);
        middletype.Add(mtype5);

        middlecards.Sort();

        IsSequential(middlecards);
        bool midstrflush = false;
        bool midscheck = IsSequential(middlecards);

        if (midscheck)
        {
            for (int i = 0; i < middletype.Count; i++)
            {
                for (int j = i + 1; j < middletype.Count; j++)
                {
                    for (int k = j + 1; k < middletype.Count; k++)
                    {
                        for (int l = k + 1; l < middletype.Count; l++)
                        {
                            for (int m = l + 1; m < middletype.Count; m++)
                            {
                                if (middletype[i] == middletype[j] && middletype[j] == middletype[k] && middletype[k] == middletype[l] && middletype[l] == middletype[m])
                                {
                                    midstrflush = true;
                                }
                                break;
                            }
                            break;
                        }
                        break;
                    }
                    break;
                }
                break;
            }
        }
        if (midstrflush)
            midstraightflushfalse = false;
        else
            midstraightflushfalse = true;
        #endregion
    }
    void RoyalFlush()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        backcards.Add(b1);
        backcards.Add(b2);
        backcards.Add(b3);
        backcards.Add(b4);
        backcards.Add(b5);

        backcards.Sort();

        List<int> backtype = new List<int>();
        backtype.Add(btype1);
        backtype.Add(btype2);
        backtype.Add(btype3);
        backtype.Add(btype4);
        backtype.Add(btype5);

        List<int> seqchecklist = new List<int>();
        seqchecklist.Add(backcards[1]);
        seqchecklist.Add(backcards[2]);
        seqchecklist.Add(backcards[3]);
        seqchecklist.Add(backcards[4]);

        IsSequential(seqchecklist);
        bool scheck = IsSequential(seqchecklist);
        bool rfcheck = false;
        bool rfcheckmain = false;
        if(scheck)
        {
            if (backcards[0] == 1 && seqchecklist[0] == 10)
                rfcheckmain = true;
            else
                rfcheckmain = false;
        }


        if(rfcheckmain)
        {
            for (int i = 0; i < backtype.Count; i++)
            {
                for (int j = i + 1; j < backtype.Count; j++)
                {
                    for (int k = j + 1; k < backtype.Count; k++)
                    {
                        for (int l = k + 1; l < backtype.Count; l++)
                        {
                            for (int m = l + 1; m < backtype.Count; m++)
                            {
                                if (backtype[i] == backtype[j] && backtype[j] == backtype[k] && backtype[k] == backtype[l] && backtype[l] == backtype[m])
                                {
                                    rfcheck = true;
                                }
                                break;
                            }
                            break;
                        }
                        break;
                    }
                    break;
                }
                break;
            }
        }
        if (rfcheck)
            royalflushfalse = false;
        else
            royalflushfalse = true;
        #endregion

        #region middle
        List<int> middlecards = new List<int>();
        middlecards.Add(m1);
        middlecards.Add(m2);
        middlecards.Add(m3);
        middlecards.Add(m4);
        middlecards.Add(m5);
        List<int> middletype = new List<int>();
        middletype.Add(mtype1);
        middletype.Add(mtype2);
        middletype.Add(mtype3);
        middletype.Add(mtype4);
        middletype.Add(mtype5);

        middlecards.Sort();

        List<int> midseqchecklist = new List<int>();
        midseqchecklist.Add(middlecards[1]);
        midseqchecklist.Add(middlecards[2]);
        midseqchecklist.Add(middlecards[3]);
        midseqchecklist.Add(middlecards[4]);

        IsSequential(midseqchecklist);
        bool midscheck = IsSequential(midseqchecklist);
        bool midrfcheck = false;
        bool midrfcheckmain = false;
        if (midscheck)
        {
            if (middlecards[0] == 1 && middlecards[0] == 10)
                midrfcheckmain = true;
            else
                midrfcheckmain = false;
        }


        if (midrfcheckmain)
        {
            for (int i = 0; i < middletype.Count; i++)
            {
                for (int j = i + 1; j < middletype.Count; j++)
                {
                    for (int k = j + 1; k < middletype.Count; k++)
                    {
                        for (int l = k + 1; l < middletype.Count; l++)
                        {
                            for (int m = l + 1; m < middletype.Count; m++)
                            {
                                if (middletype[i] == middletype[j] && middletype[j] == middletype[k] && middletype[k] == middletype[l] && middletype[l] == middletype[m])
                                {
                                    midrfcheck = true;
                                }
                                break;
                            }
                            break;
                        }
                        break;
                    }
                    break;
                }
                break;
            }
        }
        if (midrfcheck)
            midroyalflushfalse = false;
        else
            midroyalflushfalse = true;
        #endregion
    }
    void CardTextCheck()
    {
        #region back
        if (royalflushfalse == false)
        {
            backorder = 10;
            backvalue.text = "RoyalFlush";
        }

        if (straightflushfalse == false && royalflushfalse == true)
        {
            backorder = 9;
            backvalue.text = "StraightFlush";
        }

        if (fourofkindfalse == false && straightflushfalse == true && royalflushfalse == true)
        {
            backorder = 8;
            backvalue.text = "FourOfKind";
        }

        if (fullhousefalse == false && fourofkindfalse == true && straightflushfalse == true && royalflushfalse == true)
        {
            backorder = 7;
            backvalue.text = "FullHouse";
        }

        if (flushchecker == false && fullhousefalse == true && fourofkindfalse == true && straightflushfalse == true && royalflushfalse == true)
        {
            backorder = 6;
            backvalue.text = "Flush";
        }

        if (straightcheck == false && flushchecker == true && fullhousefalse == true && fourofkindfalse == true && straightflushfalse == true && royalflushfalse == true)
        {
            backorder = 5;
            backvalue.text = "Straight";
        }

        if (threeofkindfalse == false && straightcheck == true && flushchecker == true && fullhousefalse == true && fourofkindfalse == true && straightflushfalse == true && royalflushfalse == true)
        {
            backorder = 4;
            backvalue.text = "ThreeOfKind";
        }

        if (twopairfalse == false && threeofkindfalse == true && straightcheck == true && flushchecker == true && fullhousefalse == true && fourofkindfalse == true && straightflushfalse == true && royalflushfalse == true)
        {
            backorder = 3;
            backvalue.text = "TwoPair";
        }

        if (onepairfalse == false && twopairfalse == true && threeofkindfalse == true && straightcheck == true && flushchecker == true && fullhousefalse == true && fourofkindfalse == true && straightflushfalse == true && royalflushfalse == true)
        {
            backorder = 2;
            backvalue.text = "OnePair";
        }

        else if (twopairfalse == true && onepairfalse == true && hcard2false == false && threeofkindfalse == true && straightcheck == true && flushchecker == true && fullhousefalse == true && fourofkindfalse == true && straightflushfalse == true && royalflushfalse == true)
        {
            backorder = 1;
            backvalue.text = "HighCard";
        }
        #endregion

        #region middle
        if (midroyalflushfalse == false)
        {
            middleorder = 10;
            middlevalue.text = "RoyalFlush";
        }

        if (midstraightflushfalse == false && midroyalflushfalse == true)
        {
            middleorder = 9;
            middlevalue.text = "StraightFlush";
        }

        if (midfourofkindfalse == false && midstraightflushfalse == true && midroyalflushfalse == true)
        {
            middleorder = 8;
            middlevalue.text = "FourOfKind";
        }

        if (midfullhousefalse == false && midfourofkindfalse == true && midstraightflushfalse == true && midroyalflushfalse == true)
        {
            middleorder = 7;
            middlevalue.text = "FullHouse";
        }

        if (midflushchecker == false && midfullhousefalse == true && midfourofkindfalse == true && midstraightflushfalse == true && midroyalflushfalse == true)
        {
            middleorder = 6;
            middlevalue.text = "Flush";
        }

        if (midstraightcheck == false && midflushchecker == true && midfullhousefalse == true && midfourofkindfalse == true && midstraightflushfalse == true && midroyalflushfalse == true)
        {
            middleorder = 5;
            middlevalue.text = "Straight";
        }

        if (midthreeofkindfalse == false && midstraightcheck == true && midflushchecker == true && midfullhousefalse == true && midfourofkindfalse == true && midstraightflushfalse == true && midroyalflushfalse == true)
        {
            middleorder = 4;
            middlevalue.text = "ThreeOfKind";
        }

        if (midtwopairfalse == false && midthreeofkindfalse == true && midstraightcheck == true && midflushchecker == true && midfullhousefalse == true && midfourofkindfalse == true && midstraightflushfalse == true && midroyalflushfalse == true)
        {
            middleorder = 3;
            middlevalue.text = "TwoPair";
        }

        if (midonepairfalse == false && midtwopairfalse == true && midthreeofkindfalse == true && midstraightcheck == true && midflushchecker == true && midfullhousefalse == true && midfourofkindfalse == true && midstraightflushfalse == true && midroyalflushfalse == true)
        {
            middleorder = 2;
            middlevalue.text = "OnePair";
        }

        else if (midtwopairfalse == true && midonepairfalse == true && midhcard2false == false && midthreeofkindfalse == true && midstraightcheck == true && midflushchecker == true && midfullhousefalse == true && midfourofkindfalse == true && midstraightflushfalse == true && midroyalflushfalse == true)
        {
            middleorder = 1;
            middlevalue.text = "HighCard";
        }
        #endregion

        #region frnt
        if (frntonepairfalse == false)
        {
            frontorder = 2;
            frontvalue.text = "OnePair";
        }

        else if (frnthcard2false == false && frntonepairfalse == true)
        {
            frontorder = 1;
            frontvalue.text = "HighCard";
        }
        #endregion
    }

    public void DoneButton()
    {
      //  soundManager.PlayClick();
        AddCardDetails();
        gameManager.DragDropDone();
    }
    void AddCardDetails()
    {
        cardSpritelist.Clear();
        cardSpritelist.Add(front[0].GetComponent<CardIndex>().spritenum);
        cardSpritelist.Add(front[1].GetComponent<CardIndex>().spritenum);
        cardSpritelist.Add(front[2].GetComponent<CardIndex>().spritenum);

        cardSpritelist.Add(middle[0].GetComponent<CardIndex>().spritenum);
        cardSpritelist.Add(middle[1].GetComponent<CardIndex>().spritenum);
        cardSpritelist.Add(middle[2].GetComponent<CardIndex>().spritenum);
        cardSpritelist.Add(middle[3].GetComponent<CardIndex>().spritenum);
        cardSpritelist.Add(middle[4].GetComponent<CardIndex>().spritenum);

        cardSpritelist.Add(back[0].GetComponent<CardIndex>().spritenum);
        cardSpritelist.Add(back[1].GetComponent<CardIndex>().spritenum);
        cardSpritelist.Add(back[2].GetComponent<CardIndex>().spritenum);
        cardSpritelist.Add(back[3].GetComponent<CardIndex>().spritenum);
        cardSpritelist.Add(back[4].GetComponent<CardIndex>().spritenum);

        for(int i = 0; i< 13; i++)
        {
            Debug.Log("cardspritelist: " + cardSpritelist[i]);
        }
    }
}
