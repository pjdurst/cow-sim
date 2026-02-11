using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Cow : MonoBehaviour
{
    public float speed = 2f;
    
    private NavMeshAgent agent;
    private Component grassTarget = null;
    private bool isEating = false;
    private float eatTimer = 0f;
    private float eatDuration = 2.0f;
    private bool isInitialized = false;
    
    void Start()
    {
        Debug.Log("Hi, I am hungry cow at position: " + transform.position);
        gameObject.tag = "Cow";
        
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
        }
        
        agent.speed = speed;
        agent.angularSpeed = 0;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.stoppingDistance = 1.5f;
        
        StartCoroutine(WaitForNavMesh());
    }
    
    IEnumerator WaitForNavMesh()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (agent.isOnNavMesh)
        {
            isInitialized = true;
            FindNearestGrass();
        }
        else
        {
            Debug.LogError("Cow is not on NavMesh! Make sure NavMesh is baked.");
        }
    }
    
    void Update()
    {
        if (!isInitialized || agent == null || !agent.isOnNavMesh)
        {
            return;
        }
        
        if (isEating)
        {
            eatTimer += Time.deltaTime;
            if (eatTimer >= eatDuration)
            {
                FinishEating();
            }
            return;
        }
        
        if (!agent.pathPending && agent.hasPath)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (agent.velocity.sqrMagnitude < 0.01f)
                {
                    bool isGrown = false;
                    
                    if (grassTarget is Grass grass)
                    {
                        isGrown = grass.IsGrown();
                    }
                    else if (grassTarget is Clover clover)
                    {
                        isGrown = clover.IsGrown();
                    }
                    
                    if (grassTarget != null && isGrown)
                    {
                        StartEating();
                    }
                    else
                    {
                        FindNearestGrass();
                    }
                }
            }
        }
        else if (!agent.hasPath && !isEating)
        {
            FindNearestGrass();
        }
    }
    
    void FindNearestGrass()
    {
        if (!isInitialized || agent == null || !agent.isOnNavMesh)
        {
            return;
        }
        
        GameObject[] grassObjects = GameObject.FindGameObjectsWithTag("Grass");
        
        Grass nearestGrass = null;
        Clover nearestClover = null;
        float minGrassDistance = Mathf.Infinity;
        float minCloverDistance = Mathf.Infinity;
        
        foreach (GameObject grassObj in grassObjects)
        {
            Grass grass = grassObj.GetComponent<Grass>();
            if (grass != null && grass.IsGrown())
            {
                float distance = Vector3.Distance(transform.position, grassObj.transform.position);
                if (distance < minGrassDistance)
                {
                    minGrassDistance = distance;
                    nearestGrass = grass;
                }
            }
            
            Clover clover = grassObj.GetComponent<Clover>();
            if (clover != null && clover.IsGrown())
            {
                float distance = Vector3.Distance(transform.position, grassObj.transform.position);
                if (distance < minCloverDistance)
                {
                    minCloverDistance = distance;
                    nearestClover = clover;
                }
            }
        }
        
        if (nearestGrass != null)
        {
            grassTarget = nearestGrass;
            agent.SetDestination(nearestGrass.transform.position);
            Debug.Log("Cow targeting GRASS");
        }
        else if (nearestClover != null)
        {
            grassTarget = nearestClover;
            agent.SetDestination(nearestClover.transform.position);
            Debug.Log("Cow targeting CLOVER (no grass available)");
        }
        else
        {
            Debug.Log("No grown grass or clover available, waiting...");
            StartCoroutine(RetryFindGrass());
        }
    }
    
    IEnumerator RetryFindGrass()
    {
        yield return new WaitForSeconds(1.0f);
        FindNearestGrass();
    }
    
    void StartEating()
    {
        isEating = true;
        eatTimer = 0f;
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
        Debug.Log("Nom nom nom... eating grass!");
    }
    
    void FinishEating()
    {
        if (grassTarget != null)
        {
            if (grassTarget is Grass grass)
            {
                grass.EatGrass();
            }
            else if (grassTarget is Clover clover)
            {
                clover.EatGrass();
            }
        }
        
        grassTarget = null;
        isEating = false;
        
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }
        
        Debug.Log("Finished eating! Looking for more grass...");
        FindNearestGrass();
    }
}