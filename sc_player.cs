using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;


public class sc_Player : MonoBehaviour
{
    double pos_x;
    double pos_y;

    sc_Player playerObject;
    
    private int WoodCount=0;
    public void increaseWoodCount()
    {
        this.WoodCount++;
    }
    public int getWoodCount()
    {
        return this.WoodCount;
    }
    // printPlayerPos metodunu public yapın
    public void printPlayerPos()
    {
        Debug.Log("Player position: (" + pos_x + ", " + pos_y + ")");
    }
    


    // Start is called before the first frame update
    public void Start()
    {
        // Initialize player properties
        
        pos_x = transform.position.x;
        pos_y = transform.position.y;
        
        // Print the player's starting position
        printPlayerPos();
    }

    // Update is called once per frame
    void Update()
    {
        // Update player position dynamically (example usage)
        pos_x = transform.position.x;
        pos_y = transform.position.y;
    }
}
