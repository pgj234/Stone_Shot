using UnityEngine;
using UnityEngine.UI;

public class UpgradeChoiceUI : MonoBehaviour
{
    Upgrade currentUpgrade;

    Text upgradeText;

    void Awake()
    {
        upgradeText = transform.GetChild(0).GetComponent<Text>();
    }

    /// <summary>
    /// 현재 업그레이드 세팅
    /// </summary>
    internal void SetCurrentUpgrade(Upgrade _upInfo)
    {
        currentUpgrade = _upInfo;

        upgradeText.text = currentUpgrade?.upgradeInfo;
    }

    public void ChoiceUpgrade()
    {
        currentUpgrade.UpgradeProc();
    }
}
