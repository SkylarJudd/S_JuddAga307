using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{

    public string[] triggerTags;

    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;
    public UnityEvent OnTriggerStayEvent;

    private void OnTriggerEnter(Collider other) => Trigger(other, OnTriggerEnterEvent);
    private void OnTriggerExit(Collider other) => Trigger(other, OnTriggerExitEvent);
    private void OnTriggerStay(Collider other) => Trigger(other, OnTriggerStayEvent);

    private void Trigger(Collider _other, UnityEvent _Event)
    {
        for (int i = 0; i < triggerTags.Length; i++)
        {
            if (ObjectX.DoesTagExist(triggerTags[i]))

                if (_other.CompareTag(triggerTags[i]))
                {
                    _Event.Invoke();
                }
        }
    }
}
