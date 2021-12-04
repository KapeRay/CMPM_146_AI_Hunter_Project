using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    List<GameObject> aggroQueue;
    List<GameObject> tankList;
    float[] tankAggroList;
    List<GameObject> dpsList;
    float[] dpsAggroList;
    List<GameObject> healerList;
    float[] healerAggroList;
    
    bool PLAYERHITSIGNAL = false;
    bool aggroCollecting;
    bool tankDropoutFlag = true;
    float tankDropoutThreshhold = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        if (tankList == null)
            tankList = GameObject.FindGameObjectsWithTag("Tank");
        
        if (dpsList == null)
            dpsList = GameObject.FindGameObjectsWithTag("DPS");

        if (healerList == null)
            healerList = GameObject.FindGameObjectsWithTag("Tank");
    }

    // Update is called once per frame
    void Update()
    {
        if (aggroQueue.Length == 0){
            if (tankList.Length == 0 && dpsList.Length == 0 && healerList.Length == 0){
                // Game over signal
            }
            else if (PLAYERHITSIGNAL){
                // Add the first player to hit the boss to the aggro queue
            }
        }


    }
    
    public IEnumerator aggroCheckTimer(){
        aggroCollecting = true;
        yield return new WaitForSeconds(15.0f);
        aggroCheck();
    }

    void aggroCheck(){
        List<GameObject> problem_Children = new List<GameObject>{};
        float maxTankAggro = 0;
        // Iterate through the integer arrays and grab their gameObject partners if they're meeting our criteria
        for(int i = 0; i < tankAggroList.Length; ++i){
            if (tankAggroList[i] > maxTankAggro){
                maxTankAggro = tankAggroList[i];
            } 
        }
        for (int j = 0; j < dpsAggroList.Length; ++j){
            if (dpsAggroList[j] > maxTankAggro){
                tankDropoutFlag = true;
                problem_Children.Insert(dpsList[j], 0);
            }
        }

        for (int j = 0; j < healerAggroList.Length; ++j){
            if (healerAggroList[j] > maxTankAggro){
                tankDropoutFlag = true;
                problem_Children.Insert(dpsList[j], 0);
            }
        }

        if(tankDropoutFlag){
            for(int i = 0; i < tankAggroList.Length; ++i){
                maxTankAggro = Math.random();
                if(maxTankAggro > tankDropoutThreshhold){
                    tankAggroList.pop(i);
                }
            }
        }
    }
}