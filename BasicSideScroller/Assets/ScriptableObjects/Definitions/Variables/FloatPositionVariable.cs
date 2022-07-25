using UnityEngine;

[CreateAssetMenu]
public class FloatPositionVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    public Vector3 vector3Value;

    public void SetVector3Value(Vector3 value)
    {
        vector3Value = value;
    }
}
