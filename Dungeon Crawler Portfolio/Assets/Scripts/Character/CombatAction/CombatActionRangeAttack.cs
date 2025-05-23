using UnityEngine;


[CreateAssetMenu(fileName = "RangeAttack", menuName = "Combat Action/Range Attack", order = 1)]
public class CombatActionRangeAttack : CombatActionSO
{
    public ScriptableObjectTransform playerPos;
    public GameObject projectile;
    public override void CallAction()
    {
        Instantiate(projectile, playerPos.position + playerPos.rotation * Vector3.forward, playerPos.rotation);

    }

}
