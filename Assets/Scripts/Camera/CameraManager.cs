using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    /*Singleton*/
    public static CameraManager Instance;

    /*MainCam*/
    private UniversalAdditionalCameraData camData;

    /*virtualCamera*/
    [SerializeField] private CinemachineVirtualCamera InitAreaCam;
    
    /*priority*/
    private const int onLive = 10;
    private const int noLive = 0;

    /*Camera*/
    private CinemachineVirtualCamera currentCam;

    /*Origin*/
    private CinemachineVirtualCamera originCam;
    private Rect originRect = new(0, 0, 1, 1);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        camData = Camera.main.GetComponent<UniversalAdditionalCameraData>();

        // InitSet
        currentCam = InitAreaCam;
        currentCam.Priority = onLive;
    }

    /// <summary>
    /// 일반적인 카메라 변경 메서드
    /// </summary>
    public void ChangeCam(CinemachineVirtualCamera cam)
    {
        if (currentCam == cam) return;

        currentCam.Priority = noLive;
        currentCam = cam;
        currentCam.Priority = onLive;
    }

    /// <summary>
    /// 미니 게임용으로 사용
    /// </summary>
    /// <param name="isExit">미니게임 진입이면 true로 설정</param>
    /// <param name="cam">진입일 경우에만 카메라 할당</param>
    /// <param name="orthographic">true = orthographic, false = persfective</param>
    /// <param name="rect">viewportRect</param>
    /// <param name="is3D">renderer</param>
    public void ChangeCam(bool isExit = true, CinemachineVirtualCamera cam = null, bool orthographic = true, bool is3D = false, Rect rect = default)
    {
        if (isExit)
        {
            currentCam.Priority = noLive;
            currentCam = originCam;
            originCam = null;
            currentCam.Priority = onLive;

            Camera.main.orthographic = orthographic;
            Camera.main.rect = originRect;
            camData.SetRenderer(0);

            return;
        }

        if (currentCam == cam) return;
        currentCam.Priority = noLive;
        originCam = currentCam;
        currentCam = cam;
        currentCam.Priority = onLive;

        // camera - projection
        Camera.main.orthographic = orthographic;
        // camera - rendering - renderer
        if (is3D) camData.SetRenderer(1);
        else camData.SetRenderer(0);
        // camera - output - viewportRect
        if (rect != default) Camera.main.rect = rect;
    }
}