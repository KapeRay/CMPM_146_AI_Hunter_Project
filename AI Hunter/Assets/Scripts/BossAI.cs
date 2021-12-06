using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossAI : MonoBehaviour
{
    private List<GameObject> aggroQueue;
    private List<GameObject> tankList;
    private List<float> tankAggroList;
    private List<GameObject> dpsList;
    private List<float> dpsAggroList;
    private List<GameObject> healerList;
    private List<float> healerAggroList;
    public GameObject target;
    public Transform pathTarget;
    private int i;
    
    public bool PLAYERHITSIGNAL = false;
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


        foreach (var tank in tankList){
            tankAggroList.Add(tank.GetComponent<CharacterHealth>().aggro);
        }
        foreach (var dps in tankList){
            dpsAggroList.Add(dps.GetComponent<CharacterHealth>().aggro);
        }
        foreach (var healer in tankList){
            healerAggroList.Add(healer.GetComponent<CharacterHealth>().aggro);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (aggroQueue.Count == 0){
            if (tankList.Count == 0 && dpsList.Count == 0 && healerList.Count == 0){
                // Game over signal
                Debug.Log("Party wipe!");
            }
            else if (PLAYERHITSIGNAL){
                aggroCheck();
            }
        }

        // Update all characters' aggro lists
        if (target.GetComponent<CharacterHealth>().playerHealth <= 0) {
            if (target == null) {
                // Game over signal
                Debug.Log("Party wipe!");
            }
            target = aggroQueue[++i];
        }
    }
    
    public IEnumerator aggroCheckTimer(){
        aggroCollecting = true;
        yield return new WaitForSeconds(5.0f);
        aggroCheck();
    }

    void aggroCheck(){
        List<GameObject> problem_Children = new List<GameObject>{};

        for (int j = 0; j < dpsAggroList.Count; ++j) {
            dpsAggroList[j] = dpsList[j].GetComponent<CharacterHealth>().aggro;
        }
        for (int j = 0; j < tankAggroList.Count; ++j) {
            tankAggroList[j] = tankList[j].GetComponent<CharacterHealth>().aggro;
        }
        for (int j = 0; j < healerAggroList.Count; ++j) {
            healerAggroList[j] = healerList[j].GetComponent<CharacterHealth>().aggro;
        }

        float maxTankAggro = 0.001f;
        // Iterate through the integer arrays and grab their gameObject partners if they're meeting our criteria
        for(int i = 0; i < tankAggroList.Count; ++i){
            if (tankAggroList[i] > maxTankAggro){
                maxTankAggro = tankAggroList[i];
            } 
        }
        for (int j = 0; j < dpsAggroList.Count; ++j){
            if (dpsAggroList[j] > maxTankAggro){
                tankDropoutFlag = true;
                problem_Children.Add(dpsList[j]);
            }
        }

        for (int j = 0; j < healerAggroList.Count; ++j){
            if (healerAggroList[j] > maxTankAggro){
                tankDropoutFlag = true;
                problem_Children.Add(dpsList[j]);
            }
        }

        if(tankDropoutFlag){
            for(int i = 0; i < tankAggroList.Count; ++i){
                maxTankAggro = Random.Range(1, 100);
                if(maxTankAggro > tankDropoutThreshhold){
                    tankAggroList[i] = 0;
                }
            }
        }

        foreach (GameObject problem_child in problem_Children){
            aggroQueue.Add(problem_child);
        }

        target = aggroQueue[0];
        this.GetComponent<EnemyHunting>().target = target.transform;

        for (int j = 0; j < dpsAggroList.Count; ++j) {
            dpsList[j].GetComponent<CharacterHealth>().aggro = 0;
        }
        for (int j = 0; j < tankAggroList.Count; ++j) {
            tankList[j].GetComponent<CharacterHealth>().aggro = 0;
        }
        for (int j = 0; j < healerAggroList.Count; ++j) {
            healerList[j].GetComponent<CharacterHealth>().aggro = 0;
        }
        //getComponent
    }
}