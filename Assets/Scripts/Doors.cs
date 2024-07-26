using UnityEngine;

public class Doors : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    public Animator animator;
    public string doorOpenName = "DoorOpen";
    public string doorCloseName = "DoorClose";

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("Player In Door Trigger");
           
            animator.SetTrigger(doorOpenName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            print("Player In Door Trigger");
            
            animator.SetTrigger(doorCloseName);
        }
    }

}
