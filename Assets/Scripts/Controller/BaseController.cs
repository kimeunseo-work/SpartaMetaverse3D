using UnityEngine;

public enum CharacterState
{
    Idle,
    Run
}

public class BaseController : MonoBehaviour
{
    /// <summary>
    /// �÷��̾� �̵� �ӵ�
    /// </summary>
    private float moveSpeed = 5.0f;
    /// <summary>
    /// �÷��̾� rigidbody
    /// </summary>
    protected Rigidbody2D _rigidBody;
    /// <summary>
    /// �÷��̾� ȸ�� ���� ��������Ʈ �迭
    /// </summary>
    protected SpriteRenderer[] spriteRenderers;
    /// <summary>
    /// �÷��̾� �̵� ����
    /// </summary>
    protected Vector2 moveDirection = Vector2.zero;
    /// <summary>
    /// ���콺 ����
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
    /// ĳ���� �̵� ó��
    /// </summary>
    private void Movement(Vector2 direction)
    {
        _rigidBody.velocity = direction * moveSpeed;
    }

    /// <summary>
    /// ĳ���� ȸ�� ó��
    /// </summary>
    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // ���콺 ���⿡ ���� ��������Ʈ ���� ��ȯ
        bool isLeft = Mathf.Abs(rotZ) > 90f;
        foreach (var sprite in spriteRenderers)
        {
            sprite.flipX = isLeft;
        }
    }

    /// <summary>
    /// �÷��̾� �ִϸ��̼� ��ȯ
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