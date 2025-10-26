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
    /// 사용자 입력에 따라 필요한 데이터 업데이트
    /// </summary>
    protected override void HandleAction()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontal, vertical).normalized;

        // 이동 여부에 따라 상태 전환
        currentState = moveDirection == Vector2.zero 
            ? CharacterState.Idle
            : CharacterState.Run;

        // 마우스 - 캐릭터 방향 처리
        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPos = cam.ScreenToWorldPoint(mousePosition);
        lookDirection = (worldPos - (Vector2)transform.position);

        // 미세한 값은 방향 X 처리
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