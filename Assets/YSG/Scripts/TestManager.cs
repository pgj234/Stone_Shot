using UnityEngine;

public class TestManager : MonoBehaviour
{
    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) GameManager.instance.TurnChange();
    }
}
