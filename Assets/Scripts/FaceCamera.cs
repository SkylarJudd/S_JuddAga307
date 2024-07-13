using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Transform mainCamera;
    Transform localTransform;
    void Start()
    {
        mainCamera = FindAnyObjectByType<Camera>().transform;
        localTransform = GetComponent<Transform>();
    }


    void Update()
    {
        if (mainCamera != null)
        {
            localTransform.LookAt(2* localTransform.position - mainCamera.position);
        }
    }
}
