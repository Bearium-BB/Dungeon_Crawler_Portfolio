using UnityEngine;


[CreateAssetMenu(fileName = "CharacterControls", menuName = "Data Types/Character Controls ", order = 2)]
public class ScriptableObjectCharacterControls : ScriptableObject
{
    public CharacterControls value;

    private void OnEnable()
    {
        value = new CharacterControls();
    }

    private void OnDisable()
    {
        value = null;
    }
}
