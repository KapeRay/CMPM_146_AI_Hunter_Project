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
    GameObject target = null;
    GameObject goBuffer;
    float tankAggroChance = 1.0f;
    float dpsAggroChance = 0.0f;
    float healAggroChance = 0.0f;
    bool recalculatingAggro;
    public bool PLAYERHITSIGNAL = false;
    bool tankDropoutSignal;
    float tankDropoutRate = 0.6f;
    float recalculateTimer = 3.0f;
    IEnumerable<GameObject> targetGen;

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
        if(PLAYERHITSIGNAL && !recalculatingAggro){
            StopCoroutine(recalculatingAggroEnum());
            StartCoroutine(recalculatingAggroEnum());
        }

        switch(aggroState){
            case aggroStateEnum.Dps :
                lock(playerDict){
                    foreach (var player in playerDict){
                        if(player.Key.tag == "DPS" && player.Value.playerHealth > 0){
                            target = player.Key;
                        } 
                    }
                }
                if(target == null){
                    goto case aggroStateEnum.Tank;
                }
                break;
            case aggroStateEnum.Healer :
                lock(playerDict){
                    foreach (var player in playerDict){
                        if(player.Key.tag == "Healer" && player.Value.playerHealth > 0){
                            target = player.Key;
                            break;
                        } 
                    }
                }
                if(target == null){
                    aggroState = aggroStateEnum.Tank;
                }
                break;
            case aggroStateEnum.Tank :
                lock(playerDict){
                    foreach (var player in playerDict){
                        if(player.Key.tag == "Tank" && player.Value.playerHealth > 0){
                            target = player.Key;
                            break;
                        } 
                    }
                }
                break;
            case (aggroStateEnum)3:
                break;
        }

        if (target != null){
            gameObject.GetComponent<EnemyHunting>().target = target.transform;
            gameObject.GetComponent<EnemyHunting>().enabled = true;
            Debug.Log("the lad: " + gameObject.GetComponent<EnemyHunting>().target);
            if(target.GetComponent<CharacterHealth>().playerHealth < 0){
                gameObject.GetComponent<EnemyHunting>().enabled = false;
                target = null;
            }
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
            if(player.Key.tag == "Tank" && player.Value.playerHealth > 0){
                tankAggroChance += player.Value.aggroTime / sum;     
            }
            else if (player.Key.tag == "DPS" && player.Value.playerHealth > 0){
                dpsAggroChance += player.Value.aggroTime / sum; 
                if(player.Value.aggroTime > 2.0f){
                    tankDropoutSignal = true;
                }
            }
            else {
                if(player.Value.aggroTime > 2.0f){
                    tankDropoutSignal = true;
                }
            }
        }
        Debug.Log(tankDropoutSignal);
        // determine what state we're in
        sum = Random.Range(0.0f, 1.0f);
        if(tankDropoutSignal){
            if(sum < tankAggroChance && Random.Range(0.0f, 1.1f) < (1.0f - tankDropoutRate)){
                aggroState = aggroStateEnum.Tank;
                Debug.Log("Tank!");
            }
            else if( (sum < tankAggroChance + dpsAggroChance) || (tankDropoutSignal && sum < dpsAggroChance) ){
                aggroState = aggroStateEnum.Dps;
                Debug.Log("DPS");
            }
            else{
                aggroState = aggroStateEnum.Healer;
            }
        }
        else {
            if(sum < tankAggroChance * 2){
                aggroState = aggroStateEnum.Tank;
                Debug.Log("Tank!");
            }
            else if(sum < (tankAggroChance * 2 + dpsAggroChance)){
                aggroState = aggroStateEnum.Dps;
                Debug.Log("DPS");
            }
            else{
                aggroState = aggroStateEnum.Healer;
            }
        }
        
        healAggroChance = 1.0f - (dpsAggroChance + tankAggroChance);
        Debug.Log(aggroState);
        Debug.Log("RnG :" + sum);
        Debug.Log("Tank :" + tankAggroChance);
        Debug.Log("Heal :" + healAggroChance);
        Debug.Log("DPS :" + dpsAggroChance);
       
    }

    IEnumerator recalculatingAggroEnum() {
        recalculatingAggro = true;
        aggroCheck();
        yield return new WaitForSeconds(recalculateTimer);
        recalculatingAggro = false;
    }
}