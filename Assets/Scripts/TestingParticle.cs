using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingParticle : MonoBehaviour
{

    [SerializeField] ParticleSystem pSystem;

    [Header("Emission")]
    [SerializeField] int emissionCap;
    [SerializeField] int emissionAmount;
    [SerializeField] int emissionIncreasePerInput;
    [SerializeField] int emissionDecreasePerFrame;

    [Header("Lifetime")]
    [SerializeField] float lifetimeCap;
    [SerializeField] float lifetime;
    [SerializeField] float lifetimeIncreasePerInput;
    [SerializeField] float lifetimeDecreasePerFrame;

    [Header("Other")]
    [SerializeField] ParticleSystem.MinMaxCurve mmCurve;

    ParticleSystem.EmissionModule emission;
    ParticleSystem.MainModule main;
    
    // Start is called before the first frame update
    void Start()
    {
        emission = pSystem.emission;
        main = pSystem.main;

    }

    
    public void IncreaseIntensity()
    {

        emissionAmount = Mathf.Min(emissionAmount + emissionIncreasePerInput, emissionCap);
        lifetime = Mathf.Min(lifetime + lifetimeIncreasePerInput, lifetimeCap);

    }


    private void FixedUpdate()
    {
        emissionAmount = Mathf.Max(0, emissionAmount - emissionDecreasePerFrame);
        lifetime = Mathf.Max(0, lifetime - lifetimeDecreasePerFrame);

        ParticleSystem.Burst burst = new ParticleSystem.Burst(0, (short)emissionAmount);

        emission.SetBurst(0, burst);

        //mmCurve.constantMax = lifetime;
        mmCurve.constant = lifetime;
        main.startLifetime = mmCurve;


    }


}
