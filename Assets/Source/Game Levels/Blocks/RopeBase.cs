//base of script attributed to juul1a from https://youtu.be/yQiR2-0sbNw
using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RubeGoldbergGame
{
    public class RopeBase : MonoBehaviour, IPropertiesComponent

    {
    public Rigidbody2D hook;
    public GameObject[] prefabRopeSegs;
    public int numLinks = 5;

    private void Start()
    {
        GenerateRope();
    }

    private void GenerateRope()
    {
        Rigidbody2D prevBod = hook;
        for (int i = 0; i < numLinks; i++)
        {
            int index = Random.Range(0, prefabRopeSegs.Length);
            GameObject newSeg = Instantiate(prefabRopeSegs[index]);
            newSeg.transform.parent = transform;
            newSeg.transform.position = transform.position;
            HingeJoint2D hj = newSeg.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBod;

            prevBod = newSeg.GetComponent<Rigidbody2D>();
        }
    }
    
    //interface functions
    public void ActivateSelectionPanel(UISelectionBox selectionPanel)
    {
        throw new NotImplementedException();
    }

    public string SaveProperties()
    {
        throw new NotImplementedException();
    }

    public void LoadProperties(string[] propertyStrings)
    {
        throw new NotImplementedException();
    }
    }
}