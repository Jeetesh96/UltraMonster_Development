using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastEffectPooling : ObjectPooling
{
    public static BlastEffectPooling instance;

    private void Start()
    {
        instance = this;
        CreatePool();
        Invoke("Destroy", 0.5f);
    }

    /*void Destroy()
    {
        EffectsTimeObjectDestroyer.effectTime.PutBackToPoolThisObj();
        Debug.Log("Destroy");
    }*/
}