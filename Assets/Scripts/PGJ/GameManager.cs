using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] AIManager aiManager;

    static public GameManager instance;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    bool isPlayerTurn = true;

    void Start()
    {
        TurnInit();
    }

    void TurnInit()
    {
        isPlayerTurn = true;
    }

    /// <summary>
    /// 턴 바꾸기
    /// </summary>
    internal void TurnChange()
    {
        isPlayerTurn = !isPlayerTurn;

        // AI 턴
        if (false == isPlayerTurn)
        {
            aiManager.TurnStartAI();
        }
        else        // 플레이어 턴
        {
        
        }
    }

    /// <summary>
    /// 플레이어 턴인지 체크
    /// </summary>
    internal bool PlayerTurnCheck()
    {
        return isPlayerTurn;
    }
}
