using UnityEngine;

public class Clover : MonoBehaviour
{
    public int amount = 5;
    public int maxAmount = 5;
    public bool isFullyGrown = true;
    public float growRate = 0.3f;
    public float maxScale = 3f;  // Maximum scale when fully grown
    
    private float growTimer = 0f;
    private Vector3 targetScale;
    private Color targetColor;
    private float animationSpeed = 2f;
    private float minScale = 0.2f;  // Minimum scale when eaten
    
    void Start()
    {
        Debug.Log("Hi, I am clover at " + transform.position);
        gameObject.tag = "Grass";
        
        // Set initial scale to max
        targetScale = new Vector3(maxScale, maxScale, 1);
        transform.localScale = targetScale;
        
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            targetColor = spriteRenderer.color;
            targetColor.a = 1.0f;
            spriteRenderer.color = targetColor;
        }
    }
    
    void Update()
    {
        // Smooth animation towards target scale and color
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
        
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * animationSpeed);
        }
        
        // Growth logic
        if (amount < maxAmount)
        {
            growTimer += Time.deltaTime;
            
            if (growTimer >= growRate)
            {
                amount++;
                growTimer = 0f;
                Debug.Log("Clover regrowing, amount: " + amount);
                
                // Update target values for animation
                float growthProgress = (float)amount / maxAmount;
                float currentScale = minScale + (growthProgress * (maxScale - minScale));
                targetScale = new Vector3(currentScale, currentScale, 1);
                
                if (spriteRenderer != null)
                {
                    targetColor = spriteRenderer.color;
                    targetColor.a = 0.3f + (growthProgress * 0.7f);
                }
                
                if (amount >= maxAmount)
                {
                    isFullyGrown = true;
                    targetScale = new Vector3(maxScale, maxScale, 1);
                    if (spriteRenderer != null)
                    {
                        targetColor.a = 1.0f;
                    }
                    Debug.Log("Clover fully regrown!");
                }
            }
        }
    }
    
    public void EatGrass()
    {
        amount = 0;
        isFullyGrown = false;
        growTimer = 0f;
        
        // Set target for shrinking animation to minimum scale
        targetScale = new Vector3(minScale, minScale, 1);
        
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            targetColor = spriteRenderer.color;
            targetColor.a = 0.3f;
        }
        
        Debug.Log("Clover was eaten!");
    }
    
    public bool IsGrown()
    {
        return isFullyGrown && amount >= maxAmount;
    }
}