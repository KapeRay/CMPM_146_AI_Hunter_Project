using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerHealing : MonoBehaviour
{
    // Start is called before the first frame update
    Ray ray;
    RaycastHit hit;
    bool cursorIsCurrentlyOn = false;
    Collider currentTarget;
    Color originalColor;
    public int healthAmount = 10;
    bool healing = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.GetComponent<PlayerControl>().enabled)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                var cubeRenderer = hit.collider.GetComponent<Renderer>();
                
                
                if (hit.collider.tag == "DPS" || hit.collider.tag == "Healer" || hit.collider.tag == "Tank")
                {
                    
                    currentTarget = hit.collider;
                    originalColor = currentTarget.GetComponent<CharacterHealth>().original;
                    cubeRenderer.material.SetColor("_Color", Color.green);
                    if (Input.GetKeyDown("h") && !currentTarget.GetComponent<CharacterHealth>().characterIsDead)
                    {
                        healthAmount = 10;
                        hit.collider.GetComponent<CharacterHealth>().healingTheCharacter(10);
                    }
                    if (currentTarget.GetComponent<CharacterHealth>().characterIsDead && Input.GetKeyDown("h"))
                    {
                        healthAmount *= 2;
                        currentTarget.GetComponent<CharacterHealth>().healingTheCharacter(healthAmount);
                    }
                }
                else
                {
                    if (!currentTarget.GetComponent<CharacterHealth>().characterIsDead)
                    {
                        currentTarget.GetComponent<Renderer>().material.SetColor("_Color", originalColor);
                    }
                    else
                    {
                        currentTarget.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
                    }
                }
                
            }
        }
        else if(!gameObject.GetComponent<PlayerControl>().enabled && currentTarget != null)
        {
            currentTarget.GetComponent<Renderer>().material.SetColor("_Color", originalColor);
        }
        //Debug.Log("Current Target = " + currentTarget);
    }
}
