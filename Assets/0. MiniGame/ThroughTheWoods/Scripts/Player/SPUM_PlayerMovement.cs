using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ThroughTheWoods
{
    public class SPUM_PlayerMovement : MonoBehaviour
    {
        [Header("Properties")]
        public float runSpeed;
        Rigidbody2D rb;
        Vector2 movement;
        bool isFacingRight = false;
        public SPUM_Prefabs _prefabs;
        private PlayerState _currentState;
        public bool isAction = false;
        public Dictionary<PlayerState, int> IndexPair = new();
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();

            if (_prefabs == null)
            {
                _prefabs = transform.GetChild(0).GetComponent<SPUM_Prefabs>();
                if (!_prefabs.allListsHaveItemsExist())
                {
                    _prefabs.PopulateAnimationLists();
                }
            }
            _prefabs.OverrideControllerInit();
            foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
            {
                IndexPair[state] = 0;
            }
        }
        public void SetStateAnimationIndex(PlayerState state, int index = 0)
        {
            IndexPair[state] = index;
        }
        public void PlayStateAnimation(PlayerState state)
        {
            _prefabs.PlayAnimation(state, IndexPair[state]);
        }
        void Update()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        void FixedUpdate()
        {
            if (isAction) return;

            if (movement.magnitude < 0.1f)
            {
                _currentState = PlayerState.IDLE;
            }
            else
            {
                _currentState = PlayerState.MOVE;
            }

            switch (_currentState)
            {
                case PlayerState.IDLE:
                    StopMovement();
                    break;
                case PlayerState.MOVE:
                    Movement();
                    break;
            }
            PlayStateAnimation(_currentState);
        }
        public void FaceControl(float dir)
        {
            if (dir < 0 && isFacingRight)
            {
                FlipFace();
            }
            else if (dir > 0 && !isFacingRight)
            {
                FlipFace();
            }
        }
        public void FlipFace()
        {
            isFacingRight = !isFacingRight;
            Vector2 theScale = transform.localScale;
            theScale.x = -theScale.x;
            transform.localScale = theScale;
        }
        public void Movement()
        {
            //transform.position += _dirMVec * _charMS * Time.deltaTime;
            FaceControl(movement.x);
            rb.linearVelocity = runSpeed * movement.normalized;
        }
        public void StopMovement()
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}

