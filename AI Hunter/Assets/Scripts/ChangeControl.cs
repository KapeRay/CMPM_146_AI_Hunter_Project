using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeControl : MonoBehaviour
{
    public CharacterChanger change;


    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            change.ChangePlayer(this.gameObject);
            GetComponent<PlayerControl>().enabled = true;
        }
    }
}
