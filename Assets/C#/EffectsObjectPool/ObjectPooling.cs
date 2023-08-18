using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    #region Effect Pooling

    [Header("Object Pooling"), Space(10)]
    [SerializeField] private int poolAmount;
    [SerializeField] private GameObject poolObjectPrefab;
    [SerializeField] private List<GameObject> poolList;


    /// <summary>
    /// Create object pool
    /// </summary>
    public void CreatePool()
    {
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject gb = Instantiate(poolObjectPrefab, transform);
            gb.name = gb.name + i.ToString();
            gb.transform.localPosition = Vector3.zero;
            poolList.Add(gb);
            gb.SetActive(false);
        }
    }

    /// <summary>
    /// Get gameobject from pool
    /// </summary>
    /// <returns></returns>
    public GameObject GetObjectFromPool(Vector3 pos)
    {
        for (int i = 0; i < poolAmount; i++)
        {
            if (!poolList[i].activeInHierarchy)
            {
                poolList[i].SetActive(true);
                poolList[i].transform.position = pos;
                return poolList[i];
            }
        }
        return null;
    }


    /// <summary>
    /// Put back gameobject in pool
    /// </summary>
    /// <param name="gb"></param>
    public void PutObjectBackToPool(GameObject gb)
    {
        gb.transform.localPosition = Vector3.zero;
        gb.SetActive(false);
    }


    #endregion
}
