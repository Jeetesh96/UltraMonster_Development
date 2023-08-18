using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalloxScroll : MonoBehaviour
{
    public ParalloxObjects parallax;
    public GameObject cloud;

    // Use this for initialization
    void Start()
    {
        if (cloud != null)
        {
            cloud.GetComponent<Rigidbody2D>().velocity = new Vector2(1f, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (parallax != null)
        {

            parallax.Speed = -5.0f;
        }
    }


}
