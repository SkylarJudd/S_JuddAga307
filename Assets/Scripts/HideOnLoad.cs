using UnityEngine;

public class HideOnLoad : MonoBehaviour
{
    [SerializeField] bool debug;
    private void Start()
    {
        //make game mannager that has a bool for debug and get it on start.

        if(debug == false)
        gameObject.SetActive(false);
    }
}
