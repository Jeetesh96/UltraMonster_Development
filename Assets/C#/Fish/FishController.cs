using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TongitsGo;
using System;

public class FishController : MonoBehaviour
{

    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    [HideInInspector] public float speed;
    public float speeds = 0f;

    //public Vector2 dynamicSpeed;

    public float distanceTravelled;

    public float maxHealth = 100;
    public float damagePoint;
    [SerializeField] int scorePoint;
    public bool locked;
    public float respawnTimeForSpecialCharacter = 0;
    [SerializeField] int deadEffectSize;
    [SerializeField] Gradient deadEffectGradient;

    //bool enable = false;
    
    //public GameObject deathEffect;

    //public Leaderboard leaderboard;
    
    public enum CharacterType
    {
        Normal,
        Group,
        Special
    }
    public CharacterType characterType;

    public enum DiedBy
    {
        player,
        bot
    }

    void Start()
    {
        //speed = UnityEngine.Random.Range(dynamicSpeed.x, dynamicSpeed.y);

        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    void Update()
    {
        
        if (characterType == CharacterType.Normal || characterType == CharacterType.Special)
            Movement();
    }

    void Movement()
    {
        //Debug.Log("#####*************8888");
        if (pathCreator != null)
        {
            //Debug.Log("##### ");
            distanceTravelled += speeds * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
           // var v = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
           // transform.rotation = new Quaternion(0, 0, v.z, v.w);

            if (pathCreator.path.length <= distanceTravelled)
            {
                FishManager.instance.PutBackToPool(this.gameObject);
                distanceTravelled = 0;
                maxHealth = 100;
            }
        }


        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 360);
        if(this.tag == "Mermaid")
        {
            if(this.pathCreator.gameObject.transform.parent.name.Contains("RtoL"))
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180);
            }
        }

        else if(this.tag == "Group")
        {
            //transform.rotation = Quaternion.Euler(0 , 0 , 0);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
             collision.transform.parent.GetComponent<Canon>().PutObjectInPool(collision.gameObject);
            //BlastEffectPooling.instance.GetObjectFromPool(this.transform.position);

            maxHealth -= damagePoint;
            if (maxHealth <= 0)
            {
                OnDie(DiedBy.player);
                ScoreScript.scoreValue += scorePoint;

                //leaderboard.SubmitScoreRoutine(ScoreScript.scoreValue);
            }
            //leaderboard.SubmitScoreRoutine(ScoreScript.scoreValue);
        }
        else if (collision.tag == "BotBullet")
        {
            collision.transform.parent.GetComponent<CanonBot>().PutObjectInPool(collision.gameObject);
            //BlastEffectPooling.instance.GetObjectFromPool(this.transform.position);

            maxHealth -= damagePoint;
            if (maxHealth <= 0)
            {
                OnDie(DiedBy.bot);
                //ScoreScript.scoreValue += scorePoint;
            }
        }
    }

    /// <summary>
    /// Recieve damage from bullets.
    /// called by bulletBehaviour class when raycasting is true
    /// </summary>
    public void RecieveDamage()
    {
        maxHealth -= damagePoint;
        if (maxHealth <= 0)
        {
            OnDie(DiedBy.player);
        }
    }

    /// <summary>
    /// After death of fish
    /// </summary>
    /// <param name="diedBy"></param>
    public void OnDie(DiedBy diedBy)
    {

        CoinsEffectPooling.instance.GetObjectFromPool(this.transform.position);

        if (this.transform.parent.tag == "Group")
        {
            this.gameObject.SetActive(false);
        }
        else
            FishManager.instance.PutBackToPool(this.gameObject);


        distanceTravelled = 0;
        maxHealth = 100;

        switch (diedBy)
        {
            case DiedBy.player:
                //deathEffect.SetActive(true);
                GameManager.Instance.playerScore += scorePoint;
                FishManager.instance.playerScoreText.text = GameManager.Instance.playerScore.ToString();
                break;
            case DiedBy.bot:
                GameManager.Instance.enemyScore += scorePoint;
                //FishManager.instance.enemyScoreText.text = GameManager.Instance.enemyScore.ToString();
                break;
        }



    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        Debug.Log("End of path");
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
