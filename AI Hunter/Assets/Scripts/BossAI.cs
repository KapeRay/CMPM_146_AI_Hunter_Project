using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossAI : MonoBehaviour
{
    private List<GameObject> aggroQueue;
    private List<GameObject> tankList;
    private float[] tankAggroList;
    private List<GameObject> dpsList;
    private float[] dpsAggroList;
    private List<GameObject> healerList;
    private float[] healerAggroList;
    public GameObject target;
    private int i;
    
    bool PLAYERHITSIGNAL = false;
    bool aggroCollecting;
    bool tankDropoutFlag = true;
    float tankDropoutThreshhold = 60;
    // Start is called before the first frame update
    void Start()
    {
        if (tankList == null)
            tankList = GameObject.FindGameObjectsWithTag("Tank").ToList<GameObject>();
        
        if (dpsList == null)
            dpsList = GameObject.FindGameObjectsWithTag("DPS").ToList<GameObject>();

        if (healerList == null)
            healerList = GameObject.FindGameObjectsWithTag("Tank").ToList<GameObject>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (aggroQueue.Count == 0){
            if (tankList.Count == 0 && dpsList.Count == 0 && healerList.Count == 0){
                // Game over signal
            }
            else if (PLAYERHITSIGNAL){
                // Add the first player to hit the boss to the aggro queue
            }
        }

        // Update all characters' aggro lists
        if (target.GetComponent<CharacterHealth>().playerHealth <= 0) {
            target = aggroQueue[++i];
            if (target == null) {
                // Game over signal
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
        float maxTankAggro = 0.001;
        // Iterate through the integer arrays and grab their gameObject partners if they're meeting our criteria
        for(int i = 0; i < tankAggroList.Length; ++i){
            if (tankAggroList[i] > maxTankAggro){
                maxTankAggro = tankAggroList[i];
            } 
        }
        for (int j = 0; j < dpsAggroList.Length; ++j){
            if (dpsAggroList[j] > maxTankAggro){
                tankDropoutFlag = true;
                problem_Children.Add(dpsList[j]);
            }
        }

        for (int j = 0; j < healerAggroList.Length; ++j){
            if (healerAggroList[j] > maxTankAggro){
                tankDropoutFlag = true;
                problem_Children.Add(dpsList[j]);
            }
        }

        if(tankDropoutFlag){
            for(int i = 0; i < tankAggroList.Length; ++i){
                maxTankAggro = Random.Range(1, 100);
                if(maxTankAggro > tankDropoutThreshhold){
                    tankAggroList[i] = 0;
                }
            }
        }

        foreach (GameObject problem_child in problem_Children){
            aggroQueue.Add(problem_child);
        }
    }
}