using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebEffectPool : ObjectPooling
{
    public static WebEffectPool instance;
    private void Start()
    {
        instance = this;
        CreatePool();
    }
}
