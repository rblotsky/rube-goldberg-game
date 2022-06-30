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

    public Vector3 ropeDestination;

    public bool debugRegenerateRope = false;

    private void Start()
    {
        GenerateRope(hook, numLinks);
    }

    private void Update()
    {
        if (debugRegenerateRope)
        {
            RebuildRope(ropeDestination);
            debugRegenerateRope = false;
        }
    }

    private void GenerateRope(Rigidbody2D prevBod, int numSegments)
    {
        for (int i = 0; i < numSegments; i++)
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

    //recalculating and creating the required number of rope segments to span
    private void RebuildRope(Vector3 target)
    {
        //notes:
        //rope segment = 20px
        //0.25 units

        Vector2 distance = transform.position - target;
        int numSegments = Mathf.FloorToInt(distance.magnitude);
    }

    private void AdjustRopeLength(int desiredLength)
    {
        int linkCount = 0;
        GameObject currentSeg = hook.GetComponent<HingeJoint2D>().connectedBody.gameObject;
        GameObject targetLink = null; //end of linked list (will be in middle if we need to shorten rope)
        
        //traversing through the rope linked list
        while (currentSeg != null)
        {
            linkCount++;
            if (linkCount + 1 == desiredLength)
            {
                targetLink = currentSeg;
            }
            var temp = currentSeg.GetComponent<RopeSegment>().connectedBelow;
            if (temp == null && targetLink == null) //if we haven't gotten a POI segment, then save this
            {
                targetLink = currentSeg;
            }
            else
            {
                currentSeg = temp;
            }
            
        }
        //readjusting rope length
        if (linkCount > desiredLength)
        {
            RemoveSegment(targetLink);
        }
        else if (linkCount < desiredLength)
        {
            GenerateRope(targetLink.GetComponent<Rigidbody2D>(), desiredLength - linkCount);
        }
    }

    private void RemoveSegment(GameObject segToRemove)
    {
        //set prev rope seg's next as null
        segToRemove.GetComponent<RopeSegment>().connectedAbove.GetComponent<RopeSegment>().connectedBelow = null; 
        
        while (segToRemove != null)
        {
            GameObject temp = segToRemove.GetComponent<RopeSegment>().connectedBelow;
            Destroy(segToRemove);
            segToRemove = temp;
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