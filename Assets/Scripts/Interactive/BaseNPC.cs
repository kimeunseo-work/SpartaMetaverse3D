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
    /// ��ȣ�ۿ� Ű ������ ��
    /// </summary>
    public virtual void Interact()
    {
        ui.SetActive(true);
    }
    /// <summary>
    /// ��ȣ�ۿ� ���� ������ ��
    /// </summary>
    public void Enter()
    {
        keySprite.gameObject.SetActive(true);
    }
    /// <summary>
    /// ��ȣ�ۿ� ������ ���� ��
    /// </summary>
    public void Exit()
    {
        ui.SetActive(false);
        keySprite.gameObject.SetActive(false);
    }
    /// <summary>
    /// ���� ���� ��ư ������ ��
    /// </summary>
    protected virtual void EnterGame()
    {
        ui.SetActive(false);
    }
    /// <summary>
    /// ���� ������ ������ ��
    /// </summary>
    protected virtual void ExitGame()
    {
        ui.SetActive(true);
    }
}
