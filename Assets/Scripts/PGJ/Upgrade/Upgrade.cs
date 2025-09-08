using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    [SerializeField] internal string upgradeInfo;

    public abstract void UpgradeProc();
}
