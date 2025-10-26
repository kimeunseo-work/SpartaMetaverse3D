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
        // 보간 중일 때만 호출
        if(state == CameraState.Lerp) LerpBounds();
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }

    /// <summary>
    /// 플레이어 추적, 플레이어보다 느리게 움직임
    /// </summary>
    private void FollowTarget()
    {
        // 카메라와 캐릭터의 위치 파악
        var cameraPos = transform.position;
        var playerPos = target.position;
        // 카메라가 향해야 하는 방향 도출
        var direction = (playerPos - cameraPos).normalized;
        // 다음 프레임에 카메라가 이동할 위치
        var NextPos = (Vector2)cameraPos + (Vector2)direction * cameraSpeed * Time.deltaTime;

        // 카메라의 높이와 너비 파악
        var camHeight = cam.orthographicSize * 2;
        var camWidth = cam.aspect * camHeight;

        // 카메라 영역 제한
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

        // 카메라 이동
        transform.position = new Vector3(x, y, -10);
    }

    /// <summary>
    /// 카메라 제한 영역 보간
    /// 영역 바뀔 때 카메라 순간 이동 방지
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

        // 보간 끝나면 상태 원복
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