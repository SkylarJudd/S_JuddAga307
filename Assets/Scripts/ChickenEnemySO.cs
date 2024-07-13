using UnityEngine;
using UnityEngine.VFX;
[CreateAssetMenu(fileName ="ChickenEnemySO" , menuName ="ChickenGame/NavSO")]
public class ChickenEnemySO : ScriptableObject
{
    public GameObject visials;
    public string chickenEnemyName;
    public float chickenEnemySpeed;
    public float chickenEnemyRotateSpeed;
}
