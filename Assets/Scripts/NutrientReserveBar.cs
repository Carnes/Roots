using System;
using System.Collections;
using System.Collections.Generic;
using Flower;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NutrientReserveBar : MonoBehaviour
{
    private Slider _nutrientReserveBar;
    public float fillSpeed = 10;
    private ParticleSystem _particleSystem;

    public RootNutrientReserve nutrientReserve;
    public float nutrients;
    
    private void Awake()
    {
        _nutrientReserveBar = gameObject.GetComponent<Slider>();
        _particleSystem = GameObject.Find("NutrientBarParticles").GetComponent<ParticleSystem>();
    }

    public void OnEnable()
    {
        nutrientReserve = RootNutrientReserve.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        nutrientReserve = RootNutrientReserve.Instance;
        nutrients = nutrientReserve.NutrientsInReserve;
        _nutrientReserveBar.value = 0;
    }
    // Update is called once per frame
    void Update()
    {
        nutrients = nutrientReserve.NutrientsInReserve;
        var bufferAmt = 0.05f;
        if (_nutrientReserveBar.value + bufferAmt < nutrients)
        {
            _nutrientReserveBar.value += fillSpeed * Time.deltaTime;
            _particleSystem.Play();
            
        }
        else if (_nutrientReserveBar.value > nutrients + bufferAmt )
        {
            _nutrientReserveBar.value -= fillSpeed * Time.deltaTime;
            _particleSystem.Play();
            
        }
        else
        {
            _particleSystem.Stop();
        }
    }
}
