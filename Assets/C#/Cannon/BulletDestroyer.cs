using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    [SerializeField] Canon canon;
    [SerializeField] CanonBot[] canonBot;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            canon.PutObjectInPool(collision.gameObject);
        }
        else if(collision.tag == "BotBullet")
        {
            foreach (CanonBot c in canonBot)
            {
                if (collision.transform.parent == c.transform)
                {
                    c.PutObjectInPool(collision.gameObject);
                }
            }
        }
    }
}
