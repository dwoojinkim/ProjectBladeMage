using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : MonoBehaviour
{
    public int ManaCost {get; private set;}

    public GameObject WeaponPrefab;
    private GameObject weapon;
    private Bladerang bladerang;

    private Vector3 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        weapon = Instantiate(WeaponPrefab, new Vector3(1000, 1000, 0), Quaternion.identity);
        bladerang = weapon.GetComponent<Bladerang>();

        weapon.SetActive(false);

        ManaCost = 25;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
    }

    public void ThrowWeapon()
    {
        Vector3 throwDirection = mousePosition - transform.position;
        throwDirection.Normalize();

        weapon.transform.position = transform.position;
        bladerang.ThrowBladerang(throwDirection);
        weapon.SetActive(true);
    }

    public void ThrowWeapon(int direction)
    {
        weapon.transform.position = transform.position;
        bladerang.ThrowBladerang(direction);
        weapon.SetActive(true);
    }
    
}
