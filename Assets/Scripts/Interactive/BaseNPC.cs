using UnityEngine;

public class BaseNPC : MonoBehaviour
{
    [SerializeField] private GameObject ui;
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

    public virtual void EnterGame()
    {

    }
    public void Interact()
    {
        ui.SetActive(true);
    }
    public void Enter()
    {
        keySprite.gameObject.SetActive(true);
    }
    public void Exit()
    {
        ui.SetActive(false);
        keySprite.gameObject.SetActive(false);
    }
}
