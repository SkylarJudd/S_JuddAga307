using UnityEngine;

[CreateAssetMenu(fileName = "ValidTransforms", menuName = "ChickenGame/NavSO" )]
public class NavScriptibleObject : ScriptableObject
{
    public Transform[] validNavTransforms;
}
