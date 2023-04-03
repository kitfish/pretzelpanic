using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTextUI : MonoBehaviour
{
    private void Start()
    {
        RobotCashier.Instance.OnRobotBreakdown += RobotCashier_OnRobotBreakdown;
        RobotCashier.Instance.OnRobotRepair += RobotCashier_OnRobotRepair;

        Hide();
    }

    private void RobotCashier_OnRobotRepair(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void RobotCashier_OnRobotBreakdown(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
