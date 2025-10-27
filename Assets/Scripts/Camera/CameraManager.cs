using Cinemachine;
using UnityEditor.U2D.Aseprite;
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

    private CinemachineVirtualCamera currentCam;

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

        currentCam = InitAreaCam;
        currentCam.Priority = onLive;
    }

    public void ChangeCam(CinemachineVirtualCamera cam, bool orthographic = true, Rect rect = default, bool is3D = false)
    {
        if (currentCam == cam) return;

        currentCam.Priority = noLive;
        currentCam = cam;
        currentCam.Priority = onLive;

        Camera.main.orthographic = orthographic;
        if (rect != default) Camera.main.rect = rect;
        if (is3D)
        {
            camData.SetRenderer(1);
        }
        else
        {
            camData.SetRenderer(0);
        }
    }
}