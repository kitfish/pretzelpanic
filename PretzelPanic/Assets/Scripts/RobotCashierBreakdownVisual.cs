using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCashierBreakdownVisual : MonoBehaviour
{
    [SerializeField] private GameObject[] robotBreakDownVisualObjectArray;

    private void Start()
    {
        RobotCashier.Instance.OnRobotBreakdown += RobotCashier_OnRobotBreakdown;
        RobotCashier.Instance.OnRobotRepair += RobotCashier_OnRobotRepair;
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
        foreach (GameObject robotCashierBreakdown in robotBreakDownVisualObjectArray)
        {
            robotCashierBreakdown.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject robotCashierBreakdown in robotBreakDownVisualObjectArray)
        {
            robotCashierBreakdown.SetActive(false);
        }
    }
}
