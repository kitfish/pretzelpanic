using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PasserbyManager : MonoBehaviour
{
    
    public static PasserbyManager Instance { get; private set; }

    [SerializeField] Passerby passerby;

    private float passerbySpawnTimer = 0f;
    private float passerbySpawnTimerMax = 4f;
    private Vector3 passerbyLeftSpawnLocation = new Vector3(20f, 0f, 10f);
    private Vector3 passerbyRightSpawnLocation = new Vector3(-20f, 0f, 7f);
    private float passerbySpawnZMin = -1.5f;
    private float passerbySpawnZMax = 1.5f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        passerbySpawnTimer -= Time.deltaTime;
        if (passerbySpawnTimer < 0)
        {
            // Random.Range works differently with ints than floats.
            // float (minInclusive, maxInclusive)
            // int (minInclusive, maxExclusive) what?
            int passerbyDirection = UnityEngine.Random.Range(0, 2);
            Vector3 passerbySpawnLocation = Vector3.zero;

            if (passerbyDirection == 0)
            {
                passerbySpawnLocation = passerbyLeftSpawnLocation;
                float passerbySpawnZOffset = UnityEngine.Random.Range(passerbySpawnZMin, passerbySpawnZMax);
                passerbySpawnLocation.z -= passerbySpawnZOffset;
            }
            else
            {
                passerbySpawnLocation = passerbyRightSpawnLocation;
                float passerbySpawnZOffset = UnityEngine.Random.Range(passerbySpawnZMin, passerbySpawnZMax);
                passerbySpawnLocation.z -= passerbySpawnZOffset;
            }

            Passerby newPasserby = Instantiate(passerby, passerbySpawnLocation, Quaternion.identity);



            newPasserby.AssignDirecton(passerbyDirection);
            passerbySpawnTimer = passerbySpawnTimerMax;
        }

        
    }


}
