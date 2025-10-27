using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 미니 게임 진입시 플레이어 위치 이동시킬 임시 위치
    /// </summary>
    [SerializeField] private Transform tempLobby;
    private Vector3 originPos;
    /// <summary>
    /// 현재 상호작용 중인 NPC
    /// </summary>
    private BaseNPC currentTarget;

    private void OnEnable()
    {
        GameManager.Instance.OnMinigameEnterd += EnterMiniGame;
        GameManager.Instance.OnMinigameExited += ExitMiniGame;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnMinigameEnterd -= EnterMiniGame;
        GameManager.Instance.OnMinigameExited -= ExitMiniGame;
    }

    private void Update()
    {
        if (currentTarget != null && Input.GetKeyDown(KeyCode.F))
        {
            currentTarget.Interact();
        }
    }

    /*Trigger*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractiveNPC"))
        {
            currentTarget = collision.GetComponent<BaseNPC>();
            currentTarget.Enter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentTarget != null
            && currentTarget == collision.GetComponent<BaseNPC>())
        {
            currentTarget.Exit();
            currentTarget = null;
        }
    }

    /*Events*/
    /// <summary>
    /// 미니게임 진입시 플레이어 위치 이동
    /// </summary>
    private void EnterMiniGame()
    {
        originPos = transform.position;
        transform.position = tempLobby.position;
    }
    /// <summary>
    /// 미니게임 종료시 플레이어 위치 원복
    /// </summary>
    private void ExitMiniGame()
    {
        transform.position = originPos;
        originPos = Vector3.zero;
    }
}