using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotGame;
using UnityEngine.UI;
using System.Linq;

public class PiggyBPlayer : MonoBehaviour
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

    public void Spin()
    {
        totallCoin -= currentBetAmount;
        CoinsText.text = totallCoin.ToString();

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

        Invoke("AfterStop", 2f);

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

        firstRowSymbols.Clear();
        middleRowSymbols.Clear();
        lastRowSymbols.Clear();
    }



    private void MatchingRows()
    {
        // First Row Matching
        if (firstRowSymbols[0] == firstRowSymbols[1] || firstRowSymbols[0] == middleRowSymbols[1] || firstRowSymbols[0] == lastRowSymbols[1])
        {
            if (firstRowSymbols[0] == firstRowSymbols[2] || firstRowSymbols[0] == middleRowSymbols[2] || firstRowSymbols[0] == lastRowSymbols[2])
            {
                print("First 3 " + firstRowSymbols[0]);
                //////////

                switch (firstRowSymbols[0])
                {
                    case "dollar":
                        totallCoin += 100;
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "car":
                        totallCoin += 50;
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "roulette":
                        totallCoin += 30;
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "cash":
                        totallCoin += 25;
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "dice":
                        totallCoin += 15;
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "A":
                        totallCoin += 5;
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "K":
                        totallCoin += 5;
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "Q":
                        totallCoin += 5;
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "J":
                        totallCoin += 5;
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "10":
                        totallCoin += 5;
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    case "9":
                        totallCoin += 5;
                        CoinsText.text = totallCoin.ToString();
                        Debug.Log("switch 123 ");
                        break;
                    default:
                        Debug.Log("switch 123 ");
                        break;
                }


                ////// >> 4
                if (firstRowSymbols[0] == firstRowSymbols[3] || firstRowSymbols[0] == middleRowSymbols[3] || firstRowSymbols[0] == lastRowSymbols[3])
                {
                    print("First 4  " + firstRowSymbols[0]);
                    switch (firstRowSymbols[0])
                    {
                        case "dollar":
                            totallCoin += 100;
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "car":
                            totallCoin += 50;
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "roulette":
                            totallCoin += 45;
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "cash":
                            totallCoin += 25;
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "dice":
                            totallCoin += 10;
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "A":
                            totallCoin += 5;
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "K":
                            totallCoin += 5;
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "Q":
                            totallCoin += 5;
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "J":
                            totallCoin += 5;
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "10":
                            totallCoin += 5;
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        case "9":
                            totallCoin += 5;
                            CoinsText.text = totallCoin.ToString();
                            Debug.Log("switch 123 ");
                            break;
                        default:
                            Debug.Log("switch 123 ");
                            break;
                    }
                    ////// >> 5
                    if (firstRowSymbols[0] == firstRowSymbols[4] || firstRowSymbols[0] == middleRowSymbols[4] || firstRowSymbols[0] == lastRowSymbols[4])
                    {
                        Debug.Log("1 BIG WIN ");
                        switch (firstRowSymbols[0])
                        {
                            case "dollar":
                                totallCoin += 600;
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "car":
                                totallCoin += 300;
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "roulette":
                                totallCoin += 125;
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "cash":
                                totallCoin += 100;
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "dice":
                                totallCoin += 75;
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "A":
                                totallCoin += 10;
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "K":
                                totallCoin += 10;
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "Q":
                                totallCoin += 10;
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "J":
                                totallCoin += 5;
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "10":
                                totallCoin += 5;
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            case "9":
                                totallCoin += 5;
                                CoinsText.text = totallCoin.ToString();
                                Debug.Log("switch 123 ");
                                break;
                            default:
                                Debug.Log("switch 123 ");
                                break;
                        }
                        //CollectBigWin.SetActive(true);
                    }
                }
            }
        }

        // Middle Row Matching
        if (middleRowSymbols[0] == firstRowSymbols[1] || middleRowSymbols[0] == middleRowSymbols[1] || middleRowSymbols[0] == lastRowSymbols[1])
        {
            if (middleRowSymbols[0] == firstRowSymbols[2] || middleRowSymbols[0] == middleRowSymbols[2] || middleRowSymbols[0] == lastRowSymbols[2])
            {
                print("Middle 3 " + middleRowSymbols[0]);
                ////////////

                //////////
                if (middleRowSymbols[0] == firstRowSymbols[3] || middleRowSymbols[0] == middleRowSymbols[3] || middleRowSymbols[0] == lastRowSymbols[3])
                {
                    print("Middle 4  " + middleRowSymbols[0]);

                    if (middleRowSymbols[0] == firstRowSymbols[4] || middleRowSymbols[0] == middleRowSymbols[4] || middleRowSymbols[0] == lastRowSymbols[4])
                    {
                        Debug.Log("2 BIG WIN ");

                        //CollectBigWin.SetActive(true);
                    }
                }
            }
        }

        //Last Row Matching
        if (lastRowSymbols[0] == firstRowSymbols[1] || lastRowSymbols[0] == middleRowSymbols[1] || lastRowSymbols[0] == lastRowSymbols[1])
        {
            if (lastRowSymbols[0] == firstRowSymbols[2] || lastRowSymbols[0] == middleRowSymbols[2] || lastRowSymbols[0] == lastRowSymbols[2])
            {
                print("Last 3 " + lastRowSymbols[0]);

                if (lastRowSymbols[0] == firstRowSymbols[3] || lastRowSymbols[0] == middleRowSymbols[3] || lastRowSymbols[0] == lastRowSymbols[3])
                {
                    print("Last 4  " + middleRowSymbols[0]);

                    if (lastRowSymbols[0] == firstRowSymbols[4] || lastRowSymbols[0] == middleRowSymbols[4] || lastRowSymbols[0] == lastRowSymbols[4])
                    {
                        Debug.Log("3 BIG WIN ");

                        //CollectBigWin.SetActive(true);
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
    [SerializeField] private Text WinFooterText;

    // Button for Collect
    public void CollectingCoins()
    {
        winAmount = 1000;
        totallCoin += winAmount;
        WinFooterText.text = winAmount.ToString();
        CoinsText.text = totallCoin.ToString();
        collectBigWinText.text = winAmount.ToString();
        StartCoroutine(BigWinAnim());
        CollectBigWin.SetActive(false);

    }

    // All of Amounts

    private int totallCoin = 121475;

    [SerializeField] private Text CoinsText;
    [SerializeField] private Text BetAmountText;

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


    public void HoldForAutoSpin(int timesLoop)
    {
        StartCoroutine(AutoSpin(timesLoop));

    }
    public IEnumerator AutoSpin(int timesLoop)
    {
        print("auto " + timesLoop);
        for (int i = 0; i < timesLoop; i++)
        {
            yield return new WaitForSeconds(0.5f);

            Spin();
            yield return new WaitForSeconds(2f);

        }
    }

}
