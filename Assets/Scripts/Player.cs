using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController_Idle controller_Idle;
    /// <summary>
    /// �̴� ���� ���Խ� �÷��̾� ��ġ �̵���ų �ӽ� ��ġ
    /// </summary>
    [SerializeField] private Transform tempLobby;
    private Vector3 originPos;
    /// <summary>
    /// ���� ��ȣ�ۿ� ���� NPC
    /// </summary>
    private BaseNPC currentTarget;

    private void Awake()
    {
        controller_Idle = GetComponent<PlayerController_Idle>();
    }
    private void Start()
    {
        controller_Idle.enabled = true;
    }

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
    /// �̴ϰ��� ���Խ� �÷��̾� ��ġ �̵�
    /// </summary>
    private void EnterMiniGame()
    {
        // controller
        controller_Idle.enabled = false;

        // position
        originPos = transform.position;
        transform.position = tempLobby.position;
    }
    /// <summary>
    /// �̴ϰ��� ����� �÷��̾� ��ġ ����
    /// </summary>
    private void ExitMiniGame()
    {
        // controller
        controller_Idle.enabled = true;

        // position
        transform.position = originPos;
        originPos = Vector3.zero;
    }
}