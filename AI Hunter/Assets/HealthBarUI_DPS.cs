using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI_DPS : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHP;
    public float currHP;
    void Start()
    {
        GameObject DPS = GameObject.Find("Da DPS");
        CharacterHealth characterHealth = DPS.GetComponent<CharacterHealth>();
        maxHP = characterHealth.maxHealth;
        currHP = characterHealth.playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject DPS = GameObject.Find("Da DPS");
        CharacterHealth characterHealth = DPS.GetComponent<CharacterHealth>();
        currHP = characterHealth.playerHealth;
        updateUI();
    }

    void updateUI()
    {
        // update ui
        transform.localScale = new Vector3((currHP / maxHP), 1, 1);
    }
}
