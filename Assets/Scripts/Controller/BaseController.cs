using UnityEngine;

public enum CharacterState
{
    Idle,
    Run
}

public class BaseController : MonoBehaviour
{
    /// <summary>
    /// 플레이어 이동 속도
    /// </summary>
    private float moveSpeed = 5.0f;
    /// <summary>
    /// 플레이어 rigidbody
    /// </summary>
    protected Rigidbody2D _rigidBody;
    /// <summary>
    /// 플레이어 회전 위한 스프라이트 배열
    /// </summary>
    protected SpriteRenderer[] spriteRenderers;
    /// <summary>
    /// 플레이어 이동 방향
    /// </summary>
    protected Vector2 moveDirection = Vector2.zero;
    /// <summary>
    /// 마우스 방향
    /// </summary>
    protected Vector2 lookDirection = Vector2.zero;
    protected CharacterState currentState = CharacterState.Idle;
    protected Animator[] animators;

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        animators = GetComponentsInChildren<Animator>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
        Rotate(lookDirection);
        ChangeAnimation();
    }

    protected virtual void FixedUpdate()
    {
        Movement(moveDirection);
    }

    protected virtual void HandleAction()
    {

    }

    /// <summary>
    /// 캐릭터 이동 처리
    /// </summary>
    private void Movement(Vector2 direction)
    {
        _rigidBody.velocity = direction * moveSpeed;
    }

    /// <summary>
    /// 캐릭터 회전 처리
    /// </summary>
    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // 마우스 방향에 따른 스프라이트 방향 전환
        bool isLeft = Mathf.Abs(rotZ) > 90f;
        foreach (var sprite in spriteRenderers)
        {
            sprite.flipX = isLeft;
        }
    }

    /// <summary>
    /// 플레이어 애니메이션 전환
    /// </summary>
    private void ChangeAnimation()
    {
        if(currentState == CharacterState.Idle)
        {
            foreach (var anim in animators)
            {
                anim.SetBool("isRun", false);
            }
        }
        else if (currentState == CharacterState.Run)
        {
            foreach (var anim in animators)
            {
                anim.SetBool("isRun", true);
            }
        }
    }
}