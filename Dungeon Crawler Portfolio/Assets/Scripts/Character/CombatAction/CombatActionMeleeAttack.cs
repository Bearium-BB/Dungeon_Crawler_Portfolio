using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Combat Action/Melee Attack", order = 0)]
public class CombatActionMeleeAttack : CombatActionSO
{
    //This object is a scriptable object that stores players position
    public ScriptableObjectTransform playerPos;
    //This is the boxes dimensions
    public Vector3 sizeBox;
    public float multiplayerForward;

    //This generates a collision box for a brief moment and checks what is collided with it and then prints the name
    public override void CallAction()
    {
        Collider[] hitColliders = Physics.OverlapBox(playerPos.position + playerPos.rotation * Vector3.forward * multiplayerForward, sizeBox / 2, playerPos.rotation);

        foreach (Collider collider in hitColliders)
        {
            Debug.Log(collider.name);
        }
    }
}
