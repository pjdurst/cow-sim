using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class CowData
{
    public string name;
    public Vector3 position;
}

[System.Serializable]
public class TimestepData
{
    public int frame;
    public float time;
    public List<CowData> cows;
    public int totalGrass;
}

public class GameManager : MonoBehaviour
{
    private List<TimestepData> trackingData = new List<TimestepData>();
    private int currentFrame = 0;
    private int recordInterval = 60; // Record every 60 frames
    
    void Update()
    {
        currentFrame++;
        
        if (currentFrame % recordInterval == 0)
        {
            RecordTimestep();
        }
    }
    
    void RecordTimestep()
    {
        TimestepData data = new TimestepData
        {
            frame = currentFrame,
            time = Time.time,
            cows = new List<CowData>(),
            totalGrass = 0
        };
        
        // Get all cow positions
        GameObject[] cows = GameObject.FindGameObjectsWithTag("Cow");
        foreach (GameObject cow in cows)
        {
            data.cows.Add(new CowData
            {
                name = cow.name,
                position = cow.transform.position
            });
        }
        
        // Calculate total grass
        GameObject[] grassObjects = GameObject.FindGameObjectsWithTag("Grass");
        foreach (GameObject grassObj in grassObjects)
        {
            Grass grass = grassObj.GetComponent<Grass>();
            if (grass != null)
            {
                data.totalGrass += grass.amount;
            }
        }
        
        trackingData.Add(data);
    }
    
    void OnApplicationQuit()
    {
        OutputTrackingData();
    }
    
    void OutputTrackingData()
    {
        Debug.Log("\n========== GAME TRACKING DATA ==========");
        Debug.Log("Total timesteps recorded: " + trackingData.Count);
        
        foreach (TimestepData data in trackingData)
        {
            Debug.Log($"\n--- Frame {data.frame} ({data.time}s) ---");
            Debug.Log("Cows:");
            foreach (CowData cow in data.cows)
            {
                Debug.Log($"  {cow.name}: {cow.position}");
            }
            Debug.Log($"Total grass remaining: {data.totalGrass}");
        }
        
        SaveToFile();
    }
    
    void SaveToFile()
    {
        string path = Application.persistentDataPath + "/game_data.txt";
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("========== GAME TRACKING DATA ==========");
            writer.WriteLine($"Total timesteps: {trackingData.Count}\n");
            
            foreach (TimestepData data in trackingData)
            {
                writer.WriteLine($"\n--- Frame {data.frame} ({data.time}s) ---");
                writer.WriteLine("Cows:");
                foreach (CowData cow in data.cows)
                {
                    writer.WriteLine($"  {cow.name}: {cow.position}");
                }
                writer.WriteLine($"Total grass: {data.totalGrass}");
            }
        }
        
        Debug.Log("Data saved to: " + path);
    }
}