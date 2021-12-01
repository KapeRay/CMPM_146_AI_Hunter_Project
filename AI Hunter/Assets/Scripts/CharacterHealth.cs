using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int maxHealth = 100;
    public int playerHealth = 100;
    public Color original;
    void Start()
    {
        original = gameObject.GetComponent<Renderer>().material.color;
        Debug.Log(gameObject + "has color " + original);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void healingTheCharacter(int heal)
    {
        playerHealth = Mathf.Clamp(playerHealth, 0, maxHealth);
        if(playerHealth < maxHealth)
        {
            playerHealth += heal;
        }
    }
}
