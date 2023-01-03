using System;
using System.Collections;
using TriplanoTest.Data;
using TriplanoTest.Shared;
using TriplanoTest.Shared.ExtensionMethods;
using UnityEngine;

namespace TriplanoTest.AI
{
    /// <summary>
    /// ***** WARNING *****
    /// 
    /// I do not recommend using this type of programming in serious projects.
    /// It is always recommended to use a Behavior Tree or FSM for AI creation, and I don't have any that are ideal for this type of case.
    /// The reason why I did it this way is because the test prevented the use of external assets.
    /// </summary>
    public class Guard : MonoBehaviour 
    {
        [Serializable]
        private class Waypoint
        {
            public bool stopWhenReaching = true;
            public Transform point;
        }

        [Header("Guard")]
        [SerializeField] private GuardData data;
        [SerializeField] private GameEvent onFoundPlayer;
        [Space]
        [SerializeField] private Animator animator;
        [SerializeField] private ViewDetection[] viewDetections;

        [Header("Animation States")]
        [SerializeField] private string idle = "Idle";
        [SerializeField] private string walk = "Walk";
        [SerializeField] private string speedMultiplierParameter = "SpeedMultiplier";

        [Header("Patrol Points")]
        [SerializeField] private Waypoint[] patrolPoints;

        private Transform player;
        private int currentPoint;
        private bool patrol;
        private bool foundPlayer;

        private const float Threshold = 0.2f;

        private void OnEnable()
        {
            animator.SetFloat(speedMultiplierParameter, data.AnimationSpeed);
            // Reset
            transform.localPosition = Vector2.zero;
            transform.localRotation = Quaternion.identity;

            currentPoint = 0;
            foundPlayer = false;

            StartCoroutine(WaitDelay());
        }

        private void FixedUpdate()
        {
            if (!foundPlayer)
            {
                if (patrol)
                {
                    Patrol();
                }

                TryToFindPlayer();
            }
            else
            {
                LookAt(player.position);
            }
        }

        private void Patrol()
        {
            Vector3 targetPosition = patrolPoints[currentPoint].point.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, data.MoveSpeed * Time.fixedDeltaTime);

            LookAt(targetPosition);

            if (Vector3.Distance(transform.position, targetPosition) <= Threshold)
            {
                bool stop = patrolPoints[currentPoint].stopWhenReaching;
                currentPoint = currentPoint.Navigate(patrolPoints.Length, true);

                if (stop)
                {
                    StartCoroutine(WaitDelay());
                }
            }
        }

        private IEnumerator WaitDelay()
        {
            Play(idle);
            patrol = false;

            yield return new WaitForSeconds(data.WaitTime);

            Play(walk);
            patrol = true;
        }

        private void TryToFindPlayer()
        {
            foreach (ViewDetection detection in viewDetections)
            {
                if (detection.FindTargetInsideRange(out player))
                {
                    StopAllCoroutines();
                    onFoundPlayer.Invoke();
                    Play(idle);

                    foundPlayer = true;
                    return;
                }
            }
        }

        private void LookAt(Vector3 target)
        {
            Vector3 lookDirection = target - transform.position;
            lookDirection.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, data.RotationSpeed * Time.fixedDeltaTime);
        }

        private void Play(string stateName, float transition = 0.25f, int layerIndex = 0)
        {
            AnimatorStateInfo current = animator.GetCurrentAnimatorStateInfo(layerIndex);
            animator.CrossFadeInFixedTime(stateName, transition, layerIndex, current.normalizedTime);
        }
    }
}