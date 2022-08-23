using System.Collections.Generic;
using UnityEngine;

// Set of possible stages to choose from for a particular level
[CreateAssetMenu(fileName = "New Level Set", menuName = "Sets/LevelSet")]
public class LevelSet : ScriptableObject
{
    public List<LevelSO> CombatLevels;

    public List<LevelSO> TrapLevels;
}
