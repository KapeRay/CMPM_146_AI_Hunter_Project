using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    // Start is called before the first frame update

    public int maxHealth = 100;
    public int playerHealth = 100;
    public float aggro = 0.0f;
    public float timeAlive = 0.0f;
    public float aggroTime = 0.0f;
    public Color original;
    private bool isItHit = false;
    private bool enemyIsAttacking = false;
    public bool characterIsDead = false;
    void Start()
    {
        original = gameObject.GetComponent<Renderer>().material.color;
        //Debug.Log(gameObject + "has color " + original);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth <= 0)
        {
            characterIsDead = true;
            timeAlive = 0.0f;
            aggroTime = 0.0f;
            gameObject.GetComponent<Renderer>().material.color = Color.grey;
        }
        else
        {
            timeAlive += Time.deltaTime;
            if(gameObject.tag == "Tank")
                aggro += 2 * Time.deltaTime;
            aggroTime = aggro / timeAlive;
            characterIsDead = false;
        }
    }
    public void healingTheCharacter(int heal)
    {
        playerHealth = Mathf.Clamp(playerHealth, 0, maxHealth);
        if (playerHealth < maxHealth)
        {
            playerHealth += heal;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !isItHit && !characterIsDead)
        {
            other.GetComponent<EnemyHunting>().enemySpeed = 0;

            StartCoroutine(iframes());
        }
        else if (characterIsDead)
        {
            other.GetComponent<EnemyHunting>().enemySpeed = other.GetComponent<EnemyHunting>().oldSpeed;
        }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("hello");
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyHunting>().enemySpeed = other.GetComponent<EnemyHunting>().oldSpeed;

        }
            

    }
    private IEnumerator iframes()
    {
        if (playerHealth > 0)
        {
            isItHit = true;
            playerHealth -= 10;
            // process pre-yield
            //Debug.Log(playerHealth);
            yield return new WaitForSeconds(5.0f);
            isItHit = false;
        }
    }
}
