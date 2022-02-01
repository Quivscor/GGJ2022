using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonumentsSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource monument1;
    [SerializeField]
    private AudioSource monument2;
    [SerializeField]
    private AudioSource monument3;

    public void PlayMonument1()
    {
        monument1.Play();
    }
    public void PlayMonument2()
    {
        monument2.Play();
    }
    public void PlayMonument3()
    {
        monument3.Play();
    }
}

