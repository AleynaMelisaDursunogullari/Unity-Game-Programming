using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class parallax : MonoBehaviour
{
    private float lengt, startpos;
    public GameObject cam;
    public float parallaxeffect;

    // Start is called before the first frame update
    void Start()
    {
        startpos=transform.position.x;
        lengt=GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp =(cam.transform.position.x *(1-parallaxeffect));
        float dist=(cam.transform.position.x * parallaxeffect);
        transform.position=new Vector3(startpos+ dist, transform.position.y, transform.position.z);
        if(temp>startpos+lengt) {startpos+=lengt;}
        else if(temp<startpos-lengt){startpos-=lengt;} 
    }
}