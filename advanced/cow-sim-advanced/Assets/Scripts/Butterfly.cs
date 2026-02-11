using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Butterfly : MonoBehaviour
{
    public float speed = 4f;
    
    private NavMeshAgent agent;
    private Component plantTarget = null;
    private bool isHelping = false;
    private bool isInitialized = false;
    
    void Start()
    {
        Debug.Log("Hi, I am a helpful butterfly at position: " + transform.position);
        gameObject.tag = "Butterfly";
        
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
            FindPlantToHelp();
        }
        else
        {
            Debug.LogError("Butterfly is not on NavMesh! Make sure NavMesh is baked.");
        }
    }
    
    void Update()
    {
        if (!isInitialized || agent == null || !agent.isOnNavMesh)
        {
            return;
        }
        
        if (isHelping)
        {
            bool isFullyGrown = false;
            
            if (plantTarget is Grass grass)
            {
                isFullyGrown = grass.IsGrown();
            }
            else if (plantTarget is Clover clover)
            {
                isFullyGrown = clover.IsGrown();
            }
            
            if (isFullyGrown)
            {
                StopHelping();
            }
            
            return;
        }
        
        if (!agent.pathPending && agent.hasPath)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (agent.velocity.sqrMagnitude < 0.01f)
                {
                    bool needsHelp = false;
                    
                    if (plantTarget is Grass grass)
                    {
                        needsHelp = !grass.IsGrown();
                    }
                    else if (plantTarget is Clover clover)
                    {
                        needsHelp = !clover.IsGrown();
                    }
                    
                    if (plantTarget != null && needsHelp)
                    {
                        StartHelping();
                    }
                    else
                    {
                        FindPlantToHelp();
                    }
                }
            }
        }
        else if (!agent.hasPath && !isHelping)
        {
            FindPlantToHelp();
        }
    }
    
    void FindPlantToHelp()
    {
        if (!isInitialized || agent == null || !agent.isOnNavMesh)
        {
            return;
        }
        
        GameObject[] grassObjects = GameObject.FindGameObjectsWithTag("Grass");
        
        Component nearestPlant = null;
        float minDistance = Mathf.Infinity;
        
        foreach (GameObject grassObj in grassObjects)
        {
            Grass grass = grassObj.GetComponent<Grass>();
            if (grass != null && !grass.IsGrown())
            {
                float distance = Vector3.Distance(transform.position, grassObj.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPlant = grass;
                }
            }
            
            Clover clover = grassObj.GetComponent<Clover>();
            if (clover != null && !clover.IsGrown())
            {
                float distance = Vector3.Distance(transform.position, grassObj.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPlant = clover;
                }
            }
        }
        
        if (nearestPlant != null)
        {
            plantTarget = nearestPlant;
            agent.SetDestination(nearestPlant.transform.position);
            
            if (nearestPlant is Grass)
                Debug.Log("Butterfly heading to help GRASS regrow");
            else
                Debug.Log("Butterfly heading to help CLOVER regrow");
        }
        else
        {
            Debug.Log("No plants need help, butterfly waiting...");
            StartCoroutine(RetryFindPlant());
        }
    }
    
    IEnumerator RetryFindPlant()
    {
        yield return new WaitForSeconds(1.0f);
        FindPlantToHelp();
    }
    
    void StartHelping()
    {
        isHelping = true;
        
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
        
        if (plantTarget is Grass grass)
        {
            grass.growRate = grass.growRate / 2f;
            Debug.Log("Butterfly helping GRASS regrow faster!");
        }
        else if (plantTarget is Clover clover)
        {
            clover.growRate = clover.growRate / 2f;
            Debug.Log("Butterfly helping CLOVER regrow faster!");
        }
    }
    
    void StopHelping()
    {
        if (plantTarget is Grass grass)
        {
            grass.growRate = 1.0f;
            Debug.Log("Grass fully regrown! Butterfly leaving.");
        }
        else if (plantTarget is Clover clover)
        {
            clover.growRate = 0.3f;
            Debug.Log("Clover fully regrown! Butterfly leaving.");
        }
        
        plantTarget = null;
        isHelping = false;
        
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }
        
        FindPlantToHelp();
    }
}