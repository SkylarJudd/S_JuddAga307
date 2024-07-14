using UnityEngine;

public class ValidNavPoints : MonoBehaviour
{
    public ValidNavPoints[] validNavTransforms;
    public GameObject navPointTransform;

    private void Start()
    {
        navPointTransform = gameObject;
    }
}
