using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Combat Action/Melee Attack", order = 0)]
public class CombatActionMeleeAttack : CombatActionSO
{
    public ScriptableObjectTransform playerPos;
    public Vector3 sizeBox;
    public float multiplayerForward;

    public override void CallAction()
    {
        Collider[] hitColliders = Physics.OverlapBox(playerPos.value.position + playerPos.value.forward * multiplayerForward, sizeBox / 2, playerPos.value.rotation);

        foreach (Collider collider in hitColliders)
        {
            Debug.Log(collider.name);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.cyan;
    //    Matrix4x4 rotationMatrix = Matrix4x4.TRS(playerPos.value.position + playerPos.value.forward * multiplayerForward, playerPos.value.rotation, Vector3.one);
    //    Gizmos.matrix = rotationMatrix;
    //    Gizmos.DrawWireCube(Vector3.zero, sizeBox);
    //}
}
