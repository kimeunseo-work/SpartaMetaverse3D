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
        // ���� ���� ���� ȣ��
        if (state.HasFlag(CameraState.BoundLerp)) LerpBounds();
        if (state.HasFlag(CameraState.CamSizeLerp)) LerpCamSize();
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

        // ���� ������ ���� ����
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
    /// ī�޶� ũ�� ����
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

        // ���� ������ ���� ����
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