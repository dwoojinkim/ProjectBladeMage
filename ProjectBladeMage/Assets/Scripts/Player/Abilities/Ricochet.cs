using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet: MonoBehaviour
{
    public GameObject WeaponPrefab;
    public FloatVariable WeaponManaCost;
    private Weapon weaponType;
    private string weaponTypeString;
    private GameObject weapon;
    private List<Weapon> activeWeapons;
    private Weapon weaponScript;
    private int initWeaponPoolSize = 2;
    private int manaCost = 25;

    private Vector3 mousePosition;

    void Awake()
    {
        WeaponManaCost.SetValue(manaCost);
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponType = WeaponPrefab.GetComponent<Weapon>();
        weaponTypeString = weaponType.GetType().Name;

        ObjectPooler.AddPool(this.gameObject, weaponTypeString, WeaponPrefab, initWeaponPoolSize);
    }

    // Update is called once per frame
    void Update()
    {
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
    }

    // public void ThrowWeapon()
    // {
    //     Vector3 throwDirection = mousePosition - transform.position;
    //     throwDirection.Normalize();

    //     weapon.transform.position = transform.position;
    //     bladerang.ThrowBladerang(throwDirection);
    //     weapon.SetActive(true);
    // }

    public void ThrowWeapon(int direction)
    {
        weapon = ObjectPooler.SpawnFromPool(this.gameObject, weaponTypeString, transform.position, Quaternion.identity);
        weaponScript = weapon.GetComponent<Weapon>();
        //weapon.transform.position = transform.position;
        weaponScript.ThrowWeapon(direction);
        //weapon.SetActive(true);
    }
    
    public void ReturnWeapon(GameObject obj)
    {
        ObjectPooler.ReturnToPool(this.gameObject, weaponTypeString, obj);
    }

    public int GetManaCost()
    {
        return manaCost;
    }
}
