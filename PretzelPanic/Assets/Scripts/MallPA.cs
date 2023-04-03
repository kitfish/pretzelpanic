using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MallPA : MonoBehaviour
{
    public static MallPA Instance { get; private set; }

    [SerializeField] private AudioSource[] mallPAAudioSourceArray;


    private void Awake()
    {
        Instance = this;
    }

    public void Announce()
    {
        
    }

}
