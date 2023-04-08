using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronManager : MonoBehaviour
{
    [SerializeField] private GameObject patronGameObject;
    [SerializeField] private int patronListSizeMax;
    private List<GameObject> patronListArray;
    private int patronSpawmAmount = 5; //testing at higher value than the max time so patrons dont spawn
    private int patronSpawmAmountMax = 4;
    private float patronSpawnTimer = 0; 
    private float patronSpawnTimerMax = 3f;

    public static PatronManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        patronListArray = new List<GameObject>(patronListSizeMax);
    }

    private void Update()
    {
        patronSpawnTimer -= Time.deltaTime;

        if (patronSpawnTimer < 0)
        {
            patronSpawnTimer = patronSpawnTimerMax;
            if (patronSpawmAmount < patronSpawmAmountMax)
            {
                SpawnPatron();
            }
        }

    }

    private void SpawnPatron()
    {
        GameObject patron = Instantiate(patronGameObject, new Vector3(20f, 1f, 7f), Quaternion.identity);
        
    }


}
