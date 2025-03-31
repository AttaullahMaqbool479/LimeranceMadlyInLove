using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class EnemyAI : MonoBehaviour
{
    public List<Transform> points; // waypoints
    public int nextID = 0;  // int val for next point index
    public int idChangeValues = 1; // value that applies to ID for changing
    public float speed = 2.0f;
    public float health;
    public int DamageDealt;

    private void Reset()
    {
        health = 100f;
        DamageDealt = 20;
        Init();
    }

    void Init()
    {
        GetComponent<BoxCollider2D>().isTrigger = true; // Box collider to trigger
        GameObject root = new GameObject(name + "_Root");   // Create Root Object
        root.transform.position = transform.position;    //Reset pos of Root to enemy object
        transform.SetParent(root.transform);    //Set enemy object as child of root
        GameObject waypoint = new GameObject("Waypoints");    //Create waypoints object
        waypoint.transform.SetParent(root.transform);    //Reset waypoint position to root  //Make waypoints object child of root
        waypoint.transform.position = root.transform.position;

        GameObject p1 = new GameObject("Point1");   //Create 2 points and reset their pos 
        p1.transform.position = root.transform.position;
        p1.transform.SetParent(waypoint.transform); //Make points children of waypoint obj
        GameObject p2 = new GameObject("Point2");    
        p2.transform.SetParent(waypoint.transform);
        p2.transform.position = root.transform.position;  

        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);
    }
    
    void Update()
    {   
        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        Transform goalPoint = points[nextID];    //Get next point transform    
        
        if(goalPoint.transform.position.x > transform.position.x )    //Flip transform
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed*Time.deltaTime);    //Move enemy to goal point
        if(Vector2.Distance(transform.position, goalPoint.position) < 1f)    //Check the distance between enemy and goal point
        {
            //Check if we are at the end of the line, make the change
            if(nextID == points.Count - 1)
            {
                idChangeValues = -1;
            }
            //Check if we are at the start of the line, make the change
            if(nextID == 0)
            {
                idChangeValues = 1;
            }
            //Apply change of the next id
            nextID += idChangeValues;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log($"{name} Triggered");
            FindObjectOfType<HealthBar>().LoseHealth(DamageDealt);
        }
    }

    public void Hurt()
    {
        if(health <= 0)
            return;
        
        health -= FindObjectOfType<PlayerController>().GetATKforce();

        if(health <= 0)
        {
            Die();
            Debug.Log($"{name} killed!");
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}