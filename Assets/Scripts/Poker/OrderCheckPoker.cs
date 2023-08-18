using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class OrderCheckPoker : MonoBehaviour
{
    public List<Image> back = new List<Image>();
    [SerializeField] private Text backvalue;
    [SerializeField] private PokerGameManager gameManager;
    int f1, f2, f3, b1, b2, b3, b4, b5, b6, b7;
    int ftype1, ftype2, ftype3, btype1, btype2, btype3, btype4, btype5, btype6, btype7;
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

    //front bool
    bool frnthcard1false = true;
    bool frnthcard2false = true;
    bool frntonepairfalse = true;
    public int frontorder = 0;
    public int middleorder = 0;
    public int backorder = 0;
    public List<int> cardSpritelist = new List<int>();

    // Update is called once per frame
    void Update()
    {
        CheckOrder();
    }
    void SetValue()
    {

        b1 = back[0].GetComponent<CardData>().index;
        b2 = back[1].GetComponent<CardData>().index;
        b3 = back[2].GetComponent<CardData>().index;
        b4 = back[3].GetComponent<CardData>().index;
        b5 = back[4].GetComponent<CardData>().index;
        b6 = back[5].GetComponent<CardData>().index;
        b7 = back[6].GetComponent<CardData>().index;
        btype1 = back[0].GetComponent<CardData>().cardType;
        btype2 = back[1].GetComponent<CardData>().cardType;
        btype3 = back[2].GetComponent<CardData>().cardType;
        btype4 = back[3].GetComponent<CardData>().cardType;
        btype5 = back[4].GetComponent<CardData>().cardType;
        btype6 = back[5].GetComponent<CardData>().cardType;
        btype7 = back[6].GetComponent<CardData>().cardType;
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
        List<int> backtype = new List<int>();
        if (gameManager.ordrcnt == 1)
        {      
            backcards.Add(b1);
            backcards.Add(b2);

            backtype.Add(btype1);
            backtype.Add(btype2);
        }
        if(gameManager.ordrcnt == 2)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
        }
        if(gameManager.ordrcnt == 3)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
        }
        if (gameManager.ordrcnt == 4)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);
            backcards.Add(b7);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
            backtype.Add(btype7);
        }

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
        CardTextCheck();
    }

    void OnePair()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        List<int> backtype = new List<int>();
        if (gameManager.ordrcnt == 1)
        {
            backcards.Add(b1);
            backcards.Add(b2);

            backtype.Add(btype1);
            backtype.Add(btype2);
        }
        if (gameManager.ordrcnt == 2)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
        }
        if (gameManager.ordrcnt == 3)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
        }
        if (gameManager.ordrcnt == 4)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);
            backcards.Add(b7);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
            backtype.Add(btype7);
        }

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


        CardTextCheck();
    }
    void TwoPair()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        List<int> backtype = new List<int>();
        if (gameManager.ordrcnt == 1)
        {
            backcards.Add(b1);
            backcards.Add(b2);

            backtype.Add(btype1);
            backtype.Add(btype2);
        }
        if (gameManager.ordrcnt == 2)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
        }
        if (gameManager.ordrcnt == 3)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
        }
        if (gameManager.ordrcnt == 4)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);
            backcards.Add(b7);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
            backtype.Add(btype7);
        }

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

        CardTextCheck();
    }

    void ThreeOfKind()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        List<int> backtype = new List<int>();
        if (gameManager.ordrcnt == 1)
        {
            backcards.Add(b1);
            backcards.Add(b2);

            backtype.Add(btype1);
            backtype.Add(btype2);
        }
        if (gameManager.ordrcnt == 2)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
        }
        if (gameManager.ordrcnt == 3)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
        }
        if (gameManager.ordrcnt == 4)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);
            backcards.Add(b7);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
            backtype.Add(btype7);
        }

        int cardcheck = 0;

        for (int i = 0; i < backcards.Count; i++)
        {
            for (int j = i + 1; j < backcards.Count; j++)
            {
                for (int k = j + 1; k < backcards.Count; k++)
                {
                    if (backcards[i] == backcards[j] && backcards[j] == backcards[k])
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

        CardTextCheck();
    }

    void Straight()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        List<int> backtype = new List<int>();
        if (gameManager.ordrcnt == 1)
        {
            backcards.Add(b1);
            backcards.Add(b2);

            backtype.Add(btype1);
            backtype.Add(btype2);
        }
        if (gameManager.ordrcnt == 2)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
        }
        if (gameManager.ordrcnt == 3)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
        }
        if (gameManager.ordrcnt == 4)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);
            backcards.Add(b7);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
            backtype.Add(btype7);
        }

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
        List<int> backtype = new List<int>();
        if (gameManager.ordrcnt == 1)
        {
            backcards.Add(b1);
            backcards.Add(b2);

            backtype.Add(btype1);
            backtype.Add(btype2);
        }
        if (gameManager.ordrcnt == 2)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
        }
        if (gameManager.ordrcnt == 3)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
        }
        if (gameManager.ordrcnt == 4)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);
            backcards.Add(b7);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
            backtype.Add(btype7);
        }

        backcards.Sort();
        IsSequential(backcards);
        bool scheck = IsSequential(backcards);
        // Debug.Log("sequential" + scheck);
        bool flushistrue = false;

        if (scheck)
            flushseqcheck = false;
        else
            flushseqcheck = true;

        if (flushseqcheck)
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
    }
    void FullHouse()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        List<int> backtype = new List<int>();
        if (gameManager.ordrcnt == 1)
        {
            backcards.Add(b1);
            backcards.Add(b2);

            backtype.Add(btype1);
            backtype.Add(btype2);
        }
        if (gameManager.ordrcnt == 2)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
        }
        if (gameManager.ordrcnt == 3)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
        }
        if (gameManager.ordrcnt == 4)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);
            backcards.Add(b7);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
            backtype.Add(btype7);
        }


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
    }
    void FourOfKind()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        List<int> backtype = new List<int>();
        if (gameManager.ordrcnt == 1)
        {
            backcards.Add(b1);
            backcards.Add(b2);

            backtype.Add(btype1);
            backtype.Add(btype2);
        }
        if (gameManager.ordrcnt == 2)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
        }
        if (gameManager.ordrcnt == 3)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
        }
        if (gameManager.ordrcnt == 4)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);
            backcards.Add(b7);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
            backtype.Add(btype7);
        }

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

        CardTextCheck();
    }
    void StraightFlush()
    {
        #region back
        List<int> backcards = new List<int>();
        List<int> backtype = new List<int>();
        if (gameManager.ordrcnt == 1)
        {
            backcards.Add(b1);
            backcards.Add(b2);

            backtype.Add(btype1);
            backtype.Add(btype2);
        }
        if (gameManager.ordrcnt == 2)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
        }
        if (gameManager.ordrcnt == 3)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
        }
        if (gameManager.ordrcnt == 4)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);
            backcards.Add(b7);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
            backtype.Add(btype7);
        }

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

    }
    void RoyalFlush()
    {
        #region back
        //back
        List<int> backcards = new List<int>();
        List<int> backtype = new List<int>();
        if (gameManager.ordrcnt == 1)
        {
            backcards.Add(b1);
            backcards.Add(b2);

            backtype.Add(btype1);
            backtype.Add(btype2);
        }
        if (gameManager.ordrcnt == 2)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
        }
        if (gameManager.ordrcnt == 3)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
        }
        if (gameManager.ordrcnt == 4)
        {
            backcards.Add(b1);
            backcards.Add(b2);
            backcards.Add(b3);
            backcards.Add(b4);
            backcards.Add(b5);
            backcards.Add(b6);
            backcards.Add(b7);

            backtype.Add(btype1);
            backtype.Add(btype2);
            backtype.Add(btype3);
            backtype.Add(btype4);
            backtype.Add(btype5);
            backtype.Add(btype6);
            backtype.Add(btype7);
        }

        List<int> seqchecklist = new List<int>();
        seqchecklist.Add(backcards[1]);
        seqchecklist.Add(backcards[2]);
        seqchecklist.Add(backcards[3]);
        seqchecklist.Add(backcards[4]);
        seqchecklist.Add(backcards[5]);
        seqchecklist.Add(backcards[6]);

        IsSequential(seqchecklist);
        bool scheck = IsSequential(seqchecklist);
        bool rfcheck = false;
        bool rfcheckmain = false;
        if (scheck)
        {
            if (backcards[0] == 1 && seqchecklist[0] == 10)
                rfcheckmain = true;
            else
                rfcheckmain = false;
        }


        if (rfcheckmain)
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
    }

    static bool areConsecutive(int[] arr, int n)
    {
        //Sort the array
        Array.Sort(arr);
        // checking the adjacent elements
        for (int i = 1; i < n; i++)
        {
            if (arr[i] != arr[i - 1] + 1)
            {
                return false;
            }
        }
        return true;
    }
    void CardTextCheck()
    {
        #region main
        if (gameManager.gamebegin)
        {
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
        }
        #endregion
    }

  
    void AddCardDetails()
    {
        //cardSpritelist.Clear();

        cardSpritelist.Add(back[0].GetComponent<CardData>().spritenum);
        cardSpritelist.Add(back[1].GetComponent<CardData>().spritenum);
        cardSpritelist.Add(back[2].GetComponent<CardData>().spritenum);
        cardSpritelist.Add(back[3].GetComponent<CardData>().spritenum);
        cardSpritelist.Add(back[4].GetComponent<CardData>().spritenum);
        cardSpritelist.Add(back[5].GetComponent<CardData>().spritenum);
        cardSpritelist.Add(back[6].GetComponent<CardData>().spritenum);

        for (int i = 0; i < 7; i++)
        {
            Debug.Log("cardspritelist: " + cardSpritelist[i]);
        }
    }
}
