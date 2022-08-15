using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : MonoBehaviour
{
    public GameObject WeaponPrefab;
    private GameObject weapon;
    private Bladerang bladerang;

    // Start is called before the first frame update
    void Start()
    {
        weapon = Instantiate(WeaponPrefab, new Vector3(1000, 1000, 0), Quaternion.identity);
        bladerang = weapon.GetComponent<Bladerang>();

        weapon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ThrowWeapon()
    {
        weapon.transform.position = transform.position;
        bladerang.ThrowBladerang();
        weapon.SetActive(true);
    }
    
}
