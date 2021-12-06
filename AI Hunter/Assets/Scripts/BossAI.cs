using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossAI : MonoBehaviour
{
    private List<KeyValuePair<GameObject, float>> aggroQueue = new List<KeyValuePair<GameObject, float>>();
    private List<GameObject> tankList;
    private List<float> tankAggroList = new List<float>();
    private List<GameObject> dpsList;
    private List<float> dpsAggroList = new List<float>();
    private List<GameObject> healerList;
    private List<float> healerAggroList = new List<float>();
    public GameObject target;
    public Transform pathTarget;
    private int i;
    
    public bool PLAYERHITSIGNAL = false;
    bool aggroCollecting;
    bool tankDropoutFlag = true;
    float tankDropoutThreshhold = 60;
    IEnumerator inst;
    // Start is called before the first frame update
    void Start()
    {
        if (tankList == null)
            tankList = GameObject.FindGameObjectsWithTag("Tank").ToList<GameObject>();
            Debug.Log("Tank list size = " + tankList.Count);

        if (dpsList == null)
            dpsList = GameObject.FindGameObjectsWithTag("DPS").ToList<GameObject>();

        if (healerList == null)
            healerList = GameObject.FindGameObjectsWithTag("Healer").ToList<GameObject>();

        Debug.Log(tankList.Count);

        foreach (GameObject tank in tankList){
            tankAggroList.Add(tank.GetComponent<CharacterHealth>().aggro);
        }
        foreach (GameObject dps in tankList){
            dpsAggroList.Add(dps.GetComponent<CharacterHealth>().aggro);
        }
        foreach (GameObject healer in tankList){
            healerAggroList.Add(healer.GetComponent<CharacterHealth>().aggro);
        }
        inst = aggroCheckTimer();


    }

    // Update is called once per frame
    void Update()
    {

        if (aggroQueue.Count == 0){
            
            if (tankList.Count == 0 && dpsList.Count == 0 && healerList.Count == 0){
                // Game over signal
                Debug.Log("Party wipe! 1");
            }
            else if (PLAYERHITSIGNAL){
                PLAYERHITSIGNAL = false;
                aggroCheck();
                this.GetComponent<EnemyHunting>().enabled = true;
                PLAYERHITSIGNAL = true;
            }
        }

        if (target != null && target.GetComponent<CharacterHealth>().playerHealth <= 0) {
            Debug.Log("I AM DEAD");
            if (target == null) {
                // Game over signal
                Debug.Log("Party wipe! 2");
            }
            if (i + 1 < aggroQueue.Count)
            {
                
                target = aggroQueue[++i].Key;
                
            }
            else
            {
                this.GetComponent<EnemyHunting>().enabled = false;
                PLAYERHITSIGNAL = false;
                aggroQueue.Clear();
            }
            
        }

        if (!aggroCollecting && target != null)
        {
            Debug.Log("Help Plz");

            StopCoroutine(inst);
            StartCoroutine(inst);
            
        }
        Debug.Log(aggroQueue.Count);
        // Update all characters' aggro lists

        
    }
    
    public IEnumerator aggroCheckTimer(){
        aggroCollecting = true;
        yield return new WaitForSeconds(1.0f);
        
        aggroCheck();
        aggroCollecting = false;
    }

    void aggroCheck(){
        List<KeyValuePair<GameObject, float>> problem_Children = new List<KeyValuePair<GameObject, float>> {};
        //aggroQueue.Clear();

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
        for(int j = 0; j < tankAggroList.Count; ++j){
            if (tankAggroList[j] > maxTankAggro){
                maxTankAggro = tankAggroList[j];
            } 
        }
        for (int j = 0; j < dpsAggroList.Count; ++j){
            if (dpsAggroList[j] > maxTankAggro){
                tankDropoutFlag = true;
                problem_Children.Add(new KeyValuePair<GameObject, float>(dpsList[j], dpsAggroList[j]));
            }
        }

        for (int j = 0; j < healerAggroList.Count; ++j){
            if (healerAggroList[j] > maxTankAggro){
                tankDropoutFlag = true;
                //problem_Children.Add(healerList[j]);
                problem_Children.Add(new KeyValuePair<GameObject, float>(healerList[j], healerAggroList[j]));
            }
        }

        if(tankDropoutFlag){
            for(int j = 0; j < tankAggroList.Count; ++j){
                maxTankAggro = Random.Range(1, 100);
                if(maxTankAggro > tankDropoutThreshhold){
                    tankAggroList[j] = 0.0f;
                }
            }
        }
        
        aggroQueue = new List<KeyValuePair<GameObject, float>>();
        foreach (var problem_child in problem_Children){
            aggroQueue.Add(problem_child);
        }
        for(int j = 0; j < tankList.Count; j++)
        {
            if(tankAggroList[j] > 0.001f)
            {
                aggroQueue.Add(new KeyValuePair<GameObject, float>(tankList[j], tankAggroList[j]));
            }
        }

        aggroQueue.Sort((a, b) => (a.Value >= b.Value) ? 1 : -1);
        Debug.Log("problem child count = " + problem_Children.Count);

        target = aggroQueue[0].Key;
        this.GetComponent<EnemyHunting>().target = target.transform;
        Debug.Log(this.GetComponent<EnemyHunting>().target);

        for (int j = 0; j < dpsAggroList.Count; ++j) {
            dpsList[j].GetComponent<CharacterHealth>().aggro = 0.0f;
        }
        for (int j = 0; j < tankAggroList.Count; ++j) {
            tankList[j].GetComponent<CharacterHealth>().aggro = 0.0f;
        }
        for (int j = 0; j < healerAggroList.Count; ++j) {
            healerList[j].GetComponent<CharacterHealth>().aggro = 0.0f;
        }
        
        //Debug.Log(target);
        i = 0;
        //getComponent
    }
}