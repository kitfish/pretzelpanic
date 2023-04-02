using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class RobotCashier : MonoBehaviour
{

    public static RobotCashier Instance { get; private set; }

    public event EventHandler OnRobotBreakdown;
    public event EventHandler OnRobotRepair;

    private bool isBroken = false;
    private float breakdownTimer = 4f;
    private float breakdownTimerMin = 4f;
    private float breakdownTimerMax = 10f;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        breakdownTimer = UnityEngine.Random.Range(breakdownTimerMin, breakdownTimerMax);
    }

    private void Update()
    {
        if (!isBroken)
        {
            breakdownTimer -= Time.deltaTime;

            if (breakdownTimer < 0)
            {
                isBroken = true;
                OnRobotBreakdown?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            // broadcast some message about starting and hows there's already a line or something
        }
    }

    public void RepairRobot()
    {
        isBroken = false;
        breakdownTimer = UnityEngine.Random.Range(breakdownTimerMin, breakdownTimerMax);
        OnRobotRepair?.Invoke(this, EventArgs.Empty);
    }

    public bool IsBroken()
    {
        return isBroken;
    }

}
