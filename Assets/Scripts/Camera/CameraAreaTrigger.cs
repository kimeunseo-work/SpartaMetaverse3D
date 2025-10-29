using Cinemachine;
using UnityEngine;

public class CameraAreaTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraManager.Instance.ChangeCam(cam);
        }
    }
}