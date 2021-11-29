using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    // Implement movement script here?
        GameObject target = GameObject.Find("Da BOSS"); // Change variable type once we can specifically find an enemy.
        bool canAttack = true;
        float attackTimer;

    public void playerControl(){
        // Set up control structure here. Its simple enough we probably dont need a FSM.

    }

    public void setTarget(GameObject targeted){
        this.target = targeted;
    }

    public virtual void act(){
        if(canAttack){
            StopCoroutine(attackCoroutine());
            StartCoroutine(attackCoroutine());
        }
    }

    public IEnumerator attackCoroutine(){
        canAttack = false;
        Debug.Log("Player hit Da BOSS"); // Needs updates. Sufficient for first build.
        yield return new WaitForSeconds(attackTimer);
        canAttack = true;
    }
}
