using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrateLogic : MonoBehaviour
{
 
   
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GunLogic gun =  other.GetComponentInChildren<GunLogic>();

            if (gun)
            {
                gun.RefillAmmo();
                gun.PlaySound(gun.PistolReload);
                Destroy(gameObject);
            }
            
        }
    }
}
