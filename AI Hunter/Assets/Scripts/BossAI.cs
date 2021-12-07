using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//gameObject.GetComponent<CharacterHealth>().playerHealth
//gameObject.GetComponent<CharacterHealth>().aggro

public class BossAI : MonoBehaviour
{
    Dictionary<GameObject, CharacterHealth> playerDict; 
    GameObject[] tagBuffer;
    GameObject target;
    GameObject goBuffer;
    float tankAggroChance = 1.0f;
    float dpsAggroChance = 0.0f;
    float healAggroChance = 0.0f;
    bool recalculatingAggro;
    public bool PLAYERHITSIGNAL = false;
    float recalculateTimer = 1.0f;

    enum aggroStateEnum{
        Tank,
        Dps,
        Healer
    }
    aggroStateEnum aggroState = (aggroStateEnum)3;

    // Start is called before the first frame update
    void Start()
    {
        // Populates our dictionary
        playerDict = new Dictionary<GameObject, CharacterHealth>();
        tagBuffer = GameObject.FindGameObjectsWithTag("Tank");
        foreach (var tank in tagBuffer)
            playerDict.Add(tank, tank.GetComponent<CharacterHealth>());

        tagBuffer = GameObject.FindGameObjectsWithTag("Healer");
        foreach (var healer in tagBuffer)
            playerDict.Add(healer, healer.GetComponent<CharacterHealth>());

        tagBuffer = GameObject.FindGameObjectsWithTag("DPS");
        foreach (var dps in tagBuffer)
            playerDict.Add(dps, dps.GetComponent<CharacterHealth>());
    }

    // Update is called once per frame
    void Update()
    {
        switch(aggroState){
            case aggroStateEnum.Dps :
                foreach (var player in playerDict){
                   while(player.Key.tag == "DPS" && player.Value.playerHealth > 0){
                       target = player.Key;
                       gameObject.GetComponent<EnemyHunting>().target = target.transform;
                   }
                }
                goto case aggroStateEnum.Tank;
            case aggroStateEnum.Healer :
                foreach (var player in playerDict){
                   while(player.Key.tag == "Healer" && player.Value.playerHealth > 0){
                       target = player.Key;
                       gameObject.GetComponent<EnemyHunting>().target = target.transform;
                   }
                }
                aggroState = aggroStateEnum.Tank;
                break;
            case aggroStateEnum.Tank :
            default :
                if(PLAYERHITSIGNAL){
                    aggroCheck();
                }
                break;
        }
        
    }

    void aggroCheck(){
        // Gather what the current sum is
        float sum = 0.0f;
        foreach(var player in playerDict){
            if(player.Key.tag == "Tank"){
                sum += 2.0f;     
            }
            else{
                sum += player.Value.aggroTime;
            }
        }

        // Determine chances of getting aggro
        tankAggroChance = 0.0f;
        dpsAggroChance = 0.0f;

        foreach(var player in playerDict){
            if(player.Key.tag == "Tank"){
                tankAggroChance += player.Value.aggroTime / sum;     
            }
            else if (player.Key.tag == "DPS"){
                dpsAggroChance += player.Value.aggroTime / sum; 
            }
        }

        // determine what state we're in
        sum = Random.Range(0.0f, 1.1f);
        if(sum < tankAggroChance){
            aggroState = aggroStateEnum.Tank;
        }
        else if(sum < tankAggroChance + dpsAggroChance){
            aggroState = aggroStateEnum.Dps;
        }
        else{
            aggroState = aggroStateEnum.Healer;
        }

        healAggroChance = 1.0f - (dpsAggroChance + tankAggroChance);
    }

    IEnumerator recalculatingAggroEnum() {
        recalculatingAggro = true;
        yield return new WaitForSeconds(recalculateTimer);
        aggroCheck();
        recalculatingAggro = false;
    }
}