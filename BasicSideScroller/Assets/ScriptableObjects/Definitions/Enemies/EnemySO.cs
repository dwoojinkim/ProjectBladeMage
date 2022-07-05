using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName ="Enemies/Enemy")]
public class EnemySO : ScriptableObject
{
    public new string name;
    public int maxHealth;
    public int damage;
    public Sprite gameSprite;
}
