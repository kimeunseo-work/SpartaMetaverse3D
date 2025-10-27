using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform tempLobby;
    private Vector3 originPos;
    private BaseNPC currentTarget;

    private void OnEnable()
    {
        GameManager.Instance.OnMinigameEnterd += EnterMiniGame;
    }

    private void Update()
    {
        if (currentTarget != null && Input.GetKeyDown(KeyCode.F))
        {
            currentTarget.Interact();
        }
    }

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

    private void EnterMiniGame()
    {
        originPos = transform.position;
        transform.position = tempLobby.position;
    }
    private void ExitMiniGame()
    {
        transform.position = originPos;
        originPos = Vector3.zero;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnMinigameEnterd -= EnterMiniGame;
    }
}