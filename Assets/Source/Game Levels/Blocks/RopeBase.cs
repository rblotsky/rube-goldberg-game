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
    public GameObject firstSeg = null;
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
            if (i == 0 && firstSeg == null)
            {
                firstSeg = newSeg;
            }
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
        

        Vector2 distance = target - transform.position;
        float angle = (Mathf.Rad2Deg * Mathf.Atan2(distance.y, distance.x)) - 90f;
        if (angle < 0)
        {
            angle += 360;
        }
        
        int numSegments = Mathf.FloorToInt(distance.magnitude / 0.25f);
        Debug.Log("We would generate " + numSegments + " segments, angle of " + angle);
        transform.eulerAngles = (Vector3.zero);
        transform.Rotate(Vector3.forward, angle);
        
        AdjustRopeLength(numSegments);
        ArrangeRope(numSegments, target, angle);
        
    }

    private void ArrangeRope(int numSegments, Vector2 target, float angle)
    {
        GameObject curSeg = firstSeg;
        int count = 1;
        while (curSeg != null)
        {
            curSeg.transform.position = Vector2.Lerp(transform.position, target, count / (float)numSegments);
            curSeg.transform.eulerAngles = Vector3.zero;
            curSeg.transform.Rotate(Vector3.forward, angle);
            curSeg = curSeg.GetComponent<RopeSegment>().connectedBelow;
            count++;
        }
    }

    private GameObject getEndOfRope()
    {
        GameObject curSeg = firstSeg;
        while (curSeg != null)
        {
            GameObject temp = curSeg.GetComponent<RopeSegment>().connectedBelow;
            if (temp == null)
            {
                return curSeg;
            }

            curSeg = temp;
        }

        return null;
    }

    private void AdjustRopeLength(int desiredLength)
    {
        int linkCount = 0;
        GameObject currentSeg = firstSeg;
        GameObject targetLink = getEndOfRope(); //end of linked list (will be in middle if we need to shorten rope)
        
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
            currentSeg = temp;
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