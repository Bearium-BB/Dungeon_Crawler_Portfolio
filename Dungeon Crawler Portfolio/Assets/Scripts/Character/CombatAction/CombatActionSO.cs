using UnityEngine;

//Use an abstract class here so it works inside the unity inspector instead of directly just using the interface by itself
public abstract class CombatActionSO : ScriptableObject, ICombatAction
{
    public abstract void CallAction();
}