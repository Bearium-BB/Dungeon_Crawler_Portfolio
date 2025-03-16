using UnityEngine;

public abstract class CombatActionSO : ScriptableObject, ICombatAction
{
    public abstract void CallAction();
}