using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheWoods
{
    public class Controller : MonoBehaviour
    {
        public static Controller instance;
        [Header("Layer")]
        public LayerMask enemyLayer;
        [Header("Control")]
        public ControlPrefabs controlPrefabs;
        public ControlCanvasUI controlCanvasUI;
        void Awake()
        {
            instance = this;
        }
        void OnDestroy()
        {
            instance = null;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

