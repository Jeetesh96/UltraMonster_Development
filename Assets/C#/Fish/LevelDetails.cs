using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


[System.Serializable]
public class LevelDetails
{
    public string type;
    public int poolAmount;
    public float probability;
    public AssetReferenceGameObject[] characterReferences;
    public AssetReferenceGameObject characterReference;
    public List<GameObject> characterPool = new List<GameObject>();
    public List<GameObject> disabledCharacterPool = new List<GameObject>();
}

