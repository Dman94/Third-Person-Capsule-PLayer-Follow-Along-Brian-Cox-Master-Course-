using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{

    Rigidbody rb;
    float BulletSpeed = 800f;
    float BulletLifeTime = 1f;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
  

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Target")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
      else  if (other.tag == "Enemy")
        {
            EnemyLogic enemyLogic = other.GetComponent<EnemyLogic>();
            if (enemyLogic)
            {
                enemyLogic.TakeDamage(35);
            }
            Destroy(gameObject);

        }
    }
    // Update is called once per frame
    void Update()
    {
        BulletShot();
        BulletLifeSpan();
    }

    private void BulletShot()
    {
        if (rb)
        {
            rb.velocity = transform.up * BulletSpeed * Time.deltaTime;
        }
    }

    void BulletLifeSpan()
    {
        BulletLifeTime -= Time.deltaTime;

        if (BulletLifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

}
