//Code taken from CodeMonkey on YouTube

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;

    private GameObject mainCamera;
    private Transform cameraTrackerTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraTrackerTransform = GameObject.FindGameObjectWithTag("CameraTracker").transform;
        lastCameraPosition = cameraTrackerTransform.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width * transform.localScale.x / sprite.pixelsPerUnit;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTrackerTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = cameraTrackerTransform.position;

        if (Mathf.Abs(mainCamera.transform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (mainCamera.transform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(mainCamera.transform.position.x + offsetPositionX, transform.position.y);
        }
    }

    public void MoveBackground(float resetDistance)
    {
        transform.position -= transform.right * resetDistance * (1.0f - parallaxEffectMultiplier.x);
    }
}
