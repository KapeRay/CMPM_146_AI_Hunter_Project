using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject player;
    public GameObject target;
    public float speed;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject projectile = Instantiate(projectilePrefab, player.transform.position, projectilePrefab.transform.rotation);
            projectile.transform.LookAt(target.transform);
            SendHoming(projectile);
        }
    }

    public void SendHoming(GameObject projectile)
    {
        while (Vector3.Distance(target.transform.position, projectile.transform.position) >= 1f)
        {
            projectile.transform.position += (target.transform.position - projectile.transform.position).normalized * speed * Time.deltaTime;
            projectile.transform.LookAt(target.transform);
        }
        player.GetComponent<CharacterHealth>().aggro += 1.0f;
        if(!(target.GetComponent<BossAI>().PLAYERHITSIGNAL)){
            target.GetComponent<BossAI>().PLAYERHITSIGNAL = true;
        }
        Destroy(projectile);
    }
}
