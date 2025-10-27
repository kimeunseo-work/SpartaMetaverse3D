using Cinemachine;
using UnityEngine;

public class CameraAreaTrigger : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private void Awake()
    {
        cam.GetComponentInChildren<CinemachineVirtualCamera>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraManager.Instance.ChangeCam(cam);
        }
    }
}