using UnityEngine;

public class SpawnNode : MonoBehaviour
{
    // This is a variable meant to indicate which object will spawn here, wether it's a normal enemy, a downwards attacking enemy, or even a potential spawn point for the player. 
    // 0 will be one enemy type for now, 1 will be another
    public int ID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Need to create an enemy object pool before I can get cracking here
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
