using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotGame;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ChristmasPlayer : MonoBehaviour
{
    public List<Sprite> symbols = new List<Sprite>();
    public List<int> reelData = new List<int>();
    public SpriteMask bounds;


    //public Toggle stopOrdinaryToggle;
    private Machine machine;
    //private Reel reel;

    void Start()
    {
        machine = Machine.CreateAndInit(new Vector2Int(5, 3), 1f, bounds.bounds);
        machine.OnReelRenderUpdated = OnReelRenderUpdated;
        //Reel Cursor Randomize();
        for (int i = 0; i < machine.reels.Count; i++)
        {
            machine.reels[i].SetCursorAll(Random.Range(0, reelData.Count));
        }

        machine.RefreshAll();
    }

    public void AutoSpin()
    {
        totallCoin -= currentBetAmount;
        CoinsText.text = totallCoin.ToString();
        WinFooterText.text = "0";

        if (machine.isSpinReady)
        {
            machine.SpinAll();
        }
        Invoke("Stop", 0.1f);

    }

    public void Spin()
    {
        Spinb.SetActive(false);
        SpinDSel.SetActive(true);
        totallCoin -= currentBetAmount;
        CoinsText.text = totallCoin.ToString();
        WinFooterText.text = "0";

        if (machine.isSpinReady)
        {
            machine.SpinAll();
        }
        Invoke("Stop", 0.1f);
    }

    public void Stop()
    {

        if (!machine.isStopReady)
            return;

        StartCoroutine(CoStopOrdinary());

        /*if (stopOrdinaryToggle.isOn)
        {
            //Stop Ordinary
            StartCoroutine(CoStopOrdinary());
        }
        else
        {
            //Stop Instant
            int[] cursors = GetCursors();
            machine.StopAll(cursors);
        }*/
    }
    IEnumerator CoStopOrdinary()
    {
        int[] cursors = GetCursors();
        for (int i = 0; i < machine.reels.Count; i++)
        {
            machine.reels[i].Stop(cursors[i]);
            yield return new WaitForSeconds(0.5f);

        }
        //yield return new WaitForSeconds(2f);
        //print("AaterStop2");
        //AfterStop();

        Invoke("AfterStop", 0.5f);

    }
    //public Action OnSpinEnded;
    void OnReelRenderUpdated(Reel reel, int reelIndex, SpriteRenderer renderer, int cursor)
    {
        cursor = cursor < 0 ? reelData.Count + cursor : cursor;
        var symbol = symbols[reelData[cursor % reelData.Count]];

        renderer.sprite = symbol;
    }

    private int[] GetCursors()
    {
        int[] cursors = new int[machine.reels.Count];
        for (int i = 0; i < machine.reels.Count; i++)
        {
            cursors[i] = Random.Range(0, reelData.Count);
        }
        return cursors;
    }


    private List<string> firstRowSymbols = new List<string>();
    private List<string> middleRowSymbols = new List<string>();
    private List<string> lastRowSymbols = new List<string>();

    private void AfterStop()
    {
        foreach (Reel reel in machine.reels)
        {

            //reel.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = symbols[buttonClicked];
            string firstSymbols = reel.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite.name;
            string middleValue = reel.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite.name;
            string lastSymbols = reel.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite.name;

            Debug.Log("*First* " + firstSymbols);
            Debug.Log("*Midle* " + middleValue);
            Debug.Log("*Last* " + lastSymbols);

            firstRowSymbols.Add(firstSymbols);
            middleRowSymbols.Add(middleValue);
            lastRowSymbols.Add(lastSymbols);

        }
        MatchingRows();
        Debug.Log("P: " + payValue);
        if (string.IsNullOrEmpty(payValue)){
            WinFooterText.text = "0";
        }
        else{
            WinFooterText.text = payValue;
        } 

        firstRowSymbols.Clear();
        middleRowSymbols.Clear();
        lastRowSymbols.Clear();
        
        Spinb.SetActive(true);
        SpinDSel.SetActive(false);
    }

    public PayValues DPayValues;
    string payValue;

    private void MatchingRows()
    {
        payValue = DPayValues.StringStringDictionary["empty"];
        // First Row Matching
        //for (int i = 1; i < 5; i++)
        //{
        //    if (firstRowSymbols[0] == firstRowSymbols[i])
        //    {

        //    }
        //}

        // >> 3th row Matching
        if (firstRowSymbols[0] == firstRowSymbols[1] || firstRowSymbols[0] == middleRowSymbols[1] || firstRowSymbols[0] == lastRowSymbols[1])
        {
            print("First 3 " + firstRowSymbols[0]);
            // Switch for 2 Matching
            switch (firstRowSymbols[0])
            {
                case "Icon1":
                    payValue = DPayValues.StringStringDictionary["Icon1-2"];
                    totallCoin += int.Parse(payValue);
                    CoinsText.text = totallCoin.ToString();
                    Debug.Log("switch 123 ");
                    break;
                case "Icon2":
                    payValue = DPayValues.StringStringDictionary["Icon2-2"];
                    Debug.Log("switch payValue " + payValue);
                    totallCoin += int.Parse(payValue);
                    Debug.Log("switch totallCoin " + totallCoin);
                    CoinsText.text = totallCoin.ToString();
                    Debug.Log("switch 123 ");
                    break;
                case "Icon3":
                    payValue = DPayValues.StringStringDictionary["Icon3-2"];
                    totallCoin += int.Parse(payValue);
                    CoinsText.text = totallCoin.ToString();
                    Debug.Log("switch 123 ");
                    break;
                case "Icon4":
                    payValue = DPayValues.StringStringDictionary["Icon4-2"];
                    totallCoin += int.Parse(payValue);
                    CoinsText.text = totallCoin.ToString();
                    Debug.Log("switch 123 ");
                    break;
                case "Icon5":
                    payValue = DPayValues.StringStringDictionary["Icon5-2"];
                    totallCoin += int.Parse(payValue);
                    CoinsText.text = totallCoin.ToString();
                    Debug.Log("switch 123 ");
                    break;
                case "Icon6":
                    payValue = DPayValues.StringStringDictionary["Icon6-2"];
                    totallCoin += int.Parse(payValue);
                    CoinsText.text = totallCoin.ToString();
                    Debug.Log("switch 123 ");
                    break;
                default:
                    Debug.Log("switch default ");
                    break;
            }


            // >> 3th row Matching
            if (firstRowSymbols[0] == firstRowSymbols[2] || firstRowSymbols[0] == middleRowSymbols[2] || firstRowSymbols[0] == lastRowSymbols[2])
            {
                print("First 3 " + firstRowSymbols[0]);
                // Switch for 3 Matching
                switch (firstRowSymbols[0])
                {
                    case "Icon1":
                        payValue = DPayValues.StringStringDictionary["Icon1-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon2":
                        payValue = DPayValues.StringStringDictionary["Icon2-3"];
                        Debug.Log("switch payValue " + payValue);
                        totallCoin += int.Parse(payValue);
                        Debug.Log("switch totallCoin " + totallCoin);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon3":
                        payValue = DPayValues.StringStringDictionary["Icon3-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon4":
                        payValue = DPayValues.StringStringDictionary["Icon4-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon5":
                        payValue = DPayValues.StringStringDictionary["Icon5-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon6":
                        payValue = DPayValues.StringStringDictionary["Icon6-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "A":
                        payValue = DPayValues.StringStringDictionary["A-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "K":
                        payValue = DPayValues.StringStringDictionary["K-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Q":
                        payValue = DPayValues.StringStringDictionary["Q-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "J":
                        payValue = DPayValues.StringStringDictionary["J-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "10":
                        payValue = DPayValues.StringStringDictionary["10-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "9":
                        payValue = DPayValues.StringStringDictionary["9-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    default:
                        Debug.Log("switch default ");
                        break;
                }


                // >> 4th row Matching
                if (firstRowSymbols[0] == firstRowSymbols[3] || firstRowSymbols[0] == middleRowSymbols[3] || firstRowSymbols[0] == lastRowSymbols[3])
                {
                    print("First 4  " + firstRowSymbols[0]);
                    // Switch for 4 Matching
                    switch (firstRowSymbols[0])
                    {
                        case "Icon1":
                            payValue = DPayValues.StringStringDictionary["Icon1-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon2":
                            payValue = DPayValues.StringStringDictionary["Icon2-4"];
                            Debug.Log("switch payValue " + payValue);
                            totallCoin += int.Parse(payValue);
                            Debug.Log("switch totallCoin " + totallCoin);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon3":
                            payValue = DPayValues.StringStringDictionary["Icon3-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon4":
                            payValue = DPayValues.StringStringDictionary["Icon4-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon5":
                            payValue = DPayValues.StringStringDictionary["Icon5-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon6":
                            payValue = DPayValues.StringStringDictionary["Icon6-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "A":
                            payValue = DPayValues.StringStringDictionary["A-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "K":
                            payValue = DPayValues.StringStringDictionary["K-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Q":
                            payValue = DPayValues.StringStringDictionary["Q-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "J":
                            payValue = DPayValues.StringStringDictionary["J-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "10":
                            payValue = DPayValues.StringStringDictionary["10-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "9":
                            payValue = DPayValues.StringStringDictionary["9-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        default:
                            Debug.Log("switch default ");
                            break;
                    }

                    // >> 5th row Matching
                    if (firstRowSymbols[0] == firstRowSymbols[4] || firstRowSymbols[0] == middleRowSymbols[4] || firstRowSymbols[0] == lastRowSymbols[4])
                    {
                        Debug.Log("1 BIG WIN ");
                        // Switch for 5 Matching
                        switch (firstRowSymbols[0])
                        {
                            case "Icon1":
                                payValue = DPayValues.StringStringDictionary["Icon1-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon2":
                                payValue = DPayValues.StringStringDictionary["Icon2-5"];
                                Debug.Log("switch payValue " + payValue);
                                totallCoin += int.Parse(payValue);
                                Debug.Log("switch totallCoin " + totallCoin);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon3":
                                payValue = DPayValues.StringStringDictionary["Icon3-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon4":
                                payValue = DPayValues.StringStringDictionary["Icon4-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon5":
                                payValue = DPayValues.StringStringDictionary["Icon5-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon6":
                                payValue = DPayValues.StringStringDictionary["Icon6-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "A":
                                payValue = DPayValues.StringStringDictionary["A-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "K":
                                payValue = DPayValues.StringStringDictionary["K-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Q":
                                payValue = DPayValues.StringStringDictionary["Q-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "J":
                                payValue = DPayValues.StringStringDictionary["J-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "10":
                                payValue = DPayValues.StringStringDictionary["10-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "9":
                                payValue = DPayValues.StringStringDictionary["9-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            default:
                                Debug.Log("switch default ");
                                break;
                        }

                        CollectBigWin.SetActive(true);
                    }
                }
            }
        }

        // Middle Row Matching
        else if (middleRowSymbols[0] == firstRowSymbols[1] || middleRowSymbols[0] == middleRowSymbols[1] || middleRowSymbols[0] == lastRowSymbols[1])
        {

            // >> 3th row Matching
            if (middleRowSymbols[0] == firstRowSymbols[2] || middleRowSymbols[0] == middleRowSymbols[2] || middleRowSymbols[0] == lastRowSymbols[2])
            {
                print("Middle 3 " + middleRowSymbols[0]);
                // Switch for 3 Matching
                switch (middleRowSymbols[0])
                {
                    case "Icon1":
                        payValue = DPayValues.StringStringDictionary["Icon1-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon2":
                        payValue = DPayValues.StringStringDictionary["Icon2-3"];
                        Debug.Log("switch payValue " + payValue);
                        totallCoin += int.Parse(payValue);
                        Debug.Log("switch totallCoin " + totallCoin);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon3":
                        payValue = DPayValues.StringStringDictionary["Icon3-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon4":
                        payValue = DPayValues.StringStringDictionary["Icon4-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon5":
                        payValue = DPayValues.StringStringDictionary["Icon5-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon6":
                        payValue = DPayValues.StringStringDictionary["Icon6-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "A":
                        payValue = DPayValues.StringStringDictionary["A-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "K":
                        payValue = DPayValues.StringStringDictionary["K-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Q":
                        payValue = DPayValues.StringStringDictionary["Q-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "J":
                        payValue = DPayValues.StringStringDictionary["J-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "10":
                        payValue = DPayValues.StringStringDictionary["10-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "9":
                        payValue = DPayValues.StringStringDictionary["9-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    default:
                        Debug.Log("switch default ");
                        break;
                }


                // >> 4th row Matching
                if (middleRowSymbols[0] == firstRowSymbols[3] || middleRowSymbols[0] == middleRowSymbols[3] || middleRowSymbols[0] == lastRowSymbols[3])
                {
                    print("Middle 4  " + middleRowSymbols[0]);
                    // Switch for 4 Matching
                    switch (middleRowSymbols[0])
                    {
                        case "Icon1":
                            payValue = DPayValues.StringStringDictionary["Icon1-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon2":
                            payValue = DPayValues.StringStringDictionary["Icon2-4"];
                            Debug.Log("switch payValue " + payValue);
                            totallCoin += int.Parse(payValue);
                            Debug.Log("switch totallCoin " + totallCoin);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon3":
                            payValue = DPayValues.StringStringDictionary["Icon3-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon4":
                            payValue = DPayValues.StringStringDictionary["Icon4-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon5":
                            payValue = DPayValues.StringStringDictionary["Icon5-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon6":
                            payValue = DPayValues.StringStringDictionary["Icon6-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "A":
                            payValue = DPayValues.StringStringDictionary["A-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "K":
                            payValue = DPayValues.StringStringDictionary["K-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Q":
                            payValue = DPayValues.StringStringDictionary["Q-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "J":
                            payValue = DPayValues.StringStringDictionary["J-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "10":
                            payValue = DPayValues.StringStringDictionary["10-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "9":
                            payValue = DPayValues.StringStringDictionary["9-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        default:
                            Debug.Log("switch default ");
                            break;
                    }


                    // >> 5th row Matching
                    if (middleRowSymbols[0] == firstRowSymbols[4] || middleRowSymbols[0] == middleRowSymbols[4] || middleRowSymbols[0] == lastRowSymbols[4])
                    {
                        Debug.Log("2 BIG WIN ");
                        // Switch for 5 Matching
                        switch (middleRowSymbols[0])
                        {
                            case "Icon1":
                                payValue = DPayValues.StringStringDictionary["Icon1-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon2":
                                payValue = DPayValues.StringStringDictionary["Icon2-5"];
                                Debug.Log("switch payValue " + payValue);
                                totallCoin += int.Parse(payValue);
                                Debug.Log("switch totallCoin " + totallCoin);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon3":
                                payValue = DPayValues.StringStringDictionary["Icon3-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon4":
                                payValue = DPayValues.StringStringDictionary["Icon4-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon5":
                                payValue = DPayValues.StringStringDictionary["Icon5-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon6":
                                payValue = DPayValues.StringStringDictionary["Icon6-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "A":
                                payValue = DPayValues.StringStringDictionary["A-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "K":
                                payValue = DPayValues.StringStringDictionary["K-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Q":
                                payValue = DPayValues.StringStringDictionary["Q-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "J":
                                payValue = DPayValues.StringStringDictionary["J-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "10":
                                payValue = DPayValues.StringStringDictionary["10-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "9":
                                payValue = DPayValues.StringStringDictionary["9-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            default:
                                Debug.Log("switch default ");
                                break;
                        }

                        CollectBigWin.SetActive(true);
                    }
                }
            }
        }

        //Last Row Matching
        else if (lastRowSymbols[0] == firstRowSymbols[1] || lastRowSymbols[0] == middleRowSymbols[1] || lastRowSymbols[0] == lastRowSymbols[1])
        {
            // >> 3th row Matching
            if (lastRowSymbols[0] == firstRowSymbols[2] || lastRowSymbols[0] == middleRowSymbols[2] || lastRowSymbols[0] == lastRowSymbols[2])
            {
                print("Last 3 " + lastRowSymbols[0]);
                // Switch for 3 Matching
                switch (lastRowSymbols[0])
                {
                    case "Icon1":
                        payValue = DPayValues.StringStringDictionary["Icon1-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon2":
                        payValue = DPayValues.StringStringDictionary["Icon2-3"];
                        Debug.Log("switch payValue " + payValue);
                        totallCoin += int.Parse(payValue);
                        Debug.Log("switch totallCoin " + totallCoin);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon3":
                        payValue = DPayValues.StringStringDictionary["Icon3-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon4":
                        payValue = DPayValues.StringStringDictionary["Icon4-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon5":
                        payValue = DPayValues.StringStringDictionary["Icon5-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Icon6":
                        payValue = DPayValues.StringStringDictionary["Icon6-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "A":
                        payValue = DPayValues.StringStringDictionary["A-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "K":
                        payValue = DPayValues.StringStringDictionary["K-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Q":
                        payValue = DPayValues.StringStringDictionary["Q-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "J":
                        payValue = DPayValues.StringStringDictionary["J-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "10":
                        payValue = DPayValues.StringStringDictionary["10-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "9":
                        payValue = DPayValues.StringStringDictionary["9-3"];
                        totallCoin += int.Parse(payValue);
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    default:
                        Debug.Log("switch default ");
                        break;
                }


                // >> 4th row Matching
                if (lastRowSymbols[0] == firstRowSymbols[3] || lastRowSymbols[0] == middleRowSymbols[3] || lastRowSymbols[0] == lastRowSymbols[3])
                {
                    print("Last 4  " + lastRowSymbols[0]);
                    // Switch for 4 Matching
                    switch (lastRowSymbols[0])
                    {
                        case "Icon1":
                            payValue = DPayValues.StringStringDictionary["Icon1-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon2":
                            payValue = DPayValues.StringStringDictionary["Icon2-4"];
                            Debug.Log("switch payValue " + payValue);
                            totallCoin += int.Parse(payValue);
                            Debug.Log("switch totallCoin " + totallCoin);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon3":
                            payValue = DPayValues.StringStringDictionary["Icon3-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon4":
                            payValue = DPayValues.StringStringDictionary["Icon4-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon5":
                            payValue = DPayValues.StringStringDictionary["Icon5-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Icon6":
                            payValue = DPayValues.StringStringDictionary["Icon6-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "A":
                            payValue = DPayValues.StringStringDictionary["A-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "K":
                            payValue = DPayValues.StringStringDictionary["K-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Q":
                            payValue = DPayValues.StringStringDictionary["Q-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "J":
                            payValue = DPayValues.StringStringDictionary["J-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "10":
                            payValue = DPayValues.StringStringDictionary["10-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "9":
                            payValue = DPayValues.StringStringDictionary["9-4"];
                            totallCoin += int.Parse(payValue);
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        default:
                            Debug.Log("switch default ");
                            break;
                    }


                    // >> 5th row Matching
                    if (lastRowSymbols[0] == firstRowSymbols[4] || lastRowSymbols[0] == middleRowSymbols[4] || lastRowSymbols[0] == lastRowSymbols[4])
                    {
                        Debug.Log("3 BIG WIN ");
                        // Switch for 5 Matching
                        switch (lastRowSymbols[0])
                        {
                            case "Icon1":
                                payValue = DPayValues.StringStringDictionary["Icon1-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon2":
                                payValue = DPayValues.StringStringDictionary["Icon2-5"];
                                Debug.Log("switch payValue " + payValue);
                                totallCoin += int.Parse(payValue);
                                Debug.Log("switch totallCoin " + totallCoin);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon3":
                                payValue = DPayValues.StringStringDictionary["Icon3-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon4":
                                payValue = DPayValues.StringStringDictionary["Icon4-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon5":
                                payValue = DPayValues.StringStringDictionary["Icon5-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Icon6":
                                payValue = DPayValues.StringStringDictionary["Icon6-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "A":
                                payValue = DPayValues.StringStringDictionary["A-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "K":
                                payValue = DPayValues.StringStringDictionary["K-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Q":
                                payValue = DPayValues.StringStringDictionary["Q-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "J":
                                payValue = DPayValues.StringStringDictionary["J-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "10":
                                payValue = DPayValues.StringStringDictionary["10-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "9":
                                payValue = DPayValues.StringStringDictionary["9-5"];
                                totallCoin += int.Parse(payValue);
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            default:
                                Debug.Log("switch default ");
                                break;
                        }

                        CollectBigWin.SetActive(true);
                    }
                }
            }
        }

    }

    [SerializeField] private GameObject CollectBigWin;
    [SerializeField] private GameObject BigWin;

    [SerializeField] private Text bigWinText;
    private IEnumerator BigWinAnim()
    {
        bigWinText.text = winAmount.ToString();
        //Wait for 14 secs.
        yield return new WaitForSeconds(0.1f);
        //Turn My game object that is set to false(off) to True(on).
        BigWin.SetActive(true);

        //Turn the Game Oject back off after 1 sec.
        yield return new WaitForSeconds(1.5f);

        //Game object will turn off
        BigWin.SetActive(false);

        yield return new WaitForSeconds(3);
    }

    private int winAmount;
    [SerializeField] private Text collectBigWinText;
    [SerializeField] public Text WinFooterText;

    // Button for Collect
    public void CollectingCoins()
    {
        winAmount = 500;
        totallCoin += winAmount;
        WinFooterText.text = winAmount.ToString();
        CoinsText.text = totallCoin.ToString();
        collectBigWinText.text = winAmount.ToString();
        StartCoroutine(BigWinAnim());
        CollectBigWin.SetActive(false);

    }

    // All of Amounts

    private int totallCoin = 12000;

    [SerializeField] private Text CoinsText;
    [SerializeField] private Text BetAmountText;
    [SerializeField] private GameObject Spinb;
    [SerializeField] private GameObject Stopb;
    [SerializeField] private GameObject SpinDSel;
    [SerializeField] private Text SpinLeft;

    private int[] betAmountArray = new int[6] { 100, 200, 500, 1000, 1500, 3000 };

    int currentBetIndex = 2;
    int currentBetAmount = 500;

    public void PlusBetButton()
    {
        if (currentBetIndex >= betAmountArray.Length)
            return;

        currentBetAmount = betAmountArray[currentBetIndex++];
        BetAmountText.text = currentBetAmount.ToString();

    }

    public void MinusBetButton()
    {
        if (currentBetIndex <= 0)
            return;

        currentBetAmount = betAmountArray[--currentBetIndex];
        BetAmountText.text = currentBetAmount.ToString();

    }

    public void MaxBetAmount()
    {
        currentBetIndex = betAmountArray.Length - 1;
        currentBetAmount = betAmountArray[currentBetIndex];
        BetAmountText.text = betAmountArray[currentBetIndex].ToString();

    }

    Coroutine myCoroutine;
    
    public void StopAutoSpin()
    {
        StopCoroutine(myCoroutine);
        Spinb.SetActive(true);
        Stopb.SetActive(false);

    }

    public void HoldForAutoSpin(int timesLoop)
    {
        myCoroutine = StartCoroutine(AutoSpin(timesLoop));
        Spinb.SetActive(false);
        Stopb.SetActive(true);

    }

    public IEnumerator AutoSpin(int timesLoop)
    {
        print("auto " + timesLoop);
        for (int i = 0; i < timesLoop; timesLoop--)
        {
            yield return new WaitForSeconds(0.5f);
            Spin();
            int Spinleftcheck = timesLoop - 1; 
            SpinLeft.text = Spinleftcheck.ToString();
            yield return new WaitForSeconds(4f);
            Debug.Log("Times " + timesLoop);
        }
        Spinb.SetActive(true);
        Stopb.SetActive(false);
    }

    // button to quit game
    public void QuitGame()
    {
        SceneManager.LoadScene("LoadingHomeScreen");

    }

}
