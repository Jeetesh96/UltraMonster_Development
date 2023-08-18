using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsTimeObjectDestroyer : MonoBehaviour
{

    public enum EffectType
    {
        Coins,
        Web
    }

    public EffectType Effect_Type;

    void Start()
    {
        Invoke("PutBackToPoolThisObj", 1.5f);
    }


    /// <summary>
    /// Put the object to its pool after time
    /// </summary>
    /// <returns></returns>
    void PutBackToPoolThisObj()
    {
        if (Effect_Type == EffectType.Web)
        {
            WebEffectPool.instance.PutObjectBackToPool(this.gameObject);
        }
        else if (Effect_Type == EffectType.Coins)
        {

            CoinsEffectPooling.instance.PutObjectBackToPool(this.gameObject);
        }
    }
}
