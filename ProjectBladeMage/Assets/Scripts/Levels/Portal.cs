using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Portal to the next level
public class Portal : MonoBehaviour
{
    [SerializeField] private LevelSO levelData;
    private SpriteRenderer portalPreview;

    // Start is called before the first frame update
    void Awake()
    {
        portalPreview = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiatePortal()
    {
        portalPreview.sprite = levelData.portalPreview;
    }
}
