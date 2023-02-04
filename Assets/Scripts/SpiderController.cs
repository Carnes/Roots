using System;
using System.Collections;
using System.Collections.Generic;
using Flower;
using Roots;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpiderController : MonoBehaviour, IRootCollision
{
    public GameObject spider;
    public float speed = .50f;
    public bool isWalking = false;
    private Vector3 offset;
    private Animator _animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private float startx;
    private float endx;
    private string direction;
    private bool rotateSpiderOnce = true;
    private bool moveSpiderRight = true;
    private bool moveSpiderLeft = false;
    private bool moveSpiderDown = false;
    private bool moveSpiderUp = false;

    public void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startx = GameSettings.Instance.BoundaryMin.x;
        transform.position = new Vector3(startx, transform.position.y, transform.position.z);
        MoveSpider();

      // endx     = GameSettings.Instance.BoundaryMax.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking) 
        {
            if (moveSpiderRight)
            {
                WalkSpiderRight();
            }

            if (moveSpiderLeft)
            {
                WalkSpiderLeft();
            }

            if (moveSpiderDown)
            {
                WalkSpiderDown();
            }

            if (moveSpiderUp)
            {
                WalkSpiderUp();
            }

            /*if (transform.position.x < GameSettings.Instance.BoundaryMax.x && moveSpiderRight)
            {

                

                //Debug.Log(transform.position);
            }
            else if (transform.position.x > GameSettings.Instance.BoundaryMin.x) 
            {
                moveSpiderRight = false;*/
                //Debug.Log("greater than or equal to boundary max");
                /*if (rotateSpiderOnce)
                {
                    GetSpiderRotateDirection();
                    WalkSpiderDown();
                    rotateSpiderOnce = false;
                }*/


                
            //}/**/
            //}}
            /*else if(transform.position.y <= GameSettings.Instance.BoundaryMin.y)
            {
                //float x = startx;
                //float y = Mathf.Clamp(Random.Range(GameSettings.Instance.BoundaryMin.y, GameSettings.Instance.BoundaryMax.y), GameSettings.Instance.BoundaryMin.y, GameSettings.Instance.BoundaryMax.y);
                //spider.transform.position = new Vector3(x, y, GameSettings.Instance.BoundaryMax.z);
                
                
            }*/
        }

      
    }

    void WalkSpiderRight()
    {
        if (transform.position.x < GameSettings.Instance.BoundaryMax.x)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            GetSpiderRotateDirection();
            
        }
    }
    void WalkSpiderLeft()
    {
        if (transform.position.x > GameSettings.Instance.BoundaryMin.x)
        {
            transform.position += new Vector3((-1 * speed) * Time.deltaTime, 0, 0);
        }
        else
        {
            GetSpiderRotateDirection();
        }
    }

    private float ActionCountDown;

    void WalkSpiderDown()
    {
        transform.position += new Vector3(0, (-1 * speed) * Time.deltaTime, 0);

        ActionCountDown -= Time.deltaTime;

        if (ActionCountDown <= 0)
        {
            if (transform.position.x > GameSettings.Instance.BoundaryMax.x)
            {
                moveSpiderDown = false;
                moveSpiderUp = false;
                moveSpiderLeft = true;
                moveSpiderRight = false;
                transform.Rotate(0, 90, 0);
            }
            else
            {
                transform.Rotate(0, -90, 0);
                moveSpiderDown = false;
                moveSpiderUp = false;
                moveSpiderLeft = false;
                moveSpiderRight = true;  
            }
            
        }
    }
    void WalkSpiderUp()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        ActionCountDown -= Time.deltaTime;
        if (ActionCountDown <= 0)
        {
            if (transform.position.x < GameSettings.Instance.BoundaryMin.x)
            {
                moveSpiderDown = false;
                moveSpiderUp = false;
                moveSpiderLeft = false;
                moveSpiderRight = true;
            }
            else
            {
                moveSpiderDown = false;
                moveSpiderUp = false;
                moveSpiderLeft = true;
                moveSpiderRight = false;  
            }
        }
    }

    void GetSpiderRotateDirection()
    {
       //first check if spider is near the upper part of the boundary, if so, needs to rotate down
       
       //y max is top
       if (transform.position.y > GameSettings.Instance.BoundaryMin.y)
       {
           transform.Rotate(0, 90, 0);
           moveSpiderDown = true;
           moveSpiderUp = false;
           moveSpiderLeft = false;
           moveSpiderRight = false;
           ActionCountDown = Random.Range(1, 4);

       }
       else if (transform.position.y < GameSettings.Instance.BoundaryMax.y)
       {
           transform.Rotate(0, -90, 0);
           moveSpiderUp = true;
           moveSpiderDown = false;
           moveSpiderLeft = false;
           moveSpiderRight = false;
           ActionCountDown = Random.Range(1, 4);
       }
       


    }

    

    

    [ContextMenu("MoveSpider")]
    void MoveSpider()
    {
        isWalking = true;
        _animator.SetBool(IsWalking, isWalking);
        //spider.transform.position += new Vector3(speed*Time.deltaTime, 0, 0);
    }

    [ContextMenu("StopSpider")]
    void StopSpider()
    {
        isWalking = false;
        _animator.SetBool(IsWalking, isWalking);
        //spider.transform.position += new Vector3(speed*Time.deltaTime, 0, 0);
    }

    public bool HandleRootPartCollision(RootPart rootPart, Collider collidingPart)
    {
        // FIXME - handle spider logic when it hits a root part
        
        Debug.Log($"Spider strike!");
        return true; // true means destroy root part
    }
}
