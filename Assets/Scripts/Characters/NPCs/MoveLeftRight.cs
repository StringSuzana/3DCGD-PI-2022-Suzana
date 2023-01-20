using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCs
{
    public class MoveLeftRight : MonoBehaviour
    {
        private Vector3 dir = Vector3.left;
        private Vector3 startPosition;

        private float speed = 2;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            transform.Translate(dir * (speed * Time.deltaTime));

            if (transform.position.x <= startPosition.x - 4)
            {
                dir = Vector3.right;
            }
            else if (transform.position.x >= startPosition.x + 4)
            {
                dir = Vector3.left;
            }
        }
    }
}