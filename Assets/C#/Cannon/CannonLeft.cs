using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CannonLeft : CannonParentClass
{
    #region Variables

    [SerializeField] GameObject crossHairGb;
    public bool isTargetLocked = false;
    [HideInInspector] public bool isSelectingTarget = false;

    //[SerializeField] List<GameObject> allCharacters = new List<GameObject>();
    [SerializeField] Transform fishParent;

    public GameObject lockedTargetGb;
    [SerializeField] CannonManager cannonManager;
    [SerializeField] bool isLaser = false;
    [SerializeField] LineRenderer laser;


    public enum ControlArea
    {
        LeftScreen,
        RightScreen,
        LeftButtom,
        CenterBottom,
        RightBottom,
        LeftTop,
        RightTop,
        CenterTop
    }

    [SerializeField] ControlArea controlArea;
    private List<Vector2> controlAreaPoints = new List<Vector2>();

    #endregion

    private void Start()
    {
        CreateBulletPool();
    }

    /// <summary>
    /// Split the screen in 9 parts;
    /// </summary>
    void ScreenSplit()
    {
        float xDivideStep = Screen.width / 3;
        float yDividSetp = Screen.height / 2;
        controlAreaPoints.Add(new Vector2(xDivideStep, yDividSetp));
        controlAreaPoints.Add(new Vector2(2 * xDivideStep, yDividSetp));
        controlAreaPoints.Add(new Vector2(3 * xDivideStep, yDividSetp));

        controlAreaPoints.Add(new Vector2(xDivideStep, 2 * yDividSetp));
        controlAreaPoints.Add(new Vector2(2 * xDivideStep, 2 * yDividSetp));
        controlAreaPoints.Add(new Vector2(3 * xDivideStep, 2 * yDividSetp));

    }

    override protected void ControlTurret()
    {

        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;

            #region Ignore Characters in between Selected fish and Cannon
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(touchPos), cam.transform.forward);
            if (hit.collider != null)
            {
                //hitDistance = hit.distance;
                Debug.Log("Hitting");

                if (hit.collider.transform.parent.gameObject.layer == 6)
                {
                    foreach (Transform child in fishParent)
                    {
                        if (child.name == hit.collider.transform.gameObject.name)
                        {
                            if (child.GetComponent<BoxCollider2D>() != null)
                                child.GetComponent<BoxCollider2D>().enabled = true;
                        }
                        else
                        {
                            if (child.GetComponent<BoxCollider2D>() != null)
                                child.GetComponent<BoxCollider2D>().enabled = false;
                        }
                    }
                }
            }
            else
            {
                //Debug.Log("not Hitting");
                foreach (Transform child in fishParent)
                {
                    if (child.GetComponent<BoxCollider2D>() != null)
                        child.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
            #endregion

            float hitDistance = 0;

            #region Laser
            if (isLaser)
            {
                Vector2 v = (Vector2)cam.ScreenToWorldPoint(touchPos) - (Vector2)laser.transform.position;


                RaycastHit2D laserHit = Physics2D.Raycast(firingPoint.position, firingPoint.up);
                Debug.DrawRay(firingPoint.position, firingPoint.up * laserHit.distance, Color.red);

                hitDistance = laserHit.distance;
                //Debug.Log(laserHit.collider);

                if (laserHit.collider != null)
                {
                    FishController fishContoller = laserHit.collider.GetComponent<FishController>();
                    if (fishContoller != null)
                    {
                        fishContoller.maxHealth -= fishContoller.damagePoint / 20;
                        if (fishContoller.maxHealth <= 0)
                            fishContoller.OnDie(FishController.DiedBy.player);
                    }
                }
            }


            #endregion


            Vector2 turretNormal = turret.transform.up;

            if (Input.touchCount > 0)
            {

                //Return if touch any UI
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Debug.Log("UI hit!");
                    return;
                }

                switch (controlArea)
                {
                    case ControlArea.LeftButtom:
                        //if (touch.position.x > 0 && touch.position.x <= Screen.width / 3) //&& touch.position.y < Screen.height / 2)
                        //{
                        TurretBehaviour(turretNormal, touch, hitDistance);
                        //}
                        //else
                        //  crossHairGb.SetActive(false);
                        break;
                        /*case ControlArea.CenterBottom:
                            if (touch.position.x > Screen.width / 3 && touch.position.x <= 2 * Screen.width / 3) //&& touch.position.y < Screen.height / 2)
                            {
                                TurretBehaviour(turretNormal, touch , hitDistance);
                            }
                            else
                                crossHairGb.SetActive(false);
                            break;
                        case ControlArea.RightBottom:
                            if (touch.position.x > 2 * Screen.width / 3 && touch.position.x <= Screen.width) //&& touch.position.y < Screen.height / 2)
                            {
                                TurretBehaviour(turretNormal, touch , hitDistance);
                            }
                            else
                                crossHairGb.SetActive(false);
                            break;*/
                        /*case ControlArea.RightTop:
                            if (touch.position.x > 2 * Screen.width / 3 && touch.position.x <= Screen.width && touch.position.y > Screen.height / 2)
                            {
                                TurretBehaviour(turretNormal, touch);
                            }
                            else
                                crossHairGb.SetActive(false);
                            break;
                        case ControlArea.CenterTop:
                            if (touch.position.x > Screen.width / 3 && touch.position.x <= 2 * Screen.width / 3 && touch.position.y > Screen.height / 2)
                            {
                                TurretBehaviour(turretNormal, touch);
                            }
                            else
                                crossHairGb.SetActive(false);
                            break;
                        case ControlArea.LeftTop:
                            if (touch.position.x > 0 && touch.position.x <= Screen.width / 3 && touch.position.y > Screen.height / 2)
                            {
                                TurretBehaviour(turretNormal, touch);
                            }
                            else
                                crossHairGb.SetActive(false);
                            break;*/
                }

                #region Half Split
                var dir = cam.ScreenToWorldPoint(touch.position) - turret.transform.position;
                       if (controlArea == ControlArea.LeftScreen)
                       {
                        //Dividing the screen and making the left side active
                           if (touch.position.x < Screen.width / 2)
                           {
                               crossHairGb.SetActive(true);
                               crossHairGb.transform.position = (Vector2)cam.ScreenToWorldPoint(touch.position);
                               dir = crossHairGb.transform.position - turret.transform.position;

                               float theta = Vector2.SignedAngle(turretNormal, dir);
                               turret.transform.rotation = Quaternion.Euler(0, 0, turret.transform.rotation.eulerAngles.z + theta);
                               Firing();
                           }
                           else
                               crossHairGb.SetActive(false);
                       }
                       /*else if(controlArea == ControlArea.RightScreen)
                       {
                           if (touch.position.x > Screen.width / 2)
                           {
                               crossHairGb.SetActive(true);
                               crossHairGb.transform.position = (Vector2)cam.ScreenToWorldPoint(touch.position);
                               dir = crossHairGb.transform.position - turret.transform.position;

                               float theta = Vector2.SignedAngle(turretNormal, dir);
                               turret.transform.rotation = Quaternion.Euler(0, 0, turret.transform.rotation.eulerAngles.z + theta);
                               Firing();
                           }
                           else
                               crossHairGb.SetActive(false);
                       }*/
                #endregion
            }
            else
            {
                var dir = (lockedTargetGb.transform.position - lockedTargetGb.transform.right * 20) - turret.transform.position;

                //Debug.Log((lockedTargetGb.transform.position - lockedTargetGb.transform.right * 10));

                float theta = Vector2.SignedAngle(turretNormal, dir);
                crossHairGb.SetActive(true);
                crossHairGb.transform.position = lockedTargetGb.transform.position;
                turret.transform.rotation = Quaternion.Euler(0, 0, turret.transform.rotation.eulerAngles.z + theta);

                Firing(laser, hitDistance);
            }

            //if (turret.transform.rotation.eulerAngles.z >= 90 && turret.transform.rotation.eulerAngles.z < 180)
            //    turret.transform.rotation = Quaternion.Euler(0, 0, 90);
            //else if (turret.transform.rotation.eulerAngles.z <= 270 && turret.transform.rotation.eulerAngles.z > 180)
            //    turret.transform.rotation = Quaternion.Euler(0 , 0 , -90);


            //Debug.DrawLine(turret.transform.position, dir, Color.red);

        }
        else
        {
            foreach (Transform child in fishParent)
            {
                if (child.GetComponent<BoxCollider2D>() != null)
                    child.GetComponent<BoxCollider2D>().enabled = true;
            }
            crossHairGb.SetActive(false);
            if (isLaser)
            {
                laser.SetPosition(1, new Vector3(0, 0, 0));
                laser.gameObject.SetActive(false);
            }

        }

    }

    public bool isDebug = false;
    void TurretBehaviour(Vector2 turretNormal, Touch touch, float hitDistance)
    {
        //if(!isDebug)
        //    cannonManager.ManageBetweenPlayerAndBot(this.gameObject);
        crossHairGb.SetActive(true);
        crossHairGb.transform.position = (Vector2)cam.ScreenToWorldPoint(touch.position);
        var dir = crossHairGb.transform.position - turret.transform.position;

        float theta = Vector2.SignedAngle(turretNormal, dir);
        turret.transform.rotation = Quaternion.Euler(0, 0, turret.transform.rotation.eulerAngles.z + theta);
        if (isLaser)
        {
            Firing(laser, hitDistance);
        }
        else
            Firing();
    }



    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;


    public void SelectTarget()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                clicked++;

                if (clicked == 1)
                    clicktime = Time.time;

                if (clicked > 1 && Time.time - clicktime < clickdelay)
                {
                    clicked = 0;
                    clicktime = 0;
                    Touch touch = Input.GetTouch(0);
                    Vector2 touchPos = touch.position;

                    RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(touchPos), cam.transform.forward);
                    if (hit.collider != null)
                    {
                        string tag = hit.collider.tag;
                        if (tag == "Shark" || tag == "TropicalFish" || tag == "Mermaid" || tag == "Group" || tag == "Turtle" || tag == "Dragon")
                        {
                            lockedTargetGb = hit.collider.gameObject;
                            isTargetLocked = true;
                            hit.collider.GetComponent<FishController>().locked = true;
                        }
                    }

                    Debug.Log("double clicked");
                }
                else if (clicked > 2 || Time.time - clicktime > 1)
                    clicked = 0;
            }
        }
    }


    protected override void Update()
    {

        SelectTarget();
        if (!isSelectingTarget)
        {
            ControlTurret();
        }

    }
}