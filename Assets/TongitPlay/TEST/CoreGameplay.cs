using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class CoreGameplay : MonoBehaviourPunCallbacks

{
    //private PunTurnManager turnManager;
    public bool StartGame;
    public List<GameObject> StructuredDeck;
    public List<GameObject> ShuffledDeck;
    public List<GameObject> PoolDeck;
    public List<GameObject> DiscardDeck;
    public List<string> iStihCards;
    public List<string> AllPlayersNames;
    public List<string> LocalCards;
    public List<string> TableCards;
    public Text textToListAllPlayersInRoom;
    public Text MyCardsText;
    public Text userName;
    bool checkMasterClient;
    public string cardsNames;
    public string cardsToSend;
    public GameObject Hand;
    public GameObject cardPrefab;
    public Transform LocalHand;
    public GameObject iStih;
    public bool InitialisedFirstTime;

    public GameObject TableCardRPC;

    public GameObject OpenUpButton;
    public GameObject ClearOpeningSelection;

    public GameObject PoolPileIndicator;
    public GameObject DiscardPileIndicator;
    //TURN MANAGMENT
    public GameObject MasterIndicator;
    public GameObject TurnIndicator;
    Player[] AllPlayers;
    public Player ActivePlayer;
    public Player NextPlayer;
    public int ActivePlayerIndex;
    public int PlayersInGame;
    public Text InGameText;
    public Text LocalMsg;
    public InputField ChatInput;
    public string LocalUsername;
    //INGAME VARIABLES
    public int TurnTime;
    public bool isMyTurn;
    public bool PoolCardDrawn;
    public bool HandCardDiscarded;
    public bool isOpen;
    public Text OpeningSelectionValueText;
    public int OpeningSelectionValue;
    //public Transform LeftPlayerStih;
    public Transform LocalPlayerStih;
    public Transform CenterPlayerStih;
    public Transform RightPlayerStih;
    public GameObject StihPrefab;
    private int signToCheck;
    private int valueToCheck;
    private int differentSignsCount;
    public List<int> valuesToCheckIncremental;
    public Slider LocalTurnSlider;
    public bool IncrementStih;
    public bool SignStih;
    public List<Sprite> LocalCardDesign;
    public List<Image> CardDesignImages;
    public Image LocalAvatar;
    public List<Sprite> Avatars;
    public Transform Table;
    public bool masterCliendIntialized;
    public bool initialiseData;
    public void GameStart()
    {
        StartGame = true;
        InitializePlayersRPC();
    }
    public GameObject WinPanel;
    public GameObject LosePanel;
    public void ShuffleDeck()
    {
        int numberOfCards = StructuredDeck.Count;
        for (int i = 0; i <= numberOfCards - 1; i++)
        {
            int RandomCard = UnityEngine.Random.Range(0, StructuredDeck.Count - 1);
            ShuffledDeck.Add(StructuredDeck[RandomCard]);
            StructuredDeck.RemoveAt(RandomCard);
        }
    }
    public void AreWeMaster()
    {
        if (checkMasterClient && StartGame)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //MasterIndicator.SetActive(true);
                if (!masterCliendIntialized)
                {
                    StartCoroutine(TurnSwitch());
                    masterCliendIntialized = true;
                }
            }
            else
            {
                MasterIndicator.SetActive(false);
            }
        }
    }


    // Start
    private void Start()
    {
        StartCoroutine(InitialWait());
        //Init();
    }

    public void Init()
    {
        //SetLocalAvatar();
        checkMasterClient = true;
        //populateAllPlayersList();
        ShuffleDeck();
        //SetLocalCardDesign();
        GameStart();

    }
    #region Connection
    //public void ConnectToMasterSrv()
    //{
    //    if (userName.text != "")
    //    {
    //        //PhotonNetwork.AuthValues = new AuthenticationValues();
    //        //PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
    //        //PhotonNetwork.AuthValues.AddAuthParameter("user", userName.text);
    //        //PhotonNetwork.AuthValues.AddAuthParameter("Nickname", userName.text);
    //        //PhotonNetwork.NickName = userName.text;
    //        //PhotonNetwork.AuthValues.UserId = userName.text;
    //        PhotonNetwork.ConnectUsingSettings();//("v1");
    //    }
    //    else
    //    {
    //        Debug.Log("You need to provide a username");
    //    }

    //}

    //public override void OnConnectedToMaster()
    //{
    //    Debug.Log("Connected!");
    //    RoomOptions roomOptions = new RoomOptions();
    //    roomOptions.IsVisible = true;
    //    roomOptions.MaxPlayers = 3;
    //    PhotonNetwork.JoinOrCreateRoom("MyMatch", roomOptions, TypedLobby.Default);
    //    checkMasterClient = true;
    //    populateAllPlayersList();

    //}
    #endregion

    IEnumerator InitialWait()
    {
        //PhotonNetwork.NickName = GameObject.FindGameObjectWithTag("UserData").GetComponent<UserData>().UserName;
        //SetLocalAvatar();
        yield return new WaitForSeconds(5);
        checkMasterClient = true;
        populateAllPlayersList();
        photonView.RPC("StopLoadingScreen", RpcTarget.All);
        ShuffleDeck();
        //SetLocalCardDesign();


        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            CallGetCardsRPC();
            GameStart();

        }
    }
    public GameObject LoadingScreen;
    [PunRPC]
    public void StopLoadingScreen()
    {
        LoadingScreen.SetActive(false);
    }
    public List<GameObject> DeckToReshufle;
    [PunRPC]
    public void Reshuffle()
    {
        if (ShuffledDeck.Count == 0)
        {
            foreach (Transform card in Table)
            {
                DeckToReshufle.Add(card.gameObject);
                Debug.Log("Card added to DeckToReshufle: " + card.gameObject.name);
            }
            ShuffledDeck.Clear();
            int numberOfCards = DeckToReshufle.Count;
            for (int i = 0; i <= numberOfCards - 1; i++)
            {
                int RandomCard = UnityEngine.Random.Range(0, DeckToReshufle.Count - 1);
                ShuffledDeck.Add(DeckToReshufle[RandomCard]);
                DeckToReshufle.RemoveAt(RandomCard);
            }
            PoolDeck = ShuffledDeck;
            photonView.RPC("ClearTable", RpcTarget.All);
        }
    }
    [PunRPC]
    public void ClearTable()
    {
        foreach (Transform card in Table)
        {
            card.transform.SetParent(GameObject.FindGameObjectWithTag("Garbage").transform);
            Debug.Log("ClearTable");
        }
    }

    public Image LoserPanelWinnerAvatar;
    public Text LoserPanelWinnerUsername;
    [PunRPC]
    public void isGameOver(string Username, int WinnerAvatarIndex)
    {
        //LosePanel.SetActive(true);
        LoserPanelWinnerAvatar.sprite = Avatars[WinnerAvatarIndex - 1];
        LoserPanelWinnerUsername.text = Username;
    }

    #region PUNS
    [PunRPC]
    public void GetCardsRPC(string cards)
    {
        MyCardsText.text = "";
        MyCardsText.text = cards;
        foreach (string myString in cards.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
        {
            LocalCards.Add(myString);
            Instantiate(cardPrefab, LocalHand);
        }
    }
    [PunRPC]
    public void SendStih(string s, PhotonMessageInfo info)
    {

        if (s == "stih")
        {
            Instantiate(iStih, GameObject.Find(info.Sender.NickName).transform.GetChild(0).transform);
        }
        else
        {
            iStihCards.Add(s);
            Instantiate(cardPrefab, GameObject.Find(info.Sender.NickName).transform.GetChild(0).transform.GetChild(GameObject.Find(info.Sender.NickName).transform.GetChild(0).childCount - 1));
        }
    }
    [PunRPC]
    public void SendStihRemote(string s, string username, PhotonMessageInfo info)
    {

        if (s == "stih")
        {
            Debug.Log("Stih instantiated!");
            Instantiate(iStih, GameObject.Find(username).transform.GetChild(0).transform);
        }
        else
        {
            iStihCards.Add(s);
            Debug.Log("Card in stih: " + s);
            Instantiate(cardPrefab, GameObject.Find(username).transform.GetChild(0).transform.GetChild(GameObject.Find(username).transform.GetChild(0).childCount - 1));
        }
    }
    [PunRPC]
    public void SetActivePlayerBoard(string activeuser)
    {
        Debug.Log("SetActivePlayerBoard CALLED!");
        Debug.Log("ActiveUser: " + activeuser);
        foreach (string username in ActivePlayerUsernames)
        {
            Debug.Log("Username: " + activeuser);
            if (username == activeuser)
            {
                GameObject.Find(activeuser).transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                GameObject.Find(username).transform.GetChild(1).gameObject.SetActive(false);
            }
        }

    }

    public Sprite remoteAvatar;
    [PunRPC]
    public void SetRemoteAvatars(string avatarName, PhotonMessageInfo info)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.NickName == info.Sender.NickName)
            {
                foreach (Sprite s in Avatars)
                {
                    if (s.name == avatarName)
                    {
                        remoteAvatar = s;
                    }
                }
                GameObject.Find(info.Sender.NickName).transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = remoteAvatar;
            }
        }
    }
    public Sprite localAvatar;
    public void SetLocalAvatar()
    {
        LocalAvatar.sprite = Avatars[GameObject.Find("UserData").GetComponent<UserData>().Avatar - 1];
    }

    public GameObject PlayerOnStihDelete;
    [PunRPC]
    public void DeleteStihsOnPlayer(string username)
    {
        PlayerOnStihDelete = GameObject.Find(username);
        foreach (Transform t in PlayerOnStihDelete.transform.GetChild(0))
        {
            Destroy(t.gameObject);
        }
    }

    [PunRPC]
    public void SetNextPlayerTurnRPC(bool myturn)
    {

        isMyTurn = myturn;
        PoolCardDrawn = false;
        HandCardDiscarded = false;
        LocalTurnSlider.maxValue = TurnTime;
        LocalTurnSlider.value = TurnTime;
        TableCardRPC.SetActive(false);
        if (myturn)
        {
            PoolPileIndicator.SetActive(true);
        }

    }
    public GameObject LocalPlayer;
    //public GameObject LeftPlayer;
    public GameObject CenterPlayer;
    public GameObject RightPlayer;

    public List<string> ActivePlayerUsernames;


    [PunRPC]
    public void InitializePlayers(int playersInGame, PhotonMessageInfo info)
    {
        if (playersInGame == 1)
        {
            LocalPlayer.transform.Find("LocalPlayerUsername").gameObject.GetComponent<Text>().text = PhotonNetwork.NickName;
            LocalPlayer.name = PhotonNetwork.NickName;
            ActivePlayerUsernames.Add(PhotonNetwork.NickName);
            RightPlayer.SetActive(false);
            CenterPlayer.SetActive(false);
        }
        if (playersInGame == 2)
        {
            LocalPlayer.transform.Find("LocalPlayerUsername").gameObject.GetComponent<Text>().text = PhotonNetwork.NickName;
            LocalPlayer.name = PhotonNetwork.NickName;
            RightPlayer.transform.Find("RightPlayerUsername").gameObject.GetComponent<Text>().text = PhotonNetwork.PlayerListOthers[0].NickName;
            RightPlayer.name = PhotonNetwork.PlayerListOthers[0].NickName;
            ActivePlayerUsernames.Add(PhotonNetwork.NickName);
            ActivePlayerUsernames.Add(PhotonNetwork.PlayerListOthers[0].NickName);
            CenterPlayer.SetActive(false);
        }
        if (playersInGame == 3)
        {
            LocalPlayer.transform.Find("LocalPlayerUsername").gameObject.GetComponent<Text>().text = PhotonNetwork.NickName;
            LocalPlayer.name = PhotonNetwork.NickName;
            RightPlayer.transform.Find("RightPlayerUsername").gameObject.GetComponent<Text>().text = PhotonNetwork.PlayerListOthers[0].NickName;
            RightPlayer.name = PhotonNetwork.PlayerListOthers[0].NickName;
            CenterPlayer.transform.Find("CenterPlayerUsername").gameObject.GetComponent<Text>().text = PhotonNetwork.PlayerListOthers[1].NickName;
            CenterPlayer.name = PhotonNetwork.PlayerListOthers[1].NickName;
            if (PhotonNetwork.IsMasterClient)
            {
                ActivePlayerUsernames.Add(PhotonNetwork.NickName);
                ActivePlayerUsernames.Add(PhotonNetwork.PlayerListOthers[0].NickName);
                ActivePlayerUsernames.Add(PhotonNetwork.PlayerListOthers[1].NickName);
                foreach (var item in ActivePlayerUsernames)
                {
                    photonView.RPC("SetNameToAll", RpcTarget.Others,item);
                }
            }
            gameOff = false;
        }

        photonView.RPC("SetRemoteAvatars", RpcTarget.Others, LocalAvatar.sprite.name);
    }

    [PunRPC]
    public void SetNameToAll(string name)
    {
        ActivePlayerUsernames.Add(name);
    }

    [PunRPC]
    public void GetPoolCardRPC(string poolcard)
    {
        LocalCards.Add(poolcard);
        Instantiate(cardPrefab, LocalHand);
        PoolCardDrawn = true;
    }
    [PunRPC]
    public void M_ReturnPoolCardRPC(string Sender)
    {
        string cardToSend = PoolDeck[0].name;
        PoolDeck.RemoveAt(0);
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.NickName == Sender)
            {
                Player reciever = p;
                photonView.RPC("GetPoolCardRPC", reciever, cardToSend);
                return;
            }
            else
            {
                Debug.Log("Reciever not found");
            }
        }
    }

    [PunRPC]
    public void DiscardHandCardRPC(string discardedCardName)
    {
        TableCardRPC.SetActive(true);
        TableCards.Add(discardedCardName);
        var newObject = Instantiate(cardPrefab, GameObject.FindGameObjectWithTag("Table").transform);
        newObject.transform.localScale = new Vector3(1f, 1f, 1f);
        HandCardDiscarded = true;
    }

    [PunRPC]
    public void NextTurnRPC()
    {
        print("turn 2 ");
        StopAllCoroutines();
        GetNextTurn();
        StartCoroutine(TurnSwitch());
    }
    #endregion

    [PunRPC]
    public void CreateRemoteStih()
    {

    }
    [PunRPC]
    public void AddCardToRemoteStih()
    {

    }
    [PunRPC]
    public void DiscardHandCard(string discardedCardName)
    {
        //int playerID;
        if (!HandCardDiscarded && PoolCardDrawn)
        {
            populateAllPlayersList();
            TableCards.Add(discardedCardName);
            HandCardDiscarded = true;
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.NickName != PhotonNetwork.NickName)
                {
                    photonView.RPC("DiscardHandCardRPC", p, discardedCardName);
                }
            }
        }
    }
    [PunRPC]
    public void SendMsg(string msg, PhotonMessageInfo info)
    {
        InGameText.text = info.Sender.NickName + ": " + msg + "\n" + InGameText.text;
    }
    [PunRPC]
    public void CardsInHand(string username, int cardsInHand)
    {
        GameObject.Find(username).transform.GetChild(5).GetComponent<Text>().text = cardsInHand.ToString();
    }
    public void GetPoolCard()
    {
        if (isMyTurn && !PoolCardDrawn)
        {
            populateAllPlayersList();
            PoolPileIndicator.SetActive(false);
            photonView.RPC("M_ReturnPoolCardRPC", RpcTarget.MasterClient, PhotonNetwork.NickName);
        }
    }
    public void CallGetCardsRPC()
    {
        populateAllPlayersList();
        foreach (Player p in AllPlayers)
        {
            cardsNames = "";
            for (int i = 0; i < 12; i++)
            {
                cardsNames = cardsNames + ShuffledDeck[0].name;
                cardsNames = cardsNames + "\n";
                ShuffledDeck.RemoveAt(0);
            }
            photonView.RPC("GetCardsRPC", p, cardsNames);
        }
        PoolDeck = ShuffledDeck;    //all cards from shuffled deck go into pool deck
    }

    public void InitializePlayersRPC()
    {
        populateAllPlayersList();
        photonView.RPC("InitializePlayers", RpcTarget.All, PhotonNetwork.PlayerList.Length);
    }

    [PunRPC]
    public void SendStihRPC()
    {
        CheckingInstatiate(allSelectedCards);

    }

    int groupBtnChecker;
    public void Drop()
    {
        if (groupClick)
        {
            print("group BtnChecker " + groupBtnChecker);
            if (groupBtnChecker == 1)
            {
                //CreateStih(allSelectedCards, LocalFinalStih);
                CheckingInstatiate(saveGroupCards1);
                photonView.RPC("SendStih", RpcTarget.Others, "stih");
                foreach (Transform card in LocalPlayerStih.GetChild(0).transform)
                {
                    photonView.RPC("SendStih", RpcTarget.Others, card.name);
                }

                LocalPlayerStih.GetChild(0).gameObject.SetActive(false);
            }
            if (groupBtnChecker == 2)
            {
                CheckingInstatiate(saveGroupCards2);
                photonView.RPC("SendStih", RpcTarget.Others, "stih");
                foreach (Transform card in LocalPlayerStih.GetChild(1).transform)
                {
                    photonView.RPC("SendStih", RpcTarget.Others, card.name);
                }

                LocalPlayerStih.GetChild(1).gameObject.SetActive(false);
            }
            if (groupBtnChecker == 3)
            {
                CheckingInstatiate(saveGroupCards3);
                photonView.RPC("SendStih", RpcTarget.Others, "stih");
                foreach (Transform card in LocalPlayerStih.GetChild(2).transform)
                {
                    photonView.RPC("SendStih", RpcTarget.Others, card.name);
                }

                LocalPlayerStih.GetChild(2).gameObject.SetActive(false);
            }

            print("clicked ");
            //photonView.RPC("SendStihRPC", RpcTarget.All);
            groupClick = false;

        }
    }

    //test

    bool groupClick;

    public void ToSelectCard()
    {
        groupClick = true;
        groupBtnChecker = 1;

    }
    public void ToSelectCard2()
    {
        groupClick = true;
        groupBtnChecker = 2;
    }
    public void ToSelectCard3()
    {
        groupClick = true;
        groupBtnChecker = 3;
        //groupClick = !groupClick;
        //if (groupClick)
        //{
        //    groupBtnChecker = 3;
        //    groupClick = true;

        //}
    }

    //public void GetAllPlayers()
    //{
    //    AllPlayersNames.Clear();
    //    textToListAllPlayersInRoom.text = "";
    //    Player[] AllPlayers = PhotonNetwork.PlayerList;
    //    if (AllPlayers.Length == 0)
    //    {
    //        Debug.Log("List is empty");
    //    }
    //    foreach (Player p in AllPlayers)
    //    {
    //        AllPlayersNames.Add(p.UserId);
    //        textToListAllPlayersInRoom.text = textToListAllPlayersInRoom.text + p.UserId + "\n";
    //        Debug.Log(p.UserId);
    //    }
    //}

    public void SendMsgRPC()
    {
        photonView.RPC("SendMsg", RpcTarget.All, LocalMsg.text);
        ChatInput.text = "";
    }

    public void SetLocalCardDesign()
    {
        foreach (Image cd in CardDesignImages)
        {
            if (cd.IsActive())
            {
                cd.sprite = LocalCardDesign[GameObject.FindGameObjectWithTag("UserData").GetComponent<UserData>().CardDesign - 1];
            }
        }
    }

    public void ReturnToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("RoomsPun2");
    }

    public void populateAllPlayersList()
    {
        PhotonView photonView = PhotonView.Get(this);
        AllPlayers = PhotonNetwork.PlayerList;
    }

    public void GetNextTurn()
    {
        populateAllPlayersList();

        if (ActivePlayerIndex > AllPlayers.Length - 1) //resets turn order, starts from first player in order
        {
            ActivePlayerIndex = 0;
        }
        ActivePlayer = AllPlayers[ActivePlayerIndex];
        foreach (Player p in AllPlayers)
        {
            if (p == ActivePlayer)
            {
                photonView.RPC("SetNextPlayerTurnRPC", p, true);

                photonView.RPC("SetActivePlayerBoard", RpcTarget.All, p.NickName);
            }
            else
            {
                photonView.RPC("SetNextPlayerTurnRPC", p, false);
            }
        }
        ActivePlayerIndex++;
    }

    public IEnumerator TurnSwitch()
    {
        if (!InitialisedFirstTime)
        {
            TurnTime = 1;
            InitialisedFirstTime = true;
        }
        yield return new WaitForSeconds(TurnTime);
        TurnTime = 15;
        //GetPoolCard();
        //AutoDiscardHandCard();
        GetNextTurn();
        StartCoroutine(TurnSwitch());
    }

    public void NextTurn()
    {
        populateAllPlayersList();
        print("turn 3 ");
        photonView.RPC("NextTurnRPC", RpcTarget.MasterClient);

    }
    public Image WinnerAvatar;
    public Text WinnerText;
    public string WinnerAvatarIndex;

    public void IsItMyTurn()
    {
        if (!gameOff)
        {
            if (isMyTurn)
            {
                initialiseData = true;
                TurnIndicator.SetActive(true);
                LocalTurnSlider.value -= Time.deltaTime;
            }
            else
            {
                TurnIndicator.SetActive(false);
                PoolPileIndicator.SetActive(false);
                if (initialiseData)
                {
                    //ReturnAllCardsFromOpenSelectionToHand();
                    CardInHandsRPC();
                    ScoreCardAllPlayer();

                    int LocalCardsInHand = 0;
                    foreach (Transform card in LocalHand)
                    {
                        if (card.gameObject.activeSelf) //activeSelf
                        {
                            LocalCardsInHand++;
                        }
                    }
                    Debug.Log("CardsInHand " + LocalCardsInHand.ToString());
                    if (LocalCardsInHand == 0)
                    {
                        Debug.Log("ENTERED WIN CHECK " + LocalCardsInHand.ToString());
                        //WinPanel.SetActive(true);
                        WinnerAvatar.sprite = LocalAvatar.sprite;
                        WinnerText.text = PhotonNetwork.LocalPlayer.NickName;
                        photonView.RPC("isGameOver", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName, GameObject.FindGameObjectWithTag("UserData").GetComponent<UserData>().Avatar);
                    }
                    photonView.RPC("Reshuffle", RpcTarget.MasterClient);
                    initialiseData = false;
                }

            }
        }
    }

    bool gameOff = true;
    private void Update()
    {
        AreWeMaster();
        IsItMyTurn();
        if (LocalMsg.text != "")
        {
            SendMsgRPC();
        }

        ScoreCardInHandsRPC();
        CardInHandsRPC();
    }

    public List<GameObject> CardsToSort;
    public int LowestCardValue;
    private GameObject LowestCard;

    public void SortHand()
    {
        CardsToSort.Clear();
        LowestCard = null;
        LowestCardValue = 0;
        for (int i = 0; i < Hand.transform.childCount; i++)
        {
            LowestCardValue = Hand.transform.GetChild(i).GetComponent<CardScript>().CardValue;
            LowestCard = Hand.transform.GetChild(i).gameObject;
            for (int j = i + 1; j < Hand.transform.childCount; j++)
            {
                if (Hand.transform.GetChild(j).GetComponent<CardScript>().CardValue < LowestCardValue)
                {
                    LowestCardValue = Hand.transform.GetChild(j).GetComponent<CardScript>().CardValue;
                    LowestCard = Hand.transform.GetChild(j).gameObject;
                }
            }
            LowestCard.transform.SetSiblingIndex(i);
        }
    }
    public void SortSymbol()
    {
        LowestCard = null;
        LowestCardValue = 0;
        for (int i = 0; i < Hand.transform.childCount; i++)
        {
            LowestCardValue = Hand.transform.GetChild(i).GetComponent<CardScript>().CardSign;
            LowestCard = Hand.transform.GetChild(i).gameObject;
            for (int j = i + 1; j < Hand.transform.childCount; j++)
            {
                if (Hand.transform.GetChild(j).GetComponent<CardScript>().CardSign < LowestCardValue)
                {
                    LowestCardValue = Hand.transform.GetChild(j).GetComponent<CardScript>().CardSign;
                    LowestCard = Hand.transform.GetChild(j).gameObject;
                }
            }
            LowestCard.transform.SetSiblingIndex(i);
        }
    }


    public void ClearSelection()
    {
        foreach (Transform card in LocalHand.transform)
        {
            card.GetChild(0).GetComponent<Toggle>().isOn = false;
        }
    }
    public List<CardScript> selectedCards;
    public List<CardScript> selectedJokers;
    public List<CardScript> allSelectedCards;
    public bool StihCanForm;
    public void CheckIfSelectionIsValid()
    {
        StihCanForm = false;
        selectedCards.Clear();
        selectedJokers.Clear();
        allSelectedCards.Clear();
        IncrementStih = false;
        SignStih = false;

        foreach (Transform card in LocalHand.transform)
        {
            if (card.name != LocalHand.transform.name)
            {
                if (card.GetChild(0).GetComponent<Toggle>().isOn)
                {
                    allSelectedCards.Add(card.gameObject.GetComponent<CardScript>());
                    Debug.Log(card.gameObject.name);
                    selectedCards.Add(card.gameObject.GetComponent<CardScript>());
                    if (card.gameObject.GetComponent<CardScript>().isJoker)
                    {
                        selectedJokers.Add(card.gameObject.GetComponent<CardScript>());
                        selectedCards.Remove(card.gameObject.GetComponent<CardScript>());
                    }
                }
            }
        }

        int selectionCardsCount = selectedCards.Count;
        if (selectionCardsCount + selectedJokers.Count <= 2)
        {
            return;
        }

        //CheckIfSignIsSame(selectedCards);
        //CheckIfValueIsSame(selectedCards);
        //CheckIfAllSignsDifferent(selectedCards);
        CheckOnlyOneAce(selectedCards);
        //CheckIfCardValuesAreIncremental(selectedCards, selectedJokers);

        if (CheckIfValueIsSame(selectedCards) && CheckIfAllSignsDifferent(selectedCards, selectedJokers) && allSelectedCards.Count <= 4)
        {
            SignStih = true;
            CreateStih(allSelectedCards, LocalPlayerStih);
            if (isOpen)
            {
                photonView.RPC("SendStih", RpcTarget.Others, "stih");
                foreach (CardScript card in allSelectedCards)
                {
                    photonView.RPC("SendStih", RpcTarget.Others, card.name);
                }
            }
            ButtonPrefab();
            Debug.Log("button is active ");
            return;

        }

        if (CheckIfSignIsSame(selectedCards) && CheckIfCardValuesAreIncremental(selectedCards, selectedJokers))
        {
            IncrementStih = true;
            CreateStih(allSelectedCards, LocalPlayerStih);
            if (isOpen)
            {
                photonView.RPC("SendStih", RpcTarget.Others, "stih");
                foreach (CardScript card in allSelectedCards)
                {
                    photonView.RPC("SendStih", RpcTarget.Others, card.name);
                }
            }
            ButtonPrefab();
            Debug.Log("button is active ");
            return;
        }
    }

    public GameObject btnPrefab1;
    public GameObject btnPrefab2;
    public GameObject btnPrefab3;
    void ButtonPrefab()
    {
        //yield return new WaitForSeconds(1f);
        if (groupCheck == 1)
        {
            var btnprefab = Instantiate(btnPrefab1, LocalPlayerStih.GetChild(LocalPlayerStih.childCount - 1));
            btnprefab.transform.localPosition = new Vector3(0, 0, 0);
        }
        if (groupCheck == 2)
        {
            var btnprefab = Instantiate(btnPrefab2, LocalPlayerStih.GetChild(LocalPlayerStih.childCount - 1));
            btnprefab.transform.localPosition = new Vector3(0, 0, 0);

        }
        if (groupCheck == 3)
        {
            var btnprefab = Instantiate(btnPrefab3, LocalPlayerStih.GetChild(LocalPlayerStih.childCount - 1));
            btnprefab.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    #region StihCheck
    bool CheckIfSignIsSame(List<CardScript> selectedCards)
    {
        foreach (CardScript cs in selectedCards)
        {
            if (cs == selectedCards[0])
            {
                signToCheck = cs.CardSign;
            }
            if (cs.CardSign != signToCheck)
            {
                Debug.Log("Signs are not the same");

                return false;

            }
        }

        Debug.Log("Signs are same");
        return true;
    }

    bool CheckIfValueIsSame(List<CardScript> selectedCards)
    {
        foreach (CardScript cs in selectedCards)
        {
            if (cs == selectedCards[0])
            {
                valueToCheck = cs.CardValue;
            }
            if (cs.CardValue != valueToCheck)
            {

                Debug.Log("Values are not the same");
                return false;
            }
        }

        Debug.Log("Values are the same");
        return true;
    }
    public int JokerValuesToBeAdded;
    bool CheckIfCardValuesAreIncremental(List<CardScript> selectedCards, List<CardScript> selectedJokers)
    {
        JokerValuesToBeAdded = 0;
        valuesToCheckIncremental.Clear();
        foreach (CardScript cv in selectedCards)
        {
            valuesToCheckIncremental.Add(cv.CardValue);
        }

        valuesToCheckIncremental.Sort();
        for (int i = valuesToCheckIncremental.Count - 1; i > 0; i--)
        {
            Debug.Log(("FIRST: " + valuesToCheckIncremental[i].ToString() + " - " + valuesToCheckIncremental[i - 1].ToString() + " = " + (valuesToCheckIncremental[i] - valuesToCheckIncremental[i - 1]).ToString()));
            if (valuesToCheckIncremental[i] - valuesToCheckIncremental[i - 1] != 1)
            {
                if ((valuesToCheckIncremental[i] - valuesToCheckIncremental[i - 1] == 2) && selectedJokers.Count != 0)
                {
                    JokerValuesToBeAdded += valuesToCheckIncremental[i - 1] + 1;
                    if (selectedJokers.Count != 0)
                    {
                        selectedJokers.RemoveAt(0);
                    }
                    else
                    {

                        Debug.Log("Cards are not incremental");
                        return false;
                    }
                }
                else
                {

                    Debug.Log("Cards are not incremental");
                    return false;
                }
            }
        }
        for (int k = 0; k < selectedJokers.Count; k++)
        {
            JokerValuesToBeAdded += valuesToCheckIncremental[valuesToCheckIncremental.Count - 1] + 1 + k;
        }

        Debug.Log("Cards are incremental");
        return true;
    }
    public List<int> signsToCheck;
    public int cardValueSample;
    bool CheckIfAllSignsDifferent(List<CardScript> selectedCards, List<CardScript> selectedJokers)
    {
        JokerValuesToBeAdded = 0;
        signsToCheck.Clear();
        signsToCheck.Add(1);
        signsToCheck.Add(2);
        signsToCheck.Add(3);
        signsToCheck.Add(4);
        foreach (CardScript cv in selectedCards)
        {
            if (signsToCheck.Contains(cv.CardSign))
            {
                signsToCheck.Remove(cv.CardSign);
            }
            else
            {

                Debug.Log("Not unique signs");
                return false;
            }
        }
        if (selectedJokers.Count != 0)
        {
            foreach (CardScript cs in selectedCards)
            {
                if (!cs.isJoker)
                {
                    cardValueSample = cs.CardValue;
                }
            }
            if (cardValueSample == 1)
            {
                foreach (CardScript cs in selectedJokers)
                {
                    JokerValuesToBeAdded += 10;
                }
            }
            else if (cardValueSample >= 10)
            {
                foreach (CardScript cs in selectedJokers)
                {
                    JokerValuesToBeAdded += 10;
                }
            }
            else
            {
                foreach (CardScript cs in selectedJokers)
                {
                    JokerValuesToBeAdded += cardValueSample;
                }
            }
        }

        Debug.Log("Unique signs");
        return true;
    }
    public int numberOfAces;
    public CardScript CardScript;
    bool CheckOnlyOneAce(List<CardScript> selectedCards)
    {
        numberOfAces = 0;
        foreach (CardScript c in selectedCards)
        {
            if (c.CardValue == 1 || c.CardValue == 11)
            {
                numberOfAces++;
                CardScript = c;
            }
        }
        if (numberOfAces == 1)
        {

            SetOneAceValue(CardScript, selectedCards);
            return true;
        }
        else
        {

            return false;
        }
    }
    void SetOneAceValue(CardScript AceCardScript, List<CardScript> selectedCards)
    {
        foreach (CardScript c in selectedCards)
        {
            //if (c.CardValue == 10 || c.CardValue == 11)
            //{
            //    Debug.Log("Ace value set to 11");
            //    AceCardScript.CardValue = 10;
            //    return;
            //}
            if (c.CardValue == 2)
            {
                Debug.Log("Ace value set to 1");
                AceCardScript.CardValue = 1;
                return;
            }
        }
    }
    #endregion
    // test
    public Transform LocalFinalStih;
    public Transform LocalFinalStih2;
    public Transform LocalFinalStih3;

    public Image ImgPrefab;
    public List<Sprite> cardSprite;
    int roundCheck = 0;
    Transform localPos;

    void CheckingInstatiate(List<CardScript> allSelectedCards)
    {
        roundCheck++;
        print("roundCheck " + roundCheck);
        foreach (CardScript item in allSelectedCards)
        {
            if (roundCheck == 1)
            {
                localPos = LocalFinalStih;
            }
            if (roundCheck == 2)
            {
                localPos = LocalFinalStih2;
            }
            if (roundCheck == 3)
            {
                localPos = LocalFinalStih3;
            }

            if (item.CardSign == 1)
            {
                Image localStihImg = Instantiate(ImgPrefab, localPos);
                //Image leftStihImg = Instantiate(ImgPrefab, centerPos);
                //Image rightStihImg = Instantiate(ImgPrefab, rightPos);
                switch (item.CardValue)
                {
                    case 1:
                        localStihImg.sprite = cardSprite[0];
                        //leftStihImg.sprite = cardSprite[0];
                        //rightStihImg.sprite = cardSprite[0];
                        break;
                    case 2:
                        localStihImg.sprite = cardSprite[1];
                        //leftStihImg.sprite = cardSprite[1];
                        //rightStihImg.sprite = cardSprite[1];
                        break;
                    case 3:
                        localStihImg.sprite = cardSprite[2];
                        //leftStihImg.sprite = cardSprite[2];
                        //rightStihImg.sprite = cardSprite[2];
                        break;
                    case 4:
                        localStihImg.sprite = cardSprite[3];
                        //leftStihImg.sprite = cardSprite[3];
                        //rightStihImg.sprite = cardSprite[3];
                        break;
                    case 5:
                        localStihImg.sprite = cardSprite[4];
                        //leftStihImg.sprite = cardSprite[4];
                        //rightStihImg.sprite = cardSprite[4];
                        break;
                    case 6:
                        localStihImg.sprite = cardSprite[5];
                        //leftStihImg.sprite = cardSprite[5];
                        //rightStihImg.sprite = cardSprite[5];
                        break;
                    case 7:
                        localStihImg.sprite = cardSprite[6];
                        //leftStihImg.sprite = cardSprite[6];
                        //rightStihImg.sprite = cardSprite[6];
                        break;
                    case 8:
                        localStihImg.sprite = cardSprite[7];
                        //leftStihImg.sprite = cardSprite[7];
                        //rightStihImg.sprite = cardSprite[7];
                        break;
                    case 9:
                        localStihImg.sprite = cardSprite[8];
                        //leftStihImg.sprite = cardSprite[8];
                        //rightStihImg.sprite = cardSprite[8];
                        break;
                    case 10:
                        localStihImg.sprite = cardSprite[9];
                        //leftStihImg.sprite = cardSprite[9];
                        //rightStihImg.sprite = cardSprite[9];
                        break;
                    case 11:
                        localStihImg.sprite = cardSprite[10];
                        //leftStihImg.sprite = cardSprite[10];
                        //rightStihImg.sprite = cardSprite[10];
                        break;
                    case 12:
                        localStihImg.sprite = cardSprite[11];
                        //leftStihImg.sprite = cardSprite[11];
                        //rightStihImg.sprite = cardSprite[11];
                        break;
                    case 13:
                        localStihImg.sprite = cardSprite[12];
                        //leftStihImg.sprite = cardSprite[12];
                        //rightStihImg.sprite = cardSprite[12];
                        break;

                }
            }

            if (item.CardSign == 2)
            {
                Image localStihImg = Instantiate(ImgPrefab, localPos);
                //Image leftStihImg = Instantiate(ImgPrefab, centerPos);
                //Image rightStihImg = Instantiate(ImgPrefab, rightPos);
                switch (item.CardValue)
                {
                    case 1:
                        localStihImg.sprite = cardSprite[13];
                        //leftStihImg.sprite = cardSprite[13];
                        //rightStihImg.sprite = cardSprite[13];
                        break;
                    case 2:
                        localStihImg.sprite = cardSprite[14];
                        //leftStihImg.sprite = cardSprite[14];
                        //rightStihImg.sprite = cardSprite[14];
                        break;
                    case 3:
                        localStihImg.sprite = cardSprite[15];
                        //leftStihImg.sprite = cardSprite[15];
                        //rightStihImg.sprite = cardSprite[15];
                        break;
                    case 4:
                        localStihImg.sprite = cardSprite[16];
                        //leftStihImg.sprite = cardSprite[16];
                        //rightStihImg.sprite = cardSprite[16];
                        break;
                    case 5:
                        localStihImg.sprite = cardSprite[17];
                        //leftStihImg.sprite = cardSprite[17];
                        //rightStihImg.sprite = cardSprite[17];
                        break;
                    case 6:
                        localStihImg.sprite = cardSprite[18];
                        //leftStihImg.sprite = cardSprite[18];
                        //rightStihImg.sprite = cardSprite[18];
                        break;
                    case 7:
                        localStihImg.sprite = cardSprite[19];
                        //leftStihImg.sprite = cardSprite[19];
                        //rightStihImg.sprite = cardSprite[19];
                        break;
                    case 8:
                        localStihImg.sprite = cardSprite[20];
                        //leftStihImg.sprite = cardSprite[20];
                        //rightStihImg.sprite = cardSprite[20];
                        break;
                    case 9:
                        localStihImg.sprite = cardSprite[21];
                        //leftStihImg.sprite = cardSprite[21];
                        //rightStihImg.sprite = cardSprite[21];
                        break;
                    case 10:
                        localStihImg.sprite = cardSprite[22];
                        //leftStihImg.sprite = cardSprite[22];
                        //rightStihImg.sprite = cardSprite[22];
                        break;
                    case 11:
                        localStihImg.sprite = cardSprite[23];
                        //leftStihImg.sprite = cardSprite[23];
                        //rightStihImg.sprite = cardSprite[23];
                        break;
                    case 12:
                        localStihImg.sprite = cardSprite[24];
                        //leftStihImg.sprite = cardSprite[24];
                        //rightStihImg.sprite = cardSprite[24];
                        break;
                    case 13:
                        localStihImg.sprite = cardSprite[25];
                        //leftStihImg.sprite = cardSprite[25];
                        //rightStihImg.sprite = cardSprite[25];
                        break;

                }
            }

            if (item.CardSign == 3)
            {
                Image localStihImg = Instantiate(ImgPrefab, localPos);
                //Image leftStihImg = Instantiate(ImgPrefab, centerPos);
                //Image rightStihImg = Instantiate(ImgPrefab, rightPos);
                switch (item.CardValue)
                {
                    case 1:
                        localStihImg.sprite = cardSprite[26];
                        //leftStihImg.sprite = cardSprite[26];
                        //rightStihImg.sprite = cardSprite[26];
                        break;
                    case 2:
                        localStihImg.sprite = cardSprite[27];
                        //leftStihImg.sprite = cardSprite[27];
                        //rightStihImg.sprite = cardSprite[27];
                        break;
                    case 3:
                        localStihImg.sprite = cardSprite[28];
                        //leftStihImg.sprite = cardSprite[28];
                        //rightStihImg.sprite = cardSprite[28];
                        break;
                    case 4:
                        localStihImg.sprite = cardSprite[29];
                        //leftStihImg.sprite = cardSprite[29];
                        //rightStihImg.sprite = cardSprite[29];
                        break;
                    case 5:
                        localStihImg.sprite = cardSprite[30];
                        //leftStihImg.sprite = cardSprite[30];
                        //rightStihImg.sprite = cardSprite[30];
                        break;
                    case 6:
                        localStihImg.sprite = cardSprite[31];
                        //leftStihImg.sprite = cardSprite[31];
                        //rightStihImg.sprite = cardSprite[31];
                        break;
                    case 7:
                        localStihImg.sprite = cardSprite[32];
                        //leftStihImg.sprite = cardSprite[32];
                        //rightStihImg.sprite = cardSprite[32];
                        break;
                    case 8:
                        localStihImg.sprite = cardSprite[33];
                        //leftStihImg.sprite = cardSprite[33];
                        //rightStihImg.sprite = cardSprite[33];
                        break;
                    case 9:
                        localStihImg.sprite = cardSprite[34];
                        //leftStihImg.sprite = cardSprite[34];
                        //rightStihImg.sprite = cardSprite[34];
                        break;
                    case 10:
                        localStihImg.sprite = cardSprite[35];
                        //leftStihImg.sprite = cardSprite[35];
                        //rightStihImg.sprite = cardSprite[35];
                        break;
                    case 11:
                        localStihImg.sprite = cardSprite[36];
                        //leftStihImg.sprite = cardSprite[36];
                        //rightStihImg.sprite = cardSprite[36];
                        break;
                    case 12:
                        localStihImg.sprite = cardSprite[37];
                        //leftStihImg.sprite = cardSprite[37];
                        //rightStihImg.sprite = cardSprite[37];
                        break;
                    case 13:
                        localStihImg.sprite = cardSprite[38];
                        //leftStihImg.sprite = cardSprite[38];
                        //rightStihImg.sprite = cardSprite[38];
                        break;
                }
            }

            if (item.CardSign == 4)
            {
                Image localStihImg = Instantiate(ImgPrefab, localPos);
                //Image leftStihImg = Instantiate(ImgPrefab, centerPos);
                //Image rightStihImg = Instantiate(ImgPrefab, rightPos);
                switch (item.CardValue)
                {
                    case 1:
                        localStihImg.sprite = cardSprite[39];
                        //leftStihImg.sprite = cardSprite[39];
                        //rightStihImg.sprite = cardSprite[39];
                        break;
                    case 2:
                        localStihImg.sprite = cardSprite[40];
                        //leftStihImg.sprite = cardSprite[40];
                        //rightStihImg.sprite = cardSprite[40];
                        break;
                    case 3:
                        localStihImg.sprite = cardSprite[41];
                        //leftStihImg.sprite = cardSprite[41];
                        //rightStihImg.sprite = cardSprite[41];
                        break;
                    case 4:
                        localStihImg.sprite = cardSprite[42];
                        //leftStihImg.sprite = cardSprite[42];
                        //rightStihImg.sprite = cardSprite[42];
                        break;
                    case 5:
                        localStihImg.sprite = cardSprite[43];
                        //leftStihImg.sprite = cardSprite[43];
                        //rightStihImg.sprite = cardSprite[43];
                        break;
                    case 6:
                        localStihImg.sprite = cardSprite[44];
                        //leftStihImg.sprite = cardSprite[44];
                        //rightStihImg.sprite = cardSprite[44];
                        break;
                    case 7:
                        localStihImg.sprite = cardSprite[45];
                        //leftStihImg.sprite = cardSprite[45];
                        //rightStihImg.sprite = cardSprite[45];
                        break;
                    case 8:
                        localStihImg.sprite = cardSprite[46];
                        //leftStihImg.sprite = cardSprite[46];
                        //rightStihImg.sprite = cardSprite[46];
                        break;
                    case 9:
                        localStihImg.sprite = cardSprite[47];
                        //leftStihImg.sprite = cardSprite[47];
                        //rightStihImg.sprite = cardSprite[47];
                        break;
                    case 10:
                        localStihImg.sprite = cardSprite[48];
                        //leftStihImg.sprite = cardSprite[48];
                        //rightStihImg.sprite = cardSprite[48];
                        break;
                    case 11:
                        localStihImg.sprite = cardSprite[49];
                        //leftStihImg.sprite = cardSprite[49];
                        //rightStihImg.sprite = cardSprite[49];
                        break;
                    case 12:
                        localStihImg.sprite = cardSprite[50];
                        //leftStihImg.sprite = cardSprite[50];
                        //rightStihImg.sprite = cardSprite[50];
                        break;
                    case 13:
                        localStihImg.sprite = cardSprite[51];
                        //leftStihImg.sprite = cardSprite[51];
                        //rightStihImg.sprite = cardSprite[51];
                        break;

                }
            }
            if (item.CardSign == 0)
            {
                Image localStihImg = Instantiate(ImgPrefab, localPos);
                switch (item.CardValue)
                {
                    case 0:
                        localStihImg.sprite = cardSprite[52];
                        break;
                }
            }
        }
        //cardSprite.Clear();
    }

    #region Create Stih
    public List<CardScript> saveGroupCards1;
    public List<CardScript> saveGroupCards2;
    public List<CardScript> saveGroupCards3;
    int groupCheck = 0;
    public List<CardScript> SelectedCardsForOpening;

    public void CreateStih(List<CardScript> allSelectedCards, Transform PlayerStihTransform)
    {
        SelectedCardsForOpening.Clear();
        Instantiate(StihPrefab, PlayerStihTransform);
        groupCheck++;
        print("groupCheck " + groupCheck);
        foreach (CardScript cs in allSelectedCards)
        {
            if (groupCheck == 1)
            {
                saveGroupCards1.Add(cs);
            }
            if (groupCheck == 2)
            {
                saveGroupCards2.Add(cs);
            }
            if (groupCheck == 3)
            {
                saveGroupCards3.Add(cs);
            }
            //TO DO: JOKER AND VALUES TO ADD
            SelectedCardsForOpening.Add(cs);
            //saveGroupCards.Insert(cs.CardValue, cs);
            if (cs.isAce)
            {
                OpeningSelectionValue += 10;
            }
            else if (cs.CardValue > 10)
            {
                OpeningSelectionValue += 10;
            }
            else
            {
                OpeningSelectionValue += cs.CardValue;
            }
            Instantiate(cardPrefab, PlayerStihTransform.GetChild(PlayerStihTransform.childCount - 1));
        }

        foreach (CardScript cs in SelectedCardsForOpening)
        {
            cs.gameObject.transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn = false;
            cs.gameObject.SetActive(false);
        }
        OpeningSelectionValue += JokerValuesToBeAdded;
        OpeningSelectionValueText.text = OpeningSelectionValue.ToString();
    }

    public void ReturnAllCardsFromOpenSelectionToHand()
    {
        if (!isOpen)
        {
            OpeningSelectionValue = 0;
            OpeningSelectionValueText.text = "0";
            foreach (CardScript cs in SelectedCardsForOpening)
            {
                cs.gameObject.SetActive(true);
            }
            foreach (Transform stih in LocalPlayerStih)
            {
                if (stih.tag == "Stih")
                {
                    Destroy(stih.gameObject);
                }
            }
        }
    }

    public void OpenUp()
    {

        isOpen = true;
        SelectedCardsForOpening.Clear();
        OpeningSelectionValueText.gameObject.SetActive(false);
        iStihCards.Clear();
        //OpenUpButton.SetActive(false);
        ClearOpeningSelection.SetActive(false);
        foreach (Transform stih in LocalPlayerStih)
        {
            photonView.RPC("SendStih", RpcTarget.Others, "stih");
            foreach (Transform card in stih)
            {
                photonView.RPC("SendStih", RpcTarget.Others, card.name);
            }
        }
    }

    public void FinalShowUpCards()
    {
        photonView.RPC("ShowAllCardsRemote", RpcTarget.Others, "LocalHand");
        foreach (Transform card in LocalHand)
        {
            photonView.RPC("ShowAllCardsRemote", RpcTarget.Others, card.name);
        }
    }

    [PunRPC]
    public void ShowAllCardsRemote(string s, PhotonMessageInfo info)
    {

        if (s == "LocalHand")
        {
            Instantiate(Hand, GameObject.Find(info.Sender.NickName).transform.GetChild(9).transform);
        }
    }

    public void RefreshStihsOnAddCard(string username)
    {
        photonView.RPC("DeleteStihsOnPlayer", RpcTarget.Others, username);
        Debug.Log(username);
        iStihCards.Clear();
        foreach (Transform stih in GameObject.Find(username).transform.GetChild(0))
        {
            photonView.RPC("SendStihRemote", RpcTarget.Others, "stih", username);
            foreach (Transform card in stih)
            {
                photonView.RPC("SendStihRemote", RpcTarget.Others, card.name, username);
            }
        }
    }
    public int currentCardsInDeck;
    public void CardInHandsRPC()
    {
        if (!gameOff)
        {
            currentCardsInDeck = 0;
            foreach (Transform cardInHand in Hand.transform)
            {
                if (cardInHand.gameObject.activeSelf) // (cardInHand.gameObject.GetActive())
                {
                    currentCardsInDeck++;
                    //cardsInHandText.text = currentCardsInDeck.ToString();
                }
            }
            LocalPlayer.transform.GetChild(5).gameObject.GetComponent<Text>().text = currentCardsInDeck.ToString();
            //photonView.RPC("CardsInHand", RpcTarget.Others, this.LocalPlayer.name, currentCardsInDeck);
            photonView.RPC("ShowCardsInHand", RpcTarget.All, this.LocalPlayer.name, currentCardsInDeck);
        }
    }


    [PunRPC]
    public void ShowCardsInHand(string name, int value)
    {
        if (LocalPlayer.name == name)
        {
            LocalPlayer.transform.GetChild(5).gameObject.GetComponent<Text>().text = value.ToString();
        }
        if (CenterPlayer.name == name)
        {
            CenterPlayer.transform.GetChild(5).gameObject.GetComponent<Text>().text = value.ToString();
        }
        if (RightPlayer.name == name)
        {
            RightPlayer.transform.GetChild(5).gameObject.GetComponent<Text>().text = value.ToString();
        }
    }

    [PunRPC]
    public void ScoreCardsInHand(string username, int cardsInHand)
    {
        GameObject.Find(username).transform.GetChild(7).GetComponent<Text>().text = cardsInHand.ToString();
    }

    //[SerializeField] private Text ScoreCard;
    //[SerializeField] private Text cardsInHandText;

    public int cardsValues;
    int scorePlayer1;
    int scorePlayer2;
    int scorePlayer3;
    int cardscore;
    public void ScoreCardInHandsRPC()
    {
        if (!gameOff)
        {
            int newCardValue = 0;
            cardsValues = 0;
            for (int i = 0; i < Hand.transform.childCount; i++)
            {
                cardsValues += Hand.transform.GetChild(i).GetComponent<CardScript>().ScoreCardValue;
                newCardValue = cardsValues - OpeningSelectionValue;
                //ScoreCard.text = "" + newCardValue;
                //cardscore = cardsValues - OpeningSelectionValue;
            }
            cardscore = newCardValue;
            //Debug.Log("newCardValue"+ cardscore);
            //LocalPlayer.transform.GetChild(8).gameObject.GetComponent<Text>().text = cardsValues.ToString();
            //photonView.RPC("ScoreCardsInHand", RpcTarget.Others, this.LocalPlayer.name, cardsValues);
            photonView.RPC("ShowScoreCardInHands", RpcTarget.All, this.LocalPlayer.name, cardsValues);
        }
    }

    #endregion

    [PunRPC]
    public void ShowScoreCardInHands(string name, int value)
    {
        if (LocalPlayer.name == name)
        {
            LocalPlayer.transform.GetChild(8).gameObject.GetComponent<Text>().text = value.ToString();
        }
        if (CenterPlayer.name == name)
        {
            CenterPlayer.transform.GetChild(8).gameObject.GetComponent<Text>().text = value.ToString();
        }
        if (RightPlayer.name == name)
        {
            RightPlayer.transform.GetChild(8).gameObject.GetComponent<Text>().text = value.ToString();
        }
    }

    public void ScoreCardAllPlayer()
    {
        photonView.RPC("BroadCast", RpcTarget.MasterClient, ActivePlayerIndex, cardscore);

    }

    [PunRPC]
    public void BroadCast(int playerNum, int score)
    {
        if (playerNum == 0)
        {
            scorePlayer1 = score;
        }
        if (playerNum == 1)
        {
            scorePlayer2 = score;
        }
        if (playerNum == 2)
        {
            scorePlayer3 = score;
        }
    }

    [PunRPC]
    public void CompareScore()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int winner = 0;
            int a = scorePlayer1, b = scorePlayer2, c = scorePlayer3;
            print("scorePlayer " + a + " " + b + " " + c);
            if (a < b && a < c)
            {
                winner = 1;
                print("a win");
            }
            if (b < c && b < a)
            {
                winner = 2;
                print("b win");
            }
            if (c < a && c < b)
            {
                winner = 3;
                print("c win");
            }
            if (a == b && a == c)
            {
                winner = 4;
                print("a,b,c win");
            }
            if (a == b)
            {
                winner = 5;
                print("a,b win");
            }
            if (b == c)
            {
                winner = 6;
                print("b,c win");
            }
            if (c == a)
            {
                winner = 7;
                print("a,c win");
            }
            photonView.RPC("BroadCastWinner", RpcTarget.All, winner);
        }
    }
    [PunRPC]
    public void BroadCastWinner(int winner)
    {
        Debug.Log("winner "+ winner);
        //WinPanel.SetActive(true);
        WinnerAvatar.sprite = LocalAvatar.sprite;
        WinnerText.text = PhotonNetwork.LocalPlayer.NickName;
        photonView.RPC("isGameOver", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName, GameObject.FindGameObjectWithTag("UserData").GetComponent<UserData>().Avatar);
    }
    public void FightBtn()
    {
        FinalShowUpCards();
        if (isMyTurn)
        {
            gameOff =true;
            photonView.RPC("SendScoreToMaster", RpcTarget.All);
            photonView.RPC("DelayCompareScore", RpcTarget.MasterClient);
            

            //OpenUp();
            //ScoreCardAllPlayer();
        }

    }
    [PunRPC]
    public void DelayCompareScore()
    {
        Invoke("CompareScore", 2f);
    }
    [PunRPC]
    public void GetPlayerScore1(int score, int playerNum)
    {
        Debug.Log(" GetPlayerScore1");
        if (PhotonNetwork.IsMasterClient)
        {
            scorePlayer1 = cardscore;
        }
        if (playerNum == 1)
        {
            scorePlayer2 = score;
        }
        //if (playerNum == 2)
        //{
        //    scorePlayer3 = score;
        //}
    }
    [PunRPC]
    public void GetPlayerScore2(int score, int playerNum)
    {
        Debug.Log(" GetPlayerScore2");
        if (PhotonNetwork.IsMasterClient)
        {
            scorePlayer1 = cardscore;
        }
        //if (playerNum == 1)
        //{
        //    scorePlayer2 = score;
        //}
        if (playerNum == 2)
        {
            scorePlayer3 = score;
        }
    }

    [PunRPC]
    public void SendScoreToMaster()
    {
        Debug.Log("send score master");
        if (PhotonNetwork.LocalPlayer.NickName == ActivePlayerUsernames[1])
        {
            int score = cardscore;
            Debug.Log("score 1" + score);
            photonView.RPC("GetPlayerScore1", RpcTarget.MasterClient, score, 1);
        }
        if (PhotonNetwork.LocalPlayer.NickName == ActivePlayerUsernames[2])
        {
            int score = cardscore;
            Debug.Log("score 2" + score);
            photonView.RPC("GetPlayerScore2", RpcTarget.MasterClient, score, 2);
        }
        

    }

    public void DumpCardBtn()
    {
        foreach (Transform card in LocalHand.transform)
        {
            if (card.name != LocalHand.transform.name)
            {
                if (card.GetChild(0).GetComponent<Toggle>().isOn)
                {
                    selectedCards.Add(card.gameObject.GetComponent<CardScript>());
                    if (selectedCards.Count == 1)
                    {
                        if (PoolCardDrawn && !HandCardDiscarded)
                        {
                            selectedCards[0].SelectionToggleObject.SetActive(false);
                            selectedCards[0].transform.SetParent(GameObject.FindGameObjectWithTag("Table").transform);
                            int TableCount = GameObject.FindGameObjectWithTag("Table").transform.childCount;
                            if (TableCount != 0)
                            {
                                selectedCards[0].transform.SetSiblingIndex(TableCount + 1);
                            }
                            selectedCards[0].transform.localScale = new Vector3(1f, 1f, 1f);
                            selectedCards[0].transform.position = GameObject.FindGameObjectWithTag("Table").transform.position;
                            DiscardHandCard(selectedCards[0].name);
                            NextTurn();
                            //StartCoroutine(WaitForEndTurn());
                            return;
                        }
                    }
                }
            }
        }
        selectedCards.Clear();
    }

    //public List<StihScript> selectedStih;

    //public void IshitToTable(GameObject stih)
    //{
    //    foreach (Transform Stihcard in LocalPlayerStih.transform)
    //    {

    //        selectedStih.Add(Stihcard.gameObject.GetComponent<StihScript>());
    //        //stih = stihSelection;
    //        if (selectedStih.Count == 1)
    //        {
    //            foreach (Player p in PhotonNetwork.PlayerList)
    //            {
    //                if (p.NickName != PhotonNetwork.NickName)
    //                {
    //                    Debug.Log("Istih ");
    //                    photonView.RPC("SendStih", p, stih);
    //                }
    //            }

    //        }

    //    }

    //}

    int lastCard;
    public void AutoDiscardHandCard()
    {
        for (int i = 0; i <= LocalHand.childCount; i++)
        {
            lastCard = LocalHand.childCount - 1;
        }
        //Transform a = LocalHand.SetSiblingIndex(lastCard);
        foreach (Transform card in LocalHand.transform)
        {
            Debug.Log("card " + card);
            if (card.name != LocalHand.transform.name)
            {
                if (card.GetChild(0).GetComponent<Toggle>().enabled)
                {
                    selectedCards.Add(card.gameObject.GetComponent<CardScript>());
                    if (selectedCards.Count == 1)
                    {
                        Debug.Log("lastCard " + lastCard);
                        if (PoolCardDrawn && !HandCardDiscarded)
                        {
                            selectedCards[0].SelectionToggleObject.SetActive(false);
                            selectedCards[0].transform.SetParent(GameObject.FindGameObjectWithTag("Table").transform);
                            int TableCount = GameObject.FindGameObjectWithTag("Table").transform.childCount;
                            if (TableCount != 0)
                            {
                                selectedCards[0].transform.SetSiblingIndex(TableCount + 1);
                            }
                            selectedCards[0].transform.localScale = new Vector3(1f, 1f, 1f);
                            selectedCards[0].transform.position = GameObject.FindGameObjectWithTag("Table").transform.position;
                            //DiscardHandCard(selectedCards[0].name);
                            //photonView.RPC("DiscardHandCard", RpcTarget.MasterClient, selectedCards[0].name);
                            photonView.RPC("DiscardHandCardRPC", RpcTarget.Others, selectedCards[0].name);

                            //NextTurn();
                            //StartCoroutine(WaitForEndTurn());
                            return;
                        }
                    }
                }
            }
        }
        selectedCards.Clear();
    }

}