using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{

    public float gunDamage = 100f;
    public Camera fpsCam;
    public float weaponRange = 50f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

    }

    private void Shoot() {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, weaponRange))
        {

            Target target = hit.transform.GetComponent<Target>();

            if (target != null) {
                target.TakeDamage(gunDamage);
            }

        }

    }


}
