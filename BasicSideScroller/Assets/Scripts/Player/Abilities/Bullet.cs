using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool IsEnabled { get; set;}

    [SerializeField] private float speed = 100f;
    private Vector3 direction = new Vector3(1, 0, 0);

    // Start is called before the first frame update
    void Awake()
    {
        IsEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnabled)
            MoveBullet();
    }

    public void FireBullet(Vector3 dir)
    {
        direction = dir;

        //Quaternion rotation = Quaternion.LookRotation(direction);
        //transform.rotation = rotation;

    }

    // Updates position of bullet
    private void MoveBullet()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // Placeholder reset condition. Will need to be relative to screen rather than hard coded values
        if (transform.position.x >=  15 || transform.position.x <= -15 || transform.position.y >= 15 || transform.position.x <= -15)
            Reset();

    }

    private void Reset()
    {
        IsEnabled = false;
        transform.position = new Vector3(1000, 1000, 0);
    }
}
