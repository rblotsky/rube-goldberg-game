using System;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ObjectSpringPlate : MonoBehaviour
    {
        private Rigidbody2D myRigidbody;
        public float pushForce = 1000;

        private void Awake()
        {
            myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            //Debug.Log("I push");
            if (col.gameObject.tag == "ActiveObjective")
            {
                Debug.Log("I will push");
                myRigidbody.AddForce(this.transform.right * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}