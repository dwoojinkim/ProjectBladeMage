using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelPortals : MonoBehaviour
{
    [SerializeField] private Portal portal1;
    [SerializeField] private Portal portal2;
 
    // Start is called before the first frame update
    void Start()
    {
        portal1.InstantiatePortal();
        portal2.InstantiatePortal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
