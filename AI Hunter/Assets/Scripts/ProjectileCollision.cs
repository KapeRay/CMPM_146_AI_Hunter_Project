using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
   public void OnCollisionEnter(Collision collision)
   {
        if (collision.gameObject.tag == "Terrain")
        {
            Destroy(gameObject);
        }
   }
}
