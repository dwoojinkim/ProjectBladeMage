using UnityEngine;

[CreateAssetMenu(fileName = "New Playable Character", menuName = "Player/Character")]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    // basic weapon scriptable object
    // ability 1 scriptable object
    // ability 2 scriptable object
    // ultimate scriptable object
    public float movementSpeed;
    public int jumpPower;
    public int fallSpeed;
}
