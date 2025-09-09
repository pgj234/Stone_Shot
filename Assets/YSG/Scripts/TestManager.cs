using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField] GameObject unit_player;
    [SerializeField] GameObject unit_enemy;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            float randomX = Random.Range(-3f, 3f);
            Instantiate(unit_player, new Vector3(randomX, -3f), Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            float randomX = Random.Range(-3f, 3f);
            Instantiate(unit_enemy, new Vector3(randomX, 3f), Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            UnitSystem[] allUnits = Object.FindObjectsByType<UnitSystem>(FindObjectsSortMode.None);

            foreach (var unit in allUnits)
                Destroy(unit.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.T)) GameManager.instance.TurnChange();
    }
}
