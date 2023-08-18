using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFishGroup : MonoBehaviour
{

    [SerializeField] List<GameObject> fishesPrefabs = new List<GameObject>();

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        GameObject randomFish = fishesPrefabs[Random.Range(0 , fishesPrefabs.Count)];
        randomFish.GetComponent<FishController>().enabled = false;
        randomFish.GetComponent<Rigidbody>().isKinematic = true;
        //Destroy(randomFish.GetComponent<Rigidbody>());
        foreach(Transform trans in transform.transform)
        {
            GameObject gb = Instantiate(randomFish, trans);
            gb.GetComponent<FishController>().characterType = FishController.CharacterType.Group;
            gb.GetComponent<FishController>().damagePoint = 100;
            gb.transform.localPosition = Vector3.zero;
            //gb.transform.rotation = Quaternion.Euler(0, 90 , 0);
            //gb.transform.localScale = new Vector3(trans.localScale.x, trans.localScale.y, trans.localScale.z);
        }
    }
}
