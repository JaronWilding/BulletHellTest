using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jaron.BulletHell.InputManagement.Interfaces;

namespace Jaron.BulletHell.Ship
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Transform playerTransform;
        private Vector3 input;
        private IINputManager inputManager;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            input = inputManager.GetMouseVector();
            input.z = input.y;
        }
    }
}