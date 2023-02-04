using System;
using System.Collections;
using System.Collections.Generic;
using Flower;
using Roots;
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
            if (transform.position.x < GameSettings.Instance.BoundaryMax.x)
            {
                spider.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            else
            {
                float x = startx;
                float y = Mathf.Clamp(Random.Range(GameSettings.Instance.BoundaryMin.y, GameSettings.Instance.BoundaryMax.y), GameSettings.Instance.BoundaryMin.y, GameSettings.Instance.BoundaryMax.y);
                spider.transform.position = new Vector3(x, y, GameSettings.Instance.BoundaryMax.z);
                
            }
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
