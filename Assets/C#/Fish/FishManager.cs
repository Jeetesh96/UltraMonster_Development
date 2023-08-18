using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TongitsGo
{
    public class FishManager : MonoBehaviour
    {

        #region Variables
        public static FishManager instance;
        [SerializeField] private GameObject splashScreenGb;
        [SerializeField] private UnityEngine.UI.Image downloadBar;

        [SerializeField] private GameObject mainCanvasGb;

        public UnityEngine.UI.Text playerScoreText;
        public UnityEngine.UI.Text downloadProgressText;
        public Canon canonObject;
        
        [Header("Paths"), Space(12)]
        public PathCreator[] pathCreatorLToR;
        public PathCreator[] pathCreatorRToL;

        public PathCreator[] lessCurvePathLtoR;
        public PathCreator[] lessCurvePathRtoL;
        public PathCreator[] straightPathRtoL;
        public PathCreator[] straightPathLtoR;

        public PathCreator[] groundPathLToR;
        
        public PathCreator[] groundPathRToL;
        public PathCreator[] groundPathRToL2;
        public PathCreator[] groundPathRToL3;

        public PathCreator[] dragonPathRToL;
        public PathCreator[] dragonPathLToR;

        public PathCreator[] emperiorPath;

        public PathCreator[] fishPath;
        public PathCreator[] fishingpatternPath;
        public PathCreator[] fishrushPath;

        [Space(10)]
        [SerializeField] int[] probabilitiesvalue;
                                                    

        [SerializeField] List<float> cummulativeProbab = new List<float>();

        [SerializeField] int totalFishes;
        [SerializeField] int levelNumber;
        int currentLoadedFishesAmount = 0;

        [SerializeField] List<LevelDetails> specialCharacterDisableList = new List<LevelDetails>();

        public GameObject deadEffect;
        private float assetsDownloadProgrerss;
        public List<AssetReference> totalReferences = new List<AssetReference>();
        #endregion
        
        public void QuitGame()
        {
        SceneManager.LoadScene("LoadingHomeScreen");
        }        
        
        private void Awake() 
        {
            LoadAddressable();
        }

        private void Start()
        {
            instance = this;
            CreateCummulativeProbability();
        }

        void StartGame()
        {
            splashScreenGb.SetActive(false);
            mainCanvasGb.SetActive(true);
            GameManager.Instance.gameStarted = true;
            GameManager.Instance.gameEnded = false;
            InvokeRepeating("Probability", 0, 0.9f);
        }


        #region Fish Pooling Using Addressables

        #region Pooling Variables
        #endregion

        #region Pooling Variables new

        [Header("Level Details") , Space(10)]
        [SerializeField] List<LevelDetails> levelDetails;
        #endregion

        #region Fish Pooling Methods

        /// <summary>
        /// Load assets from addressable
        /// </summary>
        void LoadAddressable()
        {
            StartCoroutine(InitAddressable());
        }

        IEnumerator InitAddressable()
        {
            Debug.Log("Initializing addressable............");
            AsyncOperationHandle<IResourceLocator> handle = Addressables.InitializeAsync();
            yield return handle;
            Debug.Log("Handle returned............");
            if (handle.IsDone)
            {
                Debug.Log("Handled properly. Now creating pool............");
                Addressables.InitializeAsync().Completed += CreatePoolCallback;
                Debug.Log("No error while creating pool");
            }
        }


        /// <summary>
        /// Addressable callback
        /// </summary>
        /// <param name="obj"></param>
        private void CreatePoolCallback(AsyncOperationHandle<IResourceLocator> obj)
        {
            StartCoroutine(StartGameOnLoadingCompleted());
            StartCoroutine(ProgressHandler());
            foreach(LevelDetails details in levelDetails)
            {
                StartCoroutine(CreateFishPool(details.characterPool, details.poolAmount, details.characterReferences , details.characterReference, details.disabledCharacterPool));
            }
        }



        /// <summary>
        /// Create fish pool from addressabls
        /// </summary>
        /// <param name="poolList"></param>
        /// <param name="poolAmount"></param>
        /// <param name="references"></param>
        /// <param name="reference"></param>
        IEnumerator CreateFishPool(List<GameObject> poolList, int poolAmount, AssetReferenceGameObject[] references, AssetReferenceGameObject reference,
            List<GameObject> disabledList)
        {
            yield return null;

            if (references.Length > 0)
            {
                foreach (AssetReferenceGameObject argb in references)
                {
                    totalReferences.Add(argb);
                    //var load = argb.LoadAssetAsync<GameObject>();
                    argb.LoadAssetAsync<GameObject>().Completed += (init) =>
                    {
                        currentLoadedFishesAmount += 1;
                        //Debug.Log(currentLoadedFishesAmount);
                        //downloadBar.fillAmount = currentLoadedFishesAmount / totalFishes;
                        //Debug.Log("###### " + init.Result + " ######");
                        for (int j = 0; j < poolAmount; j++)
                        {
                            GameObject fish = Instantiate(init.Result, transform);
                            fish.SetActive(false);
                            if (fish.GetComponent<FishController>().characterType == FishController.CharacterType.Normal)
                                poolList.Add(fish);
                            else
                                disabledList.Add(fish);
                        }

                        if(disabledList.Count > 0)
                        {
                            //Debug.Log("********* Disable list 1**********");
                            StartCoroutine(SpecialCharacterDisableList(poolList, disabledList));
                        }
                    };

                }
            }
            else if (references.Length == 0)
            {
                reference.LoadAssetAsync<GameObject>().Completed += (init) =>
                {
                    currentLoadedFishesAmount += 1;
                    //downloadBar.fillAmount = currentLoadedFishesAmount / totalFishes;
                    
                    for (int i = 0; i < poolAmount; i++)
                    {
                        GameObject fish = Instantiate(init.Result, transform);
                        fish.SetActive(false);
                        if (fish.GetComponent<FishController>().characterType == FishController.CharacterType.Normal)
                            poolList.Add(fish);
                        else
                            disabledList.Add(fish);
                    }

                    if(disabledList.Count > 0)
                    {
                        //Debug.Log("********* Disable list 2**********");
                        StartCoroutine(SpecialCharacterDisableList(poolList, disabledList));
                    }
                };

                totalReferences.Add(reference);
            }
        }

        IEnumerator SpecialCharacterDisableList(List<GameObject> pool , List<GameObject> disableList)
        {
            yield return new WaitForSeconds(disableList[0].GetComponent<FishController>().respawnTimeForSpecialCharacter);
            pool.AddRange(disableList);
            disableList.Clear();
        }



        IEnumerator ProgressHandler()
        {
            Debug.Log("Start calculating progress");
            yield return new WaitUntil(() => totalReferences.Count == totalFishes);

            assetsDownloadProgrerss = 0;

            while(assetsDownloadProgrerss < 1)
            {
                foreach(AssetReference argb in totalReferences)
                {
                    assetsDownloadProgrerss += argb.OperationHandle.PercentComplete;
                }

                assetsDownloadProgrerss = assetsDownloadProgrerss / totalReferences.Count;
                downloadBar.fillAmount = assetsDownloadProgrerss;
                if (assetsDownloadProgrerss > 1)
                    assetsDownloadProgrerss = 1;
                downloadProgressText.text = "Loading Assets " + (assetsDownloadProgrerss * 100).ToString() + "%";
                if (assetsDownloadProgrerss == 1)
                    downloadProgressText.text = "Please Wait ... "; 
                //Debug.Log("progress " + assetsDownloadProgrerss);
                yield return null;
            }
        }


        IEnumerator StartGameOnLoadingCompleted()
        {
            Debug.Log("Checking");
            //Debug.Log(currentLoadedFishesAmount)
            yield return new WaitUntil(() => currentLoadedFishesAmount == totalFishes);

            StartGame();

            Debug.Log("All assets loaded");
        }


        /// <summary>
        /// Get fish from fish pool (from poolList)
        /// </summary>
        /// <param name="poolList"></param>
        /// <returns></returns>
        GameObject GetFish(List<GameObject> poolList)
        {
            if(poolList.Count > 0)
            {
                GameObject fish = poolList[Random.Range(0, poolList.Count)];
                //foreach(GameObject gb in poolList)
                //{
                //    if (!gb.activeInHierarchy)
                //        return gb;
                //}

                if (!fish.activeInHierarchy)
                {
                    return fish;
                }
                else
                {
                    //GetFish(poolList);

                    foreach (GameObject gb in poolList)
                    {
                        if (!gb.activeInHierarchy)
                            return gb;
                    }
                }
            }
            
            return null;
        }


        /// <summary>
        /// Put Back object in pool
        /// </summary>
        /// <param name="_fish"></param>
        public void PutBackToPool(GameObject _fish)
        {
            _fish.SetActive(false);
            _fish.transform.localPosition = Vector3.zero;

            if (_fish.GetComponent<FishController>().locked)
            {
                _fish.GetComponent<FishController>().locked = false;
                //canonObject.lockedTargetGb = null;
                //canonObject.isTargetLocked = false;
            }
        }

        #endregion

        #endregion

        #region Probability

        /// <summary>
        /// Calculate cummulative probability of fishes
        /// </summary>
        void CreateCummulativeProbability()
        {
            float c = 0;
            //for (int i = 0; i < probabilitiesvalue.Length; i++)
            //{
            //    c += probabilitiesvalue[i];
            //    cummulativeProbab.Add(c);
            //}

            foreach(LevelDetails details in levelDetails)
            {
                c += details.probability;
                cummulativeProbab.Add(c);
            }

            Debug.Log("Probability calculated.....");
        }


        /// <summary>
        /// Spawn fishes according to probability
        /// Cummulative probability is used 
        /// </summary>
        void Probability()
        { 
            int random = Random.Range(0, (int)cummulativeProbab[cummulativeProbab.Count - 1] + 1);
            //Debug.Log(random);
            Spawn(random);
        }

        void Spawn(int random)
        {

            if(1 == 1)
            {
                    Debug.Log("FishGame");

                    GameObject currentCharacter = null;
                    int characterIndex = 0;

                    if (random <= cummulativeProbab[0])
                    {
                         characterIndex = 0;
                         currentCharacter = SpawnFish(levelDetails[0].characterPool);
                    }
                    else if (random <= cummulativeProbab[1] && random > cummulativeProbab[0])
                    {
                          characterIndex = 1;
                          currentCharacter = SpawnFish(levelDetails[1].characterPool);
                     }
                    else if (random <= cummulativeProbab[2] && random > cummulativeProbab[1])
                    {
                          characterIndex = 2;
                         currentCharacter = SpawnFish(levelDetails[2].characterPool);
                    }
                    else if (random <= cummulativeProbab[3] && random > cummulativeProbab[2])
                    {
                           characterIndex = 3;
                          currentCharacter = SpawnFish(levelDetails[3].characterPool);
                    }
                    else if (random <= cummulativeProbab[4] && random > cummulativeProbab[3])
                    {
                         characterIndex = 4;
                          currentCharacter = SpawnFish(levelDetails[4].characterPool);
                    }
                    else if (random <= cummulativeProbab[5] && random > cummulativeProbab[4])
                    {
                         characterIndex = 5;
                         currentCharacter = SpawnFish(levelDetails[5].characterPool);
                    }
                    else if(random <= cummulativeProbab[6] && random > cummulativeProbab[5])
                    {
                          characterIndex = 6;
                          currentCharacter = SpawnFish(levelDetails[6].characterPool);
                    }
                    else if (random <= cummulativeProbab[7] && random > cummulativeProbab[6])
                    {
                            characterIndex = 7;
                            currentCharacter = SpawnFish(levelDetails[7].characterPool);
                    }
                    else if (random <= cummulativeProbab[8] && random > cummulativeProbab[7])
                    {
                            characterIndex = 8;
                            currentCharacter = SpawnFish(levelDetails[8].characterPool);
                    }
                    else if (random <= cummulativeProbab[9] && random > cummulativeProbab[8])
                    {
                            characterIndex = 9;
                            currentCharacter = SpawnFish(levelDetails[9].characterPool);
                    }
                    else if (random <= cummulativeProbab[10] && random > cummulativeProbab[9])
                    {
                            characterIndex = 10;
                            currentCharacter = SpawnFish(levelDetails[10].characterPool);
                    }
                    else if (random <= cummulativeProbab[11] && random > cummulativeProbab[10])
                    {
                            characterIndex = 11;
                            currentCharacter = SpawnFish(levelDetails[11].characterPool);
                    }
                    else if (random <= cummulativeProbab[12] && random > cummulativeProbab[11])
                    {
                            characterIndex = 12;
                            currentCharacter = SpawnFish(levelDetails[12].characterPool);
                    }

                    if(currentCharacter == null)
                    {
                            characterIndex = 0;
                            currentCharacter = SpawnFish(levelDetails[0].characterPool);
                    }

                    SpecialCharacterManage(currentCharacter , characterIndex);
            }
        }


        /// <summary>
        /// Remove special character from pool for few seconds
        /// </summary>
        /// <param name="currentCharacter"></param>
        /// <param name="characterIndex"></param>
        void SpecialCharacterManage(GameObject currentCharacter , int characterIndex)
        {
            if(currentCharacter != null)
            {
                //if (currentCharacter.GetComponent<FishController>().characterType == FishController.CharacterType.Special)
                //{
                    //specialCharacterDisableList.Add(levelDetails[characterIndex]);
                    for (int i = 0; i < levelDetails[characterIndex].characterPool.Count; i++)
                    {
                        levelDetails[characterIndex].disabledCharacterPool.Add(levelDetails[characterIndex].characterPool[i]);
                    }

                    float time = levelDetails[characterIndex].characterPool[0].GetComponent<FishController>().respawnTimeForSpecialCharacter;
                    levelDetails[characterIndex].characterPool.Clear();
                    StartCoroutine(SwapDisabledCharacterPool(time, characterIndex));
                //}
            }
        }

        IEnumerator SwapDisabledCharacterPool(float time , int index)
        {
            yield return new WaitForSeconds(time);
            for(int i = 0; i < levelDetails[index].disabledCharacterPool.Count; i++)
            {
                levelDetails[index].characterPool.Add(levelDetails[index].disabledCharacterPool[i]);
            }

            levelDetails[index].disabledCharacterPool.Clear();
        }


        #endregion


        /// <summary>
        /// Spawn Fishes in the scene
        /// </summary>
        /// <param name="poolList"></param>
        public GameObject SpawnFish(List<GameObject> poolList)
        {

            GameObject fish = GetFish(poolList);


            if (fish != null)
            {

                fish.SetActive(true);

                int selectPath = Random.Range(0, 2);

                if(levelNumber == 1 || levelNumber == 2 || levelNumber == 3)
                {
                    switch (selectPath)
                    {
                        case 0:                        //for left to right paths
                            if (fish.tag == "Group")    //only for group fishes
                            {
                                var p1 = straightPathLtoR[Random.Range(0, straightPathLtoR.Length - 1)];
                                fish.GetComponent<FishController>().pathCreator = p1;
                                foreach (Transform child in fish.transform)
                                {
                                    child.GetChild(0).gameObject.SetActive(true);
                                    child.GetChild(0).GetComponent<FishController>().speed = 0;
                                    //child.GetChild(0).GetChild(0).rotation = Quaternion.Euler(90, 90, 0);
                                }

                            }
                            else              //for other fishes
                            {
                                AdjustSizeAndLayer(fish);

                                if (fish.tag == "Phoenix" ||
                                    //fish.tag == "Mermaid" ||
                                    //fish.tag == "Dragon" ||
                                    fish.tag == "Turtle" ||
                                    fish.tag == "Whale" ||
                                    fish.tag == "Seahorse" ||
                                    fish.tag == "Phoenix")
                                {
                                    var p1 = lessCurvePathLtoR[Random.Range(0, lessCurvePathLtoR.Length - 1)];
                                    fish.GetComponent<FishController>().pathCreator = p1;
                                }
                                else if (fish.tag == "Panda" ||
                                 fish.tag == "Wizard" ||
                                 fish.tag == "Crocodile" ||
                                 fish.tag == "Bull" ||
                                 fish.tag == "Emperior")
                                {
                                    var p2 = groundPathLToR[0];
                                    fish.GetComponent<FishController>().pathCreator = p2;
                                }
                                else if(fish.tag == "Emperior2")
                                {
                                    var p13 = groundPathLToR[1];
                                    fish.GetComponent<FishController>().pathCreator = p13;
                                }
                                else if(fish.tag == "Mermaid")
                                {
                                    var p3 = straightPathLtoR[Random.Range(0, straightPathLtoR.Length - 1)];
                                    fish.GetComponent<FishController>().pathCreator = p3;
                                }
                                else if (fish.tag == "MermaidPattern1")
                                {
                                    var p6 = groundPathRToL[0];
                                    fish.GetComponent<FishController>().pathCreator = p6;
                                }
                                else if (fish.tag == "MermaidPattern2")
                                {
                                    var p7 = groundPathRToL[0];
                                    fish.GetComponent<FishController>().pathCreator = p7;
                                }
                                else if (fish.tag == "MermaidPattern3")
                                {
                                    var p8 = groundPathRToL2[0];
                                    fish.GetComponent<FishController>().pathCreator = p8;
                                }
                                else if (fish.tag == "MermaidPattern4")
                                {
                                    var p9 = groundPathRToL3[0];
                                    fish.GetComponent<FishController>().pathCreator = p9;
                                }
                                else if (fish.tag == "MermaidPattern5")
                                {
                                    var p10 = groundPathRToL[0];
                                    fish.GetComponent<FishController>().pathCreator = p10;
                                }
                                else if (fish.tag == "MermaidPattern6")
                                {
                                    var p11 = groundPathRToL2[0];
                                    fish.GetComponent<FishController>().pathCreator = p11;
                                }
                                else if (fish.tag == "MermaidPattern7")
                                {
                                    var p12 = groundPathRToL3[0];
                                    fish.GetComponent<FishController>().pathCreator = p12;
                                }
                                else if (fish.tag == "MermaidPattern8")
                                {
                                    var p14 = groundPathRToL[0];
                                    fish.GetComponent<FishController>().pathCreator = p14;
                                }
                                else if (fish.tag == "MermaidPattern9")
                                {
                                    var p15 = groundPathRToL2[0];
                                    fish.GetComponent<FishController>().pathCreator = p15;
                                }
                                else if (fish.tag == "MermaidPattern10")
                                {
                                    var p16 = groundPathRToL3[0];
                                    fish.GetComponent<FishController>().pathCreator = p16;
                                }
                                else if (fish.tag == "MermaidPattern11")
                                {
                                    var p17 = groundPathRToL[0];
                                    fish.GetComponent<FishController>().pathCreator = p17;
                                }
                                else if (fish.tag == "MermaidPattern12")
                                {
                                    var p18 = groundPathRToL2[0];
                                    fish.GetComponent<FishController>().pathCreator = p18;
                                }
                                else if (fish.tag == "MermaidPattern13")
                                {
                                    var p19 = groundPathRToL3[0];
                                    fish.GetComponent<FishController>().pathCreator = p19;
                                }
                                else if(fish.tag == "Dragon")
                                {
                                    var p4 = dragonPathLToR[Random.Range(0, dragonPathLToR.Length - 1)];
                                    fish.GetComponent<FishController>().pathCreator = p4;
                                }
                                else if(fish.tag == "Crab")
                                {
                                    var p5 = emperiorPath[Random.Range(0, emperiorPath.Length - 1)];
                                    fish.GetComponent<FishController>().pathCreator = p5;
                                }
                                else if(fish.tag == "FishPattern1")
                                {
                                    var p20 = fishPath[0];
                                    fish.GetComponent<FishController>().pathCreator = p20;
                                }
                                else if(fish.tag == "FishPattern2")
                                {
                                    var p21 = fishPath[1];
                                    fish.GetComponent<FishController>().pathCreator = p21;
                                }
                                else if(fish.tag == "FishPattern3")
                                {
                                    var p22 = fishPath[2];
                                    fish.GetComponent<FishController>().pathCreator = p22;
                                }
                                else if(fish.tag == "FishPattern4")
                                {
                                    var p24 = fishPath[0];
                                    fish.GetComponent<FishController>().pathCreator = p24;
                                }
                                else if(fish.tag == "FishPattern5")
                                {
                                    var p25 = fishPath[1];
                                    fish.GetComponent<FishController>().pathCreator = p25;
                                }
                                else if(fish.tag == "FishPattern6")
                                {
                                    var p26 = fishPath[2];
                                    fish.GetComponent<FishController>().pathCreator = p26;
                                }
                                else if(fish.tag == "FishPattern7")
                                {
                                    var p27 = fishPath[3];
                                    fish.GetComponent<FishController>().pathCreator = p27;
                                }
                                else if(fish.tag == "FishPattern8")
                                {
                                    var p28 = fishPath[4];
                                    fish.GetComponent<FishController>().pathCreator = p28;
                                }
                                else if(fish.tag == "FishPattern9")
                                {
                                    var p29 = fishPath[5];
                                    fish.GetComponent<FishController>().pathCreator = p29;
                                }
                                else if(fish.tag == "FishPattern10")
                                {
                                    var p30 = fishPath[6];
                                    fish.GetComponent<FishController>().pathCreator = p30;
                                }
                                else if(fish.tag == "FishPattern11")
                                {
                                    var p31 = fishPath[1];
                                    fish.GetComponent<FishController>().pathCreator = p31;
                                }
                                else if(fish.tag == "FishPattern12")
                                {
                                    var p32 = fishPath[2];
                                    fish.GetComponent<FishController>().pathCreator = p32;
                                }
                                else if(fish.tag == "FishPattern13")
                                {
                                    var p33 = fishPath[3];
                                    fish.GetComponent<FishController>().pathCreator = p33;
                                }
                                else if(fish.tag == "FishPattern14")
                                {
                                    var p34 = fishPath[4];
                                    fish.GetComponent<FishController>().pathCreator = p34;
                                }
                                else if(fish.tag == "FishPattern15")
                                {
                                    var p35 = fishPath[0];
                                    fish.GetComponent<FishController>().pathCreator = p35;
                                }
                                else if(fish.tag == "PatternRayFish1")
                                {
                                    var p38 = fishingpatternPath[0];
                                    fish.GetComponent<FishController>().pathCreator = p38;
                                }
                                else if(fish.tag == "PatternRayFish2")
                                {
                                    var p39 = fishingpatternPath[1];
                                    fish.GetComponent<FishController>().pathCreator = p39;
                                }
                                else if(fish.tag == "PatternRayFish3")
                                {
                                    var p40 = fishingpatternPath[2];
                                    fish.GetComponent<FishController>().pathCreator = p40;
                                }
                                else if(fish.tag == "PatternRayFish4")
                                {
                                    var p41 = fishingpatternPath[3];
                                    fish.GetComponent<FishController>().pathCreator = p41;
                                }
                                else if(fish.tag == "PatternRayFish5")
                                {
                                    var p42 = fishingpatternPath[4];
                                    fish.GetComponent<FishController>().pathCreator = p42;
                                }
                                else if(fish.tag == "PatternWhale1")
                                {
                                    var p43 = fishingpatternPath[0];
                                    fish.GetComponent<FishController>().pathCreator = p43;
                                }
                                else if(fish.tag == "PatternWhale2")
                                {
                                    var p44 = fishingpatternPath[1];
                                    fish.GetComponent<FishController>().pathCreator = p44;
                                }
                                else if(fish.tag == "PatternWhale3")
                                {
                                    var p44 = fishingpatternPath[2];
                                    fish.GetComponent<FishController>().pathCreator = p44;
                                }
                                else if(fish.tag == "PatternWhale4")
                                {
                                    var p45 = fishingpatternPath[3];
                                    fish.GetComponent<FishController>().pathCreator = p45;
                                }
                                else if(fish.tag == "PatternWhale5")
                                {
                                    var p46 = fishingpatternPath[4];
                                    fish.GetComponent<FishController>().pathCreator = p46;
                                }
                                else if(fish.tag == "RushFishRed1")
                                {
                                    var p47 = fishrushPath[0];
                                    fish.GetComponent<FishController>().pathCreator = p47;
                                }
                                else if(fish.tag == "RushFishRed2")
                                {
                                    var p48 = fishrushPath[1];
                                    fish.GetComponent<FishController>().pathCreator = p48;
                                }
                                else if(fish.tag == "RushFishRed3")
                                {
                                    var p49 = fishrushPath[2];
                                    fish.GetComponent<FishController>().pathCreator = p49;
                                }
                                else if(fish.tag == "RushFishRed4")
                                {
                                    var p50 = fishrushPath[3];
                                    fish.GetComponent<FishController>().pathCreator = p50;
                                }
                                else if(fish.tag == "RushFishRed5")
                                {
                                    var p51 = fishrushPath[4];
                                    fish.GetComponent<FishController>().pathCreator = p51;
                                }
                                else if(fish.tag == "RushFishRed6")
                                {
                                    var p52 = fishrushPath[5];
                                    fish.GetComponent<FishController>().pathCreator = p52;
                                }
                                else if(fish.tag == "RushFishRed7")
                                {
                                    var p53 = fishrushPath[6];
                                    fish.GetComponent<FishController>().pathCreator = p53;
                                }
                                else if(fish.tag == "RushFishRed8")
                                {
                                    var p54 = fishrushPath[7];
                                    fish.GetComponent<FishController>().pathCreator = p54;
                                }
                                else if(fish.tag == "RushFishRed9")
                                {
                                    var p55 = fishrushPath[8];
                                    fish.GetComponent<FishController>().pathCreator = p55;
                                }
                                else if(fish.tag == "RushFishRed10")
                                {
                                    var p56 = fishrushPath[9];
                                    fish.GetComponent<FishController>().pathCreator = p56;
                                }
                                else if(fish.tag == "RushFishRed11")
                                {
                                    var p57 = fishrushPath[10];
                                    fish.GetComponent<FishController>().pathCreator = p57;
                                }
                                else if(fish.tag == "RushFishRed12")
                                {
                                    var p58 = fishrushPath[11];
                                    fish.GetComponent<FishController>().pathCreator = p58;
                                }
                                else if(fish.tag == "RushFishRed13")
                                {
                                    var p59 = fishrushPath[12];
                                    fish.GetComponent<FishController>().pathCreator = p59;
                                }
                                else if(fish.tag == "RushFishRed14")
                                {
                                    var p60 = fishrushPath[13];
                                    fish.GetComponent<FishController>().pathCreator = p60;
                                }
                                else if(fish.tag == "RushFishRed15")
                                {
                                    var p61 = fishrushPath[14];
                                    fish.GetComponent<FishController>().pathCreator = p61;
                                }
                                else if(fish.tag == "RushFishRed16")
                                {
                                    var p62 = fishrushPath[15];
                                    fish.GetComponent<FishController>().pathCreator = p62;
                                }
                                else if(fish.tag == "RushFishRed17")
                                {
                                    var p63 = fishrushPath[16];
                                    fish.GetComponent<FishController>().pathCreator = p63;
                                }
                                else if(fish.tag == "RushFishRed18")
                                {
                                    var p64 = fishrushPath[17];
                                    fish.GetComponent<FishController>().pathCreator = p64;
                                }
                                else if(fish.tag == "RushFishRed19")
                                {
                                    var p65 = fishrushPath[18];
                                    fish.GetComponent<FishController>().pathCreator = p65;
                                }
                                else if(fish.tag == "RushFishRed20")
                                {
                                    var p66 = fishrushPath[19];
                                    fish.GetComponent<FishController>().pathCreator = p66;
                                }
                                else if(fish.tag == "RushFishRed21")
                                {
                                    var p67 = fishrushPath[20];
                                    fish.GetComponent<FishController>().pathCreator = p67;
                                }
                                else if(fish.tag == "RushFishRed22")
                                {
                                    var p68 = fishrushPath[21];
                                    fish.GetComponent<FishController>().pathCreator = p68;
                                }
                                else if(fish.tag == "RushFishRed23")
                                {
                                    var p69 = fishrushPath[22];
                                    fish.GetComponent<FishController>().pathCreator = p69;
                                }
                                else if(fish.tag == "RushFishRed24")
                                {
                                    var p70 = fishrushPath[23];
                                    fish.GetComponent<FishController>().pathCreator = p70;
                                }
                                else if(fish.tag == "RushFishRed25")
                                {
                                    var p68 = fishrushPath[24];
                                    fish.GetComponent<FishController>().pathCreator = p68;
                                }
                                else if(fish.tag == "RushFishRed26")
                                {
                                    var p69 = fishrushPath[25];
                                    fish.GetComponent<FishController>().pathCreator = p69;
                                }
                                else if(fish.tag == "RushFishRed27")
                                {
                                    var p70 = fishrushPath[26];
                                    fish.GetComponent<FishController>().pathCreator = p70;
                                }
                                else if(fish.tag == "RushFishRed28")
                                {
                                    var p71 = fishrushPath[27];
                                    fish.GetComponent<FishController>().pathCreator = p71;
                                }
                                else if(fish.tag == "RushFishRayFish1")
                                {
                                    var p72 = fishrushPath[28];
                                    fish.GetComponent<FishController>().pathCreator = p72;
                                }
                                else if(fish.tag == "RushFishRayFish2")
                                {
                                    var p73 = fishrushPath[29];
                                    fish.GetComponent<FishController>().pathCreator = p73;
                                }
                                else if(fish.tag == "RushFishWhale1")
                                {
                                    var p74 = fishrushPath[28];
                                    fish.GetComponent<FishController>().pathCreator = p74;
                                }
                                else if(fish.tag == "RushFishWhale2")
                                {
                                    var p75 = fishrushPath[29];
                                    fish.GetComponent<FishController>().pathCreator = p75;
                                }
                                else if(fish.tag == "RushFishRay1")
                                {
                                    var p76 = fishrushPath[28];
                                    fish.GetComponent<FishController>().pathCreator = p76;
                                }
                                else if(fish.tag == "RushFishRay2")
                                {
                                    var p77 = fishrushPath[29];
                                    fish.GetComponent<FishController>().pathCreator = p77;
                                }
                                else if(fish.tag == "GreenDragon")
                                {
                                    var p78 = dragonPathLToR[Random.Range(0, dragonPathLToR.Length - 1)];
                                    fish.GetComponent<FishController>().pathCreator = p78;
                                }
                                else
                                {
                                    var p = pathCreatorLToR[Random.Range(0, pathCreatorLToR.Length - 1)];
                                    fish.GetComponent<FishController>().pathCreator = p;
                                }

                                
                                
                                if (fish.tag == "Shark"
                                    || fish.tag == "TropicalFish"
                                    || fish.tag == "RayFish"
                                    || fish.tag == "Dragon"
                                    || fish.tag == "Turtle"
                                    || fish.tag == "Panda" 
                                    || fish.tag == "Wizard"
                                    || fish.tag == "Whale" 
                                    || fish.tag == "Seahorse" 
                                    || fish.tag == "Phoenix"
                                    || fish.tag == "Crocodile"
                                    || fish.tag == "Bull"
                                    || fish.tag == "Crab"
                                    || fish.tag == "MonsterFish"
                                    || fish.tag == "Octopus"
                                    || fish.tag == "AngularFish"
                                    )
                                {
                                    //fish.transform.GetChild(0).localRotation = Quaternion.Euler(0, 180, 180);
                                }
                                    //fish.transform.GetChild(0).localRotation = Quaternion.Euler(0, 180, 180);

                            }

                            break;
                        case 1:                             //for right to left path
                            if (fish.tag == "Group")        //for group fishes
                            {
                                var q1 = straightPathRtoL[Random.Range(0, straightPathRtoL.Length - 1)];
                                fish.GetComponent<FishController>().pathCreator = q1;
                                foreach (Transform child in fish.transform)
                                {
                                    child.GetChild(0).gameObject.SetActive(true);
                                    child.GetChild(0).GetComponent<FishController>().speed = 0;
                                    //child.GetChild(0).GetChild(0).rotation = Quaternion.Euler(-90, -90,0);
                                }
                            }
                            else                            //for other fishes
                            {
                                AdjustSizeAndLayer(fish);
                                if (fish.tag == "Shark" ||
                                   //fish.tag == "Mermaid" ||
                                   //fish.tag == "Dragon" ||
                                   fish.tag == "Turtle" ||
                                   fish.tag == "Whale" ||
                                   fish.tag == "Seahorse" ||
                                   fish.tag == "Phoenix")
                                {
                                    var p1 = lessCurvePathRtoL[Random.Range(0, lessCurvePathRtoL.Length - 1)];
                                    fish.GetComponent<FishController>().pathCreator = p1;
                                }
                                else if (fish.tag == "Panda" ||
                                   fish.tag == "Wizard" ||
                                   fish.tag == "Crocodile" ||
                                   fish.tag == "Bull" ||
                                   fish.tag == "Crab")
                                {
                                    var p2 = groundPathRToL[0];
                                    fish.GetComponent<FishController>().pathCreator = p2;
                                }
                                else if (fish.tag == "MermaidPattern1")
                                {
                                    var p6 = groundPathRToL[0];
                                    fish.GetComponent<FishController>().pathCreator = p6;
                                }
                                else if (fish.tag == "MermaidPattern2")
                                {
                                    var p7 = groundPathRToL[0];
                                    fish.GetComponent<FishController>().pathCreator = p7;
                                }
                                else if (fish.tag == "MermaidPattern3")
                                {
                                    var p8 = groundPathRToL2[0];
                                    fish.GetComponent<FishController>().pathCreator = p8;
                                }
                                else if (fish.tag == "MermaidPattern4")
                                {
                                    var p9 = groundPathRToL3[0];
                                    fish.GetComponent<FishController>().pathCreator = p9;
                                }
                                else if (fish.tag == "MermaidPattern5")
                                {
                                    var p10 = groundPathRToL[0];
                                    fish.GetComponent<FishController>().pathCreator = p10;
                                }
                                else if (fish.tag == "MermaidPattern6")
                                {
                                    var p11 = groundPathRToL2[0];
                                    fish.GetComponent<FishController>().pathCreator = p11;
                                }
                                else if (fish.tag == "MermaidPattern7")
                                {
                                    var p12 = groundPathRToL3[0];
                                    fish.GetComponent<FishController>().pathCreator = p12;
                                }
                                else if (fish.tag == "MermaidPattern8")
                                {
                                    var p15 = groundPathRToL[0];
                                    fish.GetComponent<FishController>().pathCreator = p15;
                                }
                                else if (fish.tag == "MermaidPattern9")
                                {
                                    var p16 = groundPathRToL2[0];
                                    fish.GetComponent<FishController>().pathCreator = p16;
                                }
                                else if (fish.tag == "MermaidPattern10")
                                {
                                    var p17 = groundPathRToL3[0];
                                    fish.GetComponent<FishController>().pathCreator = p17;
                                }
                                else if (fish.tag == "MermaidPattern11")
                                {
                                    var p18 = groundPathRToL[0];
                                    fish.GetComponent<FishController>().pathCreator = p18;
                                }
                                else if (fish.tag == "MermaidPattern12")
                                {
                                    var p19 = groundPathRToL2[0];
                                    fish.GetComponent<FishController>().pathCreator = p19;
                                }
                                else if (fish.tag == "MermaidPattern13")
                                {
                                    var p20 = groundPathRToL3[0];
                                    fish.GetComponent<FishController>().pathCreator = p20;
                                }
                                else if(fish.tag == "Emperior")
                                {
                                    var p14 = groundPathLToR[0];
                                    fish.GetComponent<FishController>().pathCreator = p14;
                                }
                                else if(fish.tag == "Emperior2")
                                {
                                    var p13 = groundPathLToR[1];
                                    fish.GetComponent<FishController>().pathCreator = p13;
                                }
                                else if (fish.tag == "Mermaid")
                                {
                                    var p3 = straightPathRtoL[Random.Range(0, straightPathRtoL.Length - 1)];
                                    fish.GetComponent<FishController>().pathCreator = p3;
                                }
                                else if (fish.tag == "Dragon")
                                {
                                    var p4 = dragonPathRToL[Random.Range(0, dragonPathRToL.Length - 1)];
                                    fish.GetComponent<FishController>().pathCreator = p4;
                                }
                                else if(fish.tag == "Crab")
                                {
                                    var p5 = emperiorPath[Random.Range(0, emperiorPath.Length - 1)];
                                    fish.GetComponent<FishController>().pathCreator = p5;
                                }
                                else if(fish.tag == "FishPattern1")
                                {
                                    var p21 = fishPath[0];
                                    fish.GetComponent<FishController>().pathCreator = p21;
                                }
                                else if(fish.tag == "FishPattern2")
                                {
                                    var p22 = fishPath[1];
                                    fish.GetComponent<FishController>().pathCreator = p22;
                                }
                                else if(fish.tag == "FishPattern3")
                                {
                                    var p23 = fishPath[2];
                                    fish.GetComponent<FishController>().pathCreator = p23;
                                }
                                else if(fish.tag == "FishPattern4")
                                {
                                    var p25 = fishPath[0];
                                    fish.GetComponent<FishController>().pathCreator = p25;
                                }
                                else if(fish.tag == "FishPattern5")
                                {
                                    var p26 = fishPath[1];
                                    fish.GetComponent<FishController>().pathCreator = p26;
                                }
                                else if(fish.tag == "FishPattern6")
                                {
                                    var p27 = fishPath[2];
                                    fish.GetComponent<FishController>().pathCreator = p27;
                                }
                                else if(fish.tag == "FishPattern7")
                                {
                                    var p28 = fishPath[3];
                                    fish.GetComponent<FishController>().pathCreator = p28;
                                }
                                else if(fish.tag == "FishPattern8")
                                {
                                    var p29 = fishPath[4];
                                    fish.GetComponent<FishController>().pathCreator = p29;
                                }
                                else if(fish.tag == "FishPattern9")
                                {
                                    var p30 = fishPath[5];
                                    fish.GetComponent<FishController>().pathCreator = p30;
                                }
                                else if(fish.tag == "FishPattern10")
                                {
                                    var p31 = fishPath[6];
                                    fish.GetComponent<FishController>().pathCreator = p31;
                                }
                                else if(fish.tag == "FishPattern11")
                                {
                                    var p32 = fishPath[1];
                                    fish.GetComponent<FishController>().pathCreator = p32;
                                }
                                else if(fish.tag == "FishPattern12")
                                {
                                    var p33 = fishPath[2];
                                    fish.GetComponent<FishController>().pathCreator = p33;
                                }
                                else if(fish.tag == "FishPattern13")
                                {
                                    var p34 = fishPath[3];
                                    fish.GetComponent<FishController>().pathCreator = p34;
                                }
                                else if(fish.tag == "FishPattern14")
                                {
                                    var p35 = fishPath[4];
                                    fish.GetComponent<FishController>().pathCreator = p35;
                                }
                                else if(fish.tag == "FishPattern15")
                                {
                                    var p36 = fishPath[0];
                                    fish.GetComponent<FishController>().pathCreator = p36;
                                }
                                /*else if(fish.tag == "CrabStrike1")
                                {
                                    var p37 = crabStrikePath[0];
                                    fish.GetComponent<FishController>().pathCreator = p37;
                                }
                                else if(fish.tag == "CrabStrike2")
                                {
                                    var p38 = crabStrikePath[1];
                                    fish.GetComponent<FishController>().pathCreator = p38;
                                }*/
                                else if(fish.tag == "PatternRayFish1")
                                {
                                    var p38 = fishingpatternPath[0];
                                    fish.GetComponent<FishController>().pathCreator = p38;
                                }
                                else if(fish.tag == "PatternRayFish2")
                                {
                                    var p39 = fishingpatternPath[1];
                                    fish.GetComponent<FishController>().pathCreator = p39;
                                }
                                else if(fish.tag == "PatternRayFish3")
                                {
                                    var p40 = fishingpatternPath[2];
                                    fish.GetComponent<FishController>().pathCreator = p40;
                                }
                                else if(fish.tag == "PatternRayFish4")
                                {
                                    var p41 = fishingpatternPath[3];
                                    fish.GetComponent<FishController>().pathCreator = p41;
                                }
                                else if(fish.tag == "PatternRayFish5")
                                {
                                    var p42 = fishingpatternPath[4];
                                    fish.GetComponent<FishController>().pathCreator = p42;
                                }
                                else if(fish.tag == "PatternWhale1")
                                {
                                    var p43 = fishingpatternPath[0];
                                    fish.GetComponent<FishController>().pathCreator = p43;
                                }
                                else if(fish.tag == "PatternWhale2")
                                {
                                    var p44 = fishingpatternPath[1];
                                    fish.GetComponent<FishController>().pathCreator = p44;
                                }
                                else if(fish.tag == "PatternWhale3")
                                {
                                    var p44 = fishingpatternPath[2];
                                    fish.GetComponent<FishController>().pathCreator = p44;
                                }
                                else if(fish.tag == "PatternWhale4")
                                {
                                    var p45 = fishingpatternPath[3];
                                    fish.GetComponent<FishController>().pathCreator = p45;
                                }
                                else if(fish.tag == "PatternWhale5")
                                {
                                    var p46 = fishingpatternPath[4];
                                    fish.GetComponent<FishController>().pathCreator = p46;
                                }
                                else if(fish.tag == "RushFishRed1")
                                {
                                    var p47 = fishrushPath[0];
                                    fish.GetComponent<FishController>().pathCreator = p47;
                                }
                                else if(fish.tag == "RushFishRed2")
                                {
                                    var p48 = fishrushPath[1];
                                    fish.GetComponent<FishController>().pathCreator = p48;
                                }
                                else if(fish.tag == "RushFishRed3")
                                {
                                    var p49 = fishrushPath[2];
                                    fish.GetComponent<FishController>().pathCreator = p49;
                                }
                                else if(fish.tag == "RushFishRed4")
                                {
                                    var p50 = fishrushPath[3];
                                    fish.GetComponent<FishController>().pathCreator = p50;
                                }
                                else if(fish.tag == "RushFishRed5")
                                {
                                    var p51 = fishrushPath[4];
                                    fish.GetComponent<FishController>().pathCreator = p51;
                                }
                                else if(fish.tag == "RushFishRed6")
                                {
                                    var p52 = fishrushPath[5];
                                    fish.GetComponent<FishController>().pathCreator = p52;
                                }
                                else if(fish.tag == "RushFishRed7")
                                {
                                    var p53 = fishrushPath[6];
                                    fish.GetComponent<FishController>().pathCreator = p53;
                                }
                                else if(fish.tag == "RushFishRed8")
                                {
                                    var p54 = fishrushPath[7];
                                    fish.GetComponent<FishController>().pathCreator = p54;
                                }
                                else if(fish.tag == "RushFishRed9")
                                {
                                    var p55 = fishrushPath[8];
                                    fish.GetComponent<FishController>().pathCreator = p55;
                                }
                                else if(fish.tag == "RushFishRed10")
                                {
                                    var p56 = fishrushPath[9];
                                    fish.GetComponent<FishController>().pathCreator = p56;
                                }
                                else if(fish.tag == "RushFishRed11")
                                {
                                    var p57 = fishrushPath[10];
                                    fish.GetComponent<FishController>().pathCreator = p57;
                                }
                                else if(fish.tag == "RushFishRed12")
                                {
                                    var p58 = fishrushPath[11];
                                    fish.GetComponent<FishController>().pathCreator = p58;
                                }
                                else if(fish.tag == "RushFishRed13")
                                {
                                    var p59 = fishrushPath[12];
                                    fish.GetComponent<FishController>().pathCreator = p59;
                                }
                                else if(fish.tag == "RushFishRed14")
                                {
                                    var p60 = fishrushPath[13];
                                    fish.GetComponent<FishController>().pathCreator = p60;
                                }
                                else if(fish.tag == "RushFishRed15")
                                {
                                    var p61 = fishrushPath[14];
                                    fish.GetComponent<FishController>().pathCreator = p61;
                                }
                                else if(fish.tag == "RushFishRed16")
                                {
                                    var p62 = fishrushPath[15];
                                    fish.GetComponent<FishController>().pathCreator = p62;
                                }
                                else if(fish.tag == "RushFishRed17")
                                {
                                    var p63 = fishrushPath[16];
                                    fish.GetComponent<FishController>().pathCreator = p63;
                                }
                                else if(fish.tag == "RushFishRed18")
                                {
                                    var p64 = fishrushPath[17];
                                    fish.GetComponent<FishController>().pathCreator = p64;
                                }
                                else if(fish.tag == "RushFishRed19")
                                {
                                    var p65 = fishrushPath[18];
                                    fish.GetComponent<FishController>().pathCreator = p65;
                                }
                                else if(fish.tag == "RushFishRed20")
                                {
                                    var p66 = fishrushPath[19];
                                    fish.GetComponent<FishController>().pathCreator = p66;
                                }
                                else if(fish.tag == "RushFishRed21")
                                {
                                    var p67 = fishrushPath[20];
                                    fish.GetComponent<FishController>().pathCreator = p67;
                                }
                                else if(fish.tag == "RushFishRed22")
                                {
                                    var p68 = fishrushPath[21];
                                    fish.GetComponent<FishController>().pathCreator = p68;
                                }
                                else if(fish.tag == "RushFishRed23")
                                {
                                    var p69 = fishrushPath[22];
                                    fish.GetComponent<FishController>().pathCreator = p69;
                                }
                                else if(fish.tag == "RushFishRed24")
                                {
                                    var p70 = fishrushPath[23];
                                    fish.GetComponent<FishController>().pathCreator = p70;
                                }
                                else if(fish.tag == "RushFishRed25")
                                {
                                    var p68 = fishrushPath[24];
                                    fish.GetComponent<FishController>().pathCreator = p68;
                                }
                                else if(fish.tag == "RushFishRed26")
                                {
                                    var p69 = fishrushPath[25];
                                    fish.GetComponent<FishController>().pathCreator = p69;
                                }
                                else if(fish.tag == "RushFishRed27")
                                {
                                    var p70 = fishrushPath[26];
                                    fish.GetComponent<FishController>().pathCreator = p70;
                                }
                                else if(fish.tag == "RushFishRed28")
                                {
                                    var p71 = fishrushPath[27];
                                    fish.GetComponent<FishController>().pathCreator = p71;
                                }
                                else if(fish.tag == "RushFishRayFish1")
                                {
                                    var p72 = fishrushPath[28];
                                    fish.GetComponent<FishController>().pathCreator = p72;
                                }
                                else if(fish.tag == "RushFishRayFish2")
                                {
                                    var p73 = fishrushPath[29];
                                    fish.GetComponent<FishController>().pathCreator = p73;
                                }
                                else if(fish.tag == "RushFishWhale1")
                                {
                                    var p74 = fishrushPath[28];
                                    fish.GetComponent<FishController>().pathCreator = p74;
                                }
                                else if(fish.tag == "RushFishWhale2")
                                {
                                    var p75 = fishrushPath[29];
                                    fish.GetComponent<FishController>().pathCreator = p75;
                                }
                                else if(fish.tag == "RushFishRay1")
                                {
                                    var p76 = fishrushPath[28];
                                    fish.GetComponent<FishController>().pathCreator = p76;
                                }
                                else if(fish.tag == "RushFishRay2")
                                {
                                    var p77 = fishrushPath[29];
                                    fish.GetComponent<FishController>().pathCreator = p77;
                                }
                                else if(fish.tag == "GreenDragon")
                                {
                                    var p78 = dragonPathLToR[Random.Range(0, dragonPathRToL.Length - 1)];
                                    fish.GetComponent<FishController>().pathCreator = p78;
                                }
                                else
                                {
                                    var p = pathCreatorRToL[Random.Range(0, pathCreatorRToL.Length - 1)];
                                    fish.GetComponent<FishController>().pathCreator = p;
                                }

                                if (fish.tag == "Shark"
                                   || fish.tag == "TropicalFish"
                                   || fish.tag == "RayFish"
                                   || fish.tag == "Dragon"
                                   || fish.tag == "Turtle"
                                   || fish.tag == "Panda"
                                   || fish.tag == "Wizard"
                                   || fish.tag == "Whale"
                                   || fish.tag == "Seahorse"
                                   || fish.tag == "Phoenix"
                                   || fish.tag == "Crocodile"
                                   || fish.tag == "Bull"
                                   || fish.tag == "Crab"
                                   || fish.tag == "MonsterFish"
                                   || fish.tag == "Octopus"
                                   || fish.tag == "AngularFish"
                                   )
                                {
                                    //fish.transform.GetChild(0).localRotation = Quaternion.Euler(180, 180, 180);
                                }
                                    
                            }

                            break;
                    }
                }
                else if (levelNumber == 4)
                {
                    switch(selectPath)
                    {
                        case 0:
                            if (fish.tag == "Ant" ||
                                fish.tag == "Caterpiller" ||
                                fish.tag == "Scorpion" ||
                                fish.tag == "Spider")
                            {
                                var q = lessCurvePathLtoR[Random.Range(0, lessCurvePathLtoR.Length - 1)];
                                fish.GetComponent<FishController>().pathCreator = q;
                                fish.transform.GetChild(0).localRotation = Quaternion.Euler(0, 180, 180);
                            }
                            else
                            {
                                var q = pathCreatorLToR[Random.Range(0, pathCreatorLToR.Length - 1)];
                                fish.GetComponent<FishController>().pathCreator = q;
                                fish.transform.GetChild(0).localRotation = Quaternion.Euler(0, 180, 180);
                            }
                            break;
                        case 1:
                            if (fish.tag == "Ant" ||
                                fish.tag == "Caterpiller" ||
                                fish.tag == "Scorpion" ||
                                fish.tag == "Spider")
                            {
                                var q = lessCurvePathRtoL[Random.Range(0, lessCurvePathRtoL.Length - 1)];
                                fish.GetComponent<FishController>().pathCreator = q;
                                fish.transform.GetChild(0).localRotation = Quaternion.Euler(180, 180, 180);
                            }
                            else
                            {
                                var q = pathCreatorRToL[Random.Range(0, pathCreatorRToL.Length - 1)];
                                fish.GetComponent<FishController>().pathCreator = q;
                                fish.transform.GetChild(0).localRotation = Quaternion.Euler(180, 180, 180);
                            }
                            break;
                    }
                }         
            }

            return fish;
        }


     

        void AdjustSizeAndLayer(GameObject fish)
        {

           /* int layerNo = Random.Range(-5, -11);

            fish.GetComponentInChildren<SpriteRenderer>().sortingOrder = layerNo;

            switch (layerNo)
            {
                case -5:
                    fish.transform.localScale = new Vector3(1f, 1f, 1f);
                    break;
                case -6:
                    fish.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                    break;
                case -7:
                    fish.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    break;
                case -8:
                    fish.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    break;
                case -9:
                    fish.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    break;
                case -10:
                    fish.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
            }*/
        }

    }
}
