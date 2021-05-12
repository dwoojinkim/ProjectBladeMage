using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private Vector3 lastPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        lastPlayerPosition = playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaMovement = playerTransform.position.x - lastPlayerPosition.x;
        transform.position += transform.right * deltaMovement;
        lastPlayerPosition = playerTransform.position;
    }
}
