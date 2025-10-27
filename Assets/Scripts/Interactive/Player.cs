using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// �̴� ���� ���Խ� �÷��̾� ��ġ �̵���ų �ӽ� ��ġ
    /// </summary>
    [SerializeField] private Transform tempLobby;
    private Vector3 originPos;
    /// <summary>
    /// ���� ��ȣ�ۿ� ���� NPC
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
    /// �̴ϰ��� ���Խ� �÷��̾� ��ġ �̵�
    /// </summary>
    private void EnterMiniGame()
    {
        originPos = transform.position;
        transform.position = tempLobby.position;
    }
    /// <summary>
    /// �̴ϰ��� ����� �÷��̾� ��ġ ����
    /// </summary>
    private void ExitMiniGame()
    {
        transform.position = originPos;
        originPos = Vector3.zero;
    }
}