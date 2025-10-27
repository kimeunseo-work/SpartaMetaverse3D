using UnityEngine;

public class PlayerController : BaseController
{
    private Camera cam;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        cam = Camera.main;
    }

    protected override void Update()
    {
        base.Update();
    }
    
    /// <summary>
    /// ����� �Է¿� ���� �ʿ��� ������ ������Ʈ
    /// </summary>
    protected override void HandleAction()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontal, vertical).normalized;

        // �̵� ���ο� ���� ���� ��ȯ
        currentState = moveDirection == Vector2.zero 
            ? CharacterState.Idle
            : CharacterState.Run;

        // ���콺 - ĳ���� ���� ó��
        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPos = cam.ScreenToWorldPoint(mousePosition);
        lookDirection = (worldPos - (Vector2)transform.position);

        // �̼��� ���� ���� X ó��
        if (lookDirection.magnitude < .9f)
        {
            lookDirection = Vector2.zero;
        }
        else
        {
            lookDirection = lookDirection.normalized;
        }
    }
}