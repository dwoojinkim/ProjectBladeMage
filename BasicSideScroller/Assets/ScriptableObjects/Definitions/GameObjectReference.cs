using UnityEngine;

[CreateAssetMenu(fileName="New GameObject Reference",menuName="GameObject Reference")]
public class GameObjectReference : ScriptableObject
{
    public GameObject gameObject;

    public void SetGameObject(GameObject value)
    {
        gameObject = value;
    }

    public Transform GetTransform()
    {
        return gameObject.transform;
    }
}
