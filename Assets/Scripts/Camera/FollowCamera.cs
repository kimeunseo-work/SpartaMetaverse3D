using UnityEngine;

public enum CameraState
{
    Idle,
    Lerp
}

public class FollowCamera : MonoBehaviour
{
    // target = player
    [SerializeField] private Transform target;
    
    // camera
    private Camera cam;
    private const float cameraSpeed = 50f;

    // cameraArea
    private Bounds currentBounds;
    private Bounds desiredBounds;
    private Collider2D currentCollider;
    private const float lerpSpeed = 2.5f;
    private CameraState state = CameraState.Idle;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        currentCollider = GameManager.Instance.InitCollider;
        currentBounds = currentCollider.bounds;
    }
    private void Update()
    {
        // ���� ���� ���� ȣ��
        if(state == CameraState.Lerp) LerpBounds();
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }

    /// <summary>
    /// �÷��̾� ����, �÷��̾�� ������ ������
    /// </summary>
    private void FollowTarget()
    {
        // ī�޶�� ĳ������ ��ġ �ľ�
        var cameraPos = transform.position;
        var playerPos = target.position;
        // ī�޶� ���ؾ� �ϴ� ���� ����
        var direction = (playerPos - cameraPos).normalized;
        // ���� �����ӿ� ī�޶� �̵��� ��ġ
        var NextPos = (Vector2)cameraPos + (Vector2)direction * cameraSpeed * Time.deltaTime;

        // ī�޶��� ���̿� �ʺ� �ľ�
        var camHeight = cam.orthographicSize * 2;
        var camWidth = cam.aspect * camHeight;

        // ī�޶� ���� ����
        float x = Mathf.Clamp(
                            NextPos.x,
                            currentBounds.min.x + camWidth / 2,
                            currentBounds.max.x - camWidth / 2
                        );
        float y = Mathf.Clamp(
                            NextPos.y,
                            currentBounds.min.y + camHeight / 2,
                            currentBounds.max.y - camHeight / 2
                        );

        // ī�޶� �̵�
        transform.position = new Vector3(x, y, -10);
    }

    /// <summary>
    /// ī�޶� ���� ���� ����
    /// ���� �ٲ� �� ī�޶� ���� �̵� ����
    /// </summary>
    private void LerpBounds()
    {
        currentBounds.min = Vector2.Lerp(
                                currentBounds.min,
                                desiredBounds.min,
                                lerpSpeed * Time.deltaTime
                            );
        currentBounds.max = Vector2.Lerp(
                                currentBounds.max,
                                desiredBounds.max,
                                lerpSpeed * Time.deltaTime
                            );

        // ���� ������ ���� ����
        if(currentBounds == desiredBounds)
        {
            state = CameraState.Idle;
        }
    }

    public void SetDesiredBounds(Collider2D collider)
    {
        if (collider == currentCollider) return;

        currentCollider = collider;
        desiredBounds = currentCollider.bounds;

        state = CameraState.Lerp;
    }
}