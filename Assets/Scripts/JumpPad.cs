using UnityEngine;

public class JumpPad : MonoBehaviour
{
   [SerializeField] float jumpPadHight;
   ThirdPersonMovementScript ThirdPersonMovementScript;

    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonMovementScript = other.GetComponent<ThirdPersonMovementScript>();
        if (ThirdPersonMovementScript != null )
        {
            ThirdPersonMovementScript.JumpAction(jumpPadHight);
        }
    }
}
