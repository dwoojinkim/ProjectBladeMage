using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName ="Enemies/Enemy")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public int maxHealth;
    public int damage;
    public RuntimeAnimatorController animController;


}
