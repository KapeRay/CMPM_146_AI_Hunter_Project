using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterChanger : MonoBehaviour
{
    public GameObject[] DPS;
    public GameObject[] HEALER;
    public GameObject[] TANK;
    public GameObject[] players;
    public GameObject dps;
    public GameObject tank;
    public GameObject healer;

    [SerializeField]
    GameObject currentPlayer;
    // Start is called before the first frame update
    void Start()
    {
        DPS = GameObject.FindGameObjectsWithTag("DPS");
        HEALER = GameObject.FindGameObjectsWithTag("Healer");
        TANK = GameObject.FindGameObjectsWithTag("Tank");
        players = DPS.Concat(HEALER).ToArray().Concat(TANK).ToArray();
        //Debug.Log(players.Length);

        for (int i = 1; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerControl>().enabled = false;
        }
        currentPlayer = players[0];
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            dps.GetComponent<PlayerControl>().enabled = true;
            tank.GetComponent<PlayerControl>().enabled = false;
            healer.GetComponent<PlayerControl>().enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            dps.GetComponent<PlayerControl>().enabled = false;
            tank.GetComponent<PlayerControl>().enabled = true;
            healer.GetComponent<PlayerControl>().enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            dps.GetComponent<PlayerControl>().enabled = false;
            tank.GetComponent<PlayerControl>().enabled = false;
            healer.GetComponent<PlayerControl>().enabled = true;
        }
    }

    public void ChangePlayer(GameObject player)
    {
        currentPlayer.GetComponent<PlayerControl>().enabled = false;
        currentPlayer = player;
    }
    
}
