using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : MonoBehaviour
{


    private void Start()
    {
        RobotCashier.Instance.OnRobotBreakdown += RobotCashier_OnRobotBreakdown;
        RobotCashier.Instance.OnRobotRepair += RobotCashier_OnRobotRepair;
    }

    private void RobotCashier_OnRobotRepair(object sender, System.EventArgs e)
    {
        //throw new System.NotImplementedException();
    }

    private void RobotCashier_OnRobotBreakdown(object sender, System.EventArgs e)
    {
        //throw new System.NotImplementedException();
    }
}
