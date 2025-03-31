using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class TrapObject : MonoBehaviour
{
    public int DamageDealt;

    void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        DamageDealt = 10;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log($"{name} Triggered");
            FindObjectOfType<HealthBar>().LoseHealth(DamageDealt);
        }
    }
}
