using UnityEngine;

public class CameraAreaTrigger : MonoBehaviour
{
    private BoxCollider2D box;
    private FollowCamera mainCam;

    private void Awake()
    {
        box = GetComponentInChildren<BoxCollider2D>();
        mainCam = Camera.main.GetComponent<FollowCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mainCam.SetDesiredBounds(box);
        }
    }
}