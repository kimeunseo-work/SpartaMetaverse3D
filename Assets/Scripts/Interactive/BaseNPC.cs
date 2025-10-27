using UnityEngine;

public class BaseNPC : MonoBehaviour
{
    [SerializeField] protected GameObject ui;
    [SerializeField] private Transform lobby;
    private SpriteRenderer keySprite;

    protected virtual void Awake()
    {
        keySprite = GetComponentInChildren<SpriteRenderer>();
    }
    protected virtual void Start()
    {
        ui.SetActive(false);
        keySprite.gameObject.SetActive(false);
    }
    /// <summary>
    /// 상호작용 키 눌렀을 때
    /// </summary>
    public virtual void Interact()
    {
        ui.SetActive(true);
    }
    /// <summary>
    /// 상호작용 존에 들어왔을 때
    /// </summary>
    public void Enter()
    {
        keySprite.gameObject.SetActive(true);
    }
    /// <summary>
    /// 상호작용 존에서 나갈 때
    /// </summary>
    public void Exit()
    {
        ui.SetActive(false);
        keySprite.gameObject.SetActive(false);
    }
    /// <summary>
    /// 게임 시작 버튼 눌렀을 때
    /// </summary>
    protected virtual void EnterGame()
    {
        ui.SetActive(false);
    }
    /// <summary>
    /// 게임 끝나고 나왔을 때
    /// </summary>
    protected virtual void ExitGame()
    {
        ui.SetActive(true);
    }
}
