using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject player;
    public GameObject target;
    public float speed;
    public KeyCode shoot = KeyCode.Space;
    public KeyCode powerShot = KeyCode.K;

    private void Update()
    {
        if(Input.GetKeyDown(shoot))
        {
            GameObject projectile = Instantiate(projectilePrefab, player.transform.position, projectilePrefab.transform.rotation);
            projectile.transform.LookAt(target.transform);
            StartCoroutine(SendHoming(projectile));
            Debug.Log("Shoot");
        }
        if (Input.GetKey(powerShot))
        {
            GameObject projectile = Instantiate(projectilePrefab, player.transform.position, projectilePrefab.transform.rotation);
            projectile.transform.LookAt(target.transform);
            StartCoroutine(SendHoming(projectile));
        }
    }

    public IEnumerator SendHoming(GameObject projectile)
    {
        while (Vector3.Distance(target.transform.position, projectile.transform.position) >= 1f)
        {
            projectile.transform.position += (target.transform.position - projectile.transform.position).normalized * speed * Time.deltaTime;
            projectile.transform.LookAt(target.transform);
            yield return null;
        }
        player.GetComponent<CharacterHealth>().aggro += 1.0f;
        if(!(target.GetComponent<BossAI>().PLAYERHITSIGNAL)){
            target.GetComponent<BossAI>().PLAYERHITSIGNAL = true;
        }
        Destroy(projectile);
    }
}
