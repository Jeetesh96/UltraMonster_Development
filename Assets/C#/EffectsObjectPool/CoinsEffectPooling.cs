using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsEffectPooling : ObjectPooling
{
    public static CoinsEffectPooling instance;
    private void Start()
    {
        instance = this;
        CreatePool();
    }
}
