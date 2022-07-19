using UnityEngine;


[CreateAssetMenu(fileName = "New Projectile", menuName ="Projectile")]
public class ProjectileSO : ScriptableObject
{
    public float projectileSpeed;
    public int baseDamage;
    public int maxNumHits;      // Maximum number of units projectile can pierce through

}
