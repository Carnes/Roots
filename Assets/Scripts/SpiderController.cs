using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpiderController : MonoBehaviour
{
    public GameObject spider;
    public float speed = .50f;
    public bool isWalking = false;
    private Vector3 offset;
    private Animator _animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    public void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        MoveSpider();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            spider.transform.position += new Vector3(speed*Time.deltaTime, 0, 0); 
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

}
