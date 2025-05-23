using UnityEngine;


[CreateAssetMenu(fileName = "Stats", menuName = "Entity/Stats", order = 0)]
public class EntityStats : ScriptableObject
{
    public float health = 100;
    public float speed = 5;
    public float physicalDamage = 5;
    public float magicDamage = 5;
    public float physicalResistance = 5;
    public float magicResistance = 5;
    public float physicalResistancePercentage = 0.5f;
    public float magicResistancePercentage = 0.5f;
}
