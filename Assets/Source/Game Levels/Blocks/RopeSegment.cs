//base of script attributed to juul1a from https://youtu.be/yQiR2-0sbNw

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    public class RopeSegment : MonoBehaviour
    {
        //DATA
        public GameObject connectedAbove, connectedBelow;

        //Start Script
        private void Start()
        {
            //set the next or previous ropes
            connectedAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
            RopeSegment aboveSegment = connectedAbove.GetComponent<RopeSegment>();
            if (aboveSegment != null)
            {
                aboveSegment.connectedBelow = gameObject;
                //float spriteBottom = connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y;
                float spriteBottom = 0.25f;
                //Debug.Log("sprite bottom is " + spriteBottom);
                GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, spriteBottom * -1);
            }
            else
            {
                GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);
            }
        }
    }
    
}