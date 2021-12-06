using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatingAggro : MonoBehaviour
{
    public float aggro = 0;
    public float damage = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateAggro()
    {
        // if the gameObject is the tank
        if (gameObject.tag == "Tank" && !gameObject.GetComponent<CharacterHealth>().characterIsDead)
        {
            //Once damage is implemented aggro will get added by that damage times the tanks modifier
            //aggro += whatever script that holds damage * tank modifier
        }
        else if (gameObject.tag == "Tank" && gameObject.GetComponent<CharacterHealth>().characterIsDead)
        {
            //aggro += 0 * tank modifier
        }
        // if the gameObject is the dps
        if (gameObject.tag == "DPS" && !gameObject.GetComponent<CharacterHealth>().characterIsDead)
        {
            //Once damage is implemented aggro will get added by that damage times the tanks modifier
            //aggro += whatever script that holds damage * DPS modifier
        }
        else if (gameObject.tag == "DPS" && gameObject.GetComponent<CharacterHealth>().characterIsDead)
        {
            //aggro += 0 * DPS modifier
        }
        // if the gameObject is the healer
        if (gameObject.tag == "Healer" && !gameObject.GetComponent<CharacterHealth>().characterIsDead)
        {
            //Once damage is implemented aggro will get added by that damage times the tanks modifier
            aggro += gameObject.GetComponent<HealerHealing>().healthAmount * 1f;
            Debug.Log("GameObject" + gameObject + " Aggro = " + aggro);
        }
        else if (gameObject.tag == "Healer" && gameObject.GetComponent<CharacterHealth>().characterIsDead)
        {
            aggro = 0;
        }
    }
}
