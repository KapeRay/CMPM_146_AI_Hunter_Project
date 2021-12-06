using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//gameObject.GetComponent<CharacterHealth>().playerHealth
//gameObject.GetComponent<CharacterHealth>().aggro

public class BossAI : MonoBehaviour
{
    Dictionary<float, GameObject> playerDict; 
    GameObject[] tagBuffer;
    GameObject target;
    float tankAggroChance = 1.0f;
    float dpsAggroChance = 0.0f;
    float healAggroChance = 0.0f;
    bool recalculatingAggro;
    float recalculateTimer = 1.0f;

    enum aggroStateEnum{
        Tank,
        Dps,
        Healer
    }
    int aggroState = 3;

    // Start is called before the first frame update
    void Start()
    {
        // Populates our dictionary
        playerDict = new Dictionary<float, GameObject>();
        tagBuffer = GameObject.FindGameObjectsWithTag("Tank");
        foreach (var tank in tagBuffer)
            playerDict.Add(2.0f, tank);

        foreach (var healer in tagBuffer)
            playerDict.Add(healer.GetComponent<CharacterHealth>().aggroTime, healer);

        foreach (var dps in tagBuffer)
            playerDict.Add(dps.GetComponent<CharacterHealth>().aggroTime, dps);
    }

    // Update is called once per frame
    void Update()
    {
        switch(aggroState){
            case (int)aggroStateEnum.Dps :
            case (int)aggroStateEnum.Healer :
            case (int)aggroStateEnum.Tank :
            default :
                break;
        }
        
    }

    void aggroCheck(){
        float sum = 0.0f;
        foreach(var player in playerDict){
            if(player.Value.tag == "Tank"){
                sum += 2.0f;     
            }
            else{
                player.Key = player.GetComponent<CharacterHealth>().aggroTime;
                sum += player.Key;
            }
            
        }

        tankAggroChance = 0.0f;
        dpsAggroChance = 0.0f;
        healAggroChance = 0.0f;

        foreach(var player in playerDict){
            if(player.Value.tag == "Tank"){
                tankAggroChance += player.Key / sum;     
            }
            else if (player.Value.tag == "DPS"){
                dpsAggroChance += player.Key / sum; 
            }
        }

        sum = Random.Range(0.0f, 1.1f);
        if(sum < tankAggroChance){
            aggroState = (int)aggroStateEnum.Tank;
        }
        else if(sum < tankAggroChance + dpsAggroChance){
            aggroState = (int)aggroStateEnum.Dps;
        }
        else{
            aggroState = (int)aggroStateEnum.Healer;
        }
    }

    IEnumerator recalculatingAggroEnum() {
        recalculatingAggro = true;
        yield return new WaitForSeconds(recalculateTimer);
        aggroCheck();
        recalculatingAggro = false;
    }
}