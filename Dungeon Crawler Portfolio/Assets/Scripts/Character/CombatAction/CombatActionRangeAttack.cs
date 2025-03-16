using UnityEngine;


[CreateAssetMenu(fileName = "RangeAttack", menuName = "Combat Action/Range Attack", order = 1)]
public class CombatActionRangeAttack : CombatActionSO
{
    public ScriptableObjectTransform playerPos;
    public GameObject projectile;
    public override void CallAction()
    {
        Instantiate(projectile, playerPos.value.position + playerPos.value.forward, playerPos.value.rotation);

    }

}
