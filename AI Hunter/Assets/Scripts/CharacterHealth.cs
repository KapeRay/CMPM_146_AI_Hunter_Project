using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    // Start is called before the first frame update

    public int maxHealth = 100;
    public int playerHealth = 100;
    public Color original;
    private bool isItHit = false;
    void Start()
    {
        original = gameObject.GetComponent<Renderer>().material.color;
        //Debug.Log(gameObject + "has color " + original);
    }

    // Update is called once per frame
    void Update()
    {

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
        Debug.Log("hello");
        if (other.gameObject.tag == "Enemy" && !isItHit)
        {
            
            StartCoroutine(iframes());
        }
    }
    private IEnumerator iframes()
    {
        if (playerHealth > 0)
        {
            isItHit = true;
            playerHealth -= 10;
            // process pre-yield
            Debug.Log(playerHealth);
            yield return new WaitForSeconds(5.0f);
            isItHit = false;
        }
    }
}
