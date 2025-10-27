#if false
using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[Flags]
public enum CameraState
{
    Idle,
    BoundLerp,
    CamSizeLerp,
}

public class FollowCamera : MonoBehaviour
{
    // target = player
    [SerializeField] private Transform target;

    // camera
    private Camera cam;
    private PixelPerfectCamera ppCam;
    private const float cameraSpeed = 50f;

    // cameraArea
    private Bounds currentBounds;
    private Bounds desiredBounds;
    private Collider2D currentCollider;
    private const float lerpSpeed = 2.5f;
    private CameraState state = CameraState.Idle;

    // cameraSize
    private Vector2Int desiredCamSize;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        ppCam = cam.GetComponent<PixelPerfectCamera>();
    }
    private void Start()
    {
        //currentCollider = GameManager.Instance.InitCollider;
        currentBounds = currentCollider.bounds;
    }
    private void Update()
    {
        // 보간 중일 때만 호출
        if (state.HasFlag(CameraState.BoundLerp)) LerpBounds();
        if (state.HasFlag(CameraState.CamSizeLerp)) LerpCamSize();
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
        // cameraArea
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
        if (currentBounds == desiredBounds)
        {
            state &= ~CameraState.BoundLerp;
            if (!state.HasFlag(CameraState.CamSizeLerp))
            {
                state = CameraState.Idle;
            }
        }
    }

    /// <summary>
    /// 카메라 크기 보간
    /// </summary>
    private void LerpCamSize()
    {
        cam.orthographicSize = Mathf.MoveTowards(
                                        ppCam.refResolutionY,
                                        desiredCamSize.y,
                                        120f * Time.deltaTime) / (ppCam.assetsPPU * 2f);

        // cameraSize
        ppCam.refResolutionX = Mathf.RoundToInt(
                                        Mathf.MoveTowards(
                                                ppCam.refResolutionX,
                                                desiredCamSize.x,
                                                120f * Time.deltaTime
                                            ));
        ppCam.refResolutionY = Mathf.RoundToInt(
                                        Mathf.MoveTowards(
                                                ppCam.refResolutionY,
                                                desiredCamSize.y,
                                                120f * Time.deltaTime
                                            ));

        // 보간 끝나면 상태 원복
        var checkLerp = new Vector2Int(ppCam.refResolutionX, ppCam.refResolutionY);
        if (checkLerp == desiredCamSize)
        {
            ppCam.enabled = true;
            state &= ~CameraState.CamSizeLerp;

            if (!state.HasFlag(CameraState.BoundLerp))
            {
                state = CameraState.Idle;
            }
        }
    }

    public void SetDesiredBounds(Collider2D collider, Vector2Int refResolution)
    {
        if (collider == currentCollider) return;

        currentCollider = collider;
        desiredBounds = currentCollider.bounds;

        ppCam.enabled = false;
        desiredCamSize = refResolution;

        state &= ~CameraState.Idle;
        state |= CameraState.BoundLerp | CameraState.CamSizeLerp;
    }
}
#endif