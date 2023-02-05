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
    public float speed = .50f;
    public bool isWalking = false;
    private Vector3 offset;
    private Animator _animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private bool moveSpiderRight = true;
    private bool moveSpiderLeft = false;
    private bool moveSpiderDown = false;
    private bool moveSpiderUp = false;

    public void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        var startx = GameSettings.Instance.BoundaryMin.x;
        var starty = Random.Range(GameSettings.Instance.BoundaryMin.y, GameSettings.Instance.BoundaryMax.y);
        transform.position = new Vector3(startx, starty, GameSettings.Instance.BoundaryMax.z);
        MoveSpider();
    }

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
        }
    }

    void WalkSpiderRight()
    {
        _animator.SetFloat(Speed, speed);
        
        if (transform.position.x < GameSettings.Instance.BoundaryMax.x)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.Euler(0, 90, -90);
        }
        else
        {
            GetSpiderRotateDirection();
        }
    }

    void WalkSpiderLeft()
    {
        _animator.SetFloat(Speed, speed);
        
        if (transform.position.x > GameSettings.Instance.BoundaryMin.x)
        {
            transform.position += new Vector3((-1 * speed) * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.Euler(180, 90, -90);
        }
        else
        {
            GetSpiderRotateDirection();
        }
    }

    private float ActionCountDown;
    private static readonly int Speed = Animator.StringToHash("speed");

    void WalkSpiderDown()
    {
        transform.position += new Vector3(0, (-1 * speed) * Time.deltaTime, 0);
        transform.rotation = Quaternion.Euler(90, 90, -90);

        ActionCountDown -= Time.deltaTime;

        if (IsBelowBottomBoundary || IsAboveTopBoundary)
            ActionCountDown = 0f;

        if (ActionCountDown <= 0)
        {
            if (transform.position.x > GameSettings.Instance.BoundaryMax.x)
            {
                moveSpiderDown = false;
                moveSpiderUp = false;
                moveSpiderLeft = true;
                moveSpiderRight = false;
            }
            else
            {
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
        transform.rotation = Quaternion.Euler(270, 90, -90);
        ActionCountDown -= Time.deltaTime;

        if (IsBelowBottomBoundary || IsAboveTopBoundary)
            ActionCountDown = 0f;

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
        var y = transform.position.y;
        var min = GameSettings.Instance.BoundaryMin;
        var max = GameSettings.Instance.BoundaryMax;

        if (y > min.y && y < max.y)
        {
            var isHeads = Random.value < 0.5f;
            if (isHeads)
                ChangeDirectionGoUp();
            else
                ChangeDirectionGoDown();
        }
        else if (IsAboveTopBoundary)
        {
            ChangeDirectionGoDown();
        }
        else if (IsBelowBottomBoundary)
        {
            ChangeDirectionGoUp();
        }
    }

    private bool IsAboveTopBoundary => transform.position.y > GameSettings.Instance.BoundaryMax.y;
    private bool IsBelowBottomBoundary => transform.position.y < GameSettings.Instance.BoundaryMin.y;

    private void ChangeDirectionGoUp()
    {
        moveSpiderUp = true;
        moveSpiderDown = false;
        moveSpiderLeft = false;
        moveSpiderRight = false;
        ResetActionCountDown();
    }

    private void ChangeDirectionGoDown()
    {
        moveSpiderDown = true;
        moveSpiderUp = false;
        moveSpiderLeft = false;
        moveSpiderRight = false;
        ResetActionCountDown();
    }

    private void ResetActionCountDown()
    {
        ActionCountDown = Random.Range(5, 25) / speed;
    }

    [ContextMenu("MoveSpider")]
    void MoveSpider()
    {
        isWalking = true;
        _animator.SetBool(IsWalking, isWalking);
    }

    [ContextMenu("StopSpider")]
    void StopSpider()
    {
        isWalking = false;
        _animator.SetBool(IsWalking, isWalking);
    }

    public bool HandleRootPartCollision(RootPart rootPart, Collider collidingPart)
    {
        if (moveSpiderRight)
        {
            moveSpiderUp = false;
            moveSpiderDown = false;
            moveSpiderLeft = true;
            moveSpiderRight = false;
        }
        else
        {
            moveSpiderUp = false;
            moveSpiderDown = false;
            moveSpiderLeft = false;
            moveSpiderRight = true;
        }

        Debug.Log($"Spider strike!");
        return true; // true means destroy root part
    }
}