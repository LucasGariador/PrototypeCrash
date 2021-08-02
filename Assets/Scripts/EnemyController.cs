using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MovementController
{

    private IEnumerator coroutine;

    [SerializeField]
    Transform centerOfPlataform;

    Transform target;

    [Header("Time in seconds")]
    [SerializeField]
    float changeDirection = 3f;
    public override void Initialize()
    {
        
    }

    private void Awake()
    {
        target = FindClosestEnemy().transform;
        coroutine = ChangeTarget();
        StartCoroutine(coroutine);
    }


    private void Update()
    {
        ChaseEnemy();

        if (target != null)
            DoDash();

        if (EliminatePlayer())
            Destroy(gameObject);
    }

    //all the enemies and the player must be tagged as "Enemy" to be find
    private GameObject FindClosestEnemy()
    {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in gos)
            {
                if (go != gameObject)
                {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = go;
                        distance = curDistance;
                    }
                }
            }
            return closest;
    }
    // if the target hasnt been eliminated, run again the FindClosestEnemy method to replace it
    private void ChaseEnemy()
    {
        if (target != null)
        {
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        }
        else
        {
            target = FindClosestEnemy().transform;
        }

        MoveDirection = transform.forward;
    }

    
    //Every (changeDirection)seconds the character seek again for the closest enemy
    private IEnumerator ChangeTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeDirection);
            target = FindClosestEnemy().transform;
        }
    }

    // if the target distance is 3f or less, does the dash
    private void DoDash()
    {
        if (Vector3.Distance(target.position, transform.position) <= 3f)
        {
            OnDash();
            target = centerOfPlataform;
        }
    }

    // Check if the players gets push off the plataform (I use the half of the scale to get the diameter)
    private bool EliminatePlayer()
    {
        return (Vector3.Distance(transform.position, centerOfPlataform.position) > centerOfPlataform.localScale.x / 2);
    }
}
