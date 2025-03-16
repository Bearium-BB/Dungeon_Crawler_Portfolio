using UnityEngine;


[CreateAssetMenu(fileName = "Stats", menuName = "Character/Stats", order = 0)]
public class CharacterStats : ScriptableObject
{
    //float jumpHeight = 2;
    public float speed = 5;
    public float physicalDamage = 5;
    public float magicDamage = 5;
    public float physicalResistance = 5;
    public float magicResistance = 5;
    public float physicalResistancePercentage = 0.5f;
    public float magicResistancePercentage = 0.5f;
}
