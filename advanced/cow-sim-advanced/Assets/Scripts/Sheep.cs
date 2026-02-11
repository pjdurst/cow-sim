using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Sheep : MonoBehaviour
{
    public float speed = 1.5f;
    
    private NavMeshAgent agent;
    private Component grassTarget = null;
    private bool isEating = false;
    private float eatTimer = 0f;
    private float eatDuration = 2.5f;
    private bool isInitialized = false;
    
    void Start()
    {
        Debug.Log("Hi, I am hungry sheep at position: " + transform.position);
        gameObject.tag = "Sheep";
        
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
            FindNearestFood();
        }
        else
        {
            Debug.LogError("Sheep is not on NavMesh! Make sure NavMesh is baked.");
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
                        FindNearestFood();
                    }
                }
            }
        }
        else if (!agent.hasPath && !isEating)
        {
            FindNearestFood();
        }
    }
    
    void FindNearestFood()
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
        
        if (nearestClover != null)
        {
            grassTarget = nearestClover;
            agent.SetDestination(nearestClover.transform.position);
            Debug.Log("Sheep targeting CLOVER");
        }
        else if (nearestGrass != null)
        {
            grassTarget = nearestGrass;
            agent.SetDestination(nearestGrass.transform.position);
            Debug.Log("Sheep targeting GRASS (no clover available)");
        }
        else
        {
            Debug.Log("No grown food available for sheep, waiting...");
            StartCoroutine(RetryFindFood());
        }
    }
    
    IEnumerator RetryFindFood()
    {
        yield return new WaitForSeconds(1.0f);
        FindNearestFood();
    }
    
    void StartEating()
    {
        isEating = true;
        eatTimer = 0f;
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
        Debug.Log("Baa baa... eating!");
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
        
        Debug.Log("Sheep finished eating! Looking for more food...");
        FindNearestFood();
    }
}