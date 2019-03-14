using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefulFunctions : MonoBehaviour
{
    /// <summary>
    /// This function finds a random point within the bounds of a collider on the x-z plane.
    /// </summary>
    /// <param name="bounds">The colliders bounds</param>
    /// <param name="inPosition">The current position of the entity asking (used to maintain y position)</param>
    /// <returns></returns>
    public Vector3 RandomPointInCollider(Bounds bounds, Vector3 inPosition)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            inPosition.y,
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    /// <summary>
    /// This function returns the square distance from the player.
    /// </summary>
    /// <param name="inPosition"></param>
    /// <returns></returns>
    public float CalculateSqrDistanceFromPlayer(Vector3 inPosition)
    {
        return (GlobalReferences.instance.playerMovement.transform.position - inPosition).sqrMagnitude;
    }

    /// <summary>
    /// This function returns the square distance from a target.
    /// </summary>
    /// <param name="inPosition"></param>
    /// <returns></returns>
    public float CalculateSqrDistanceFromTarget(Vector3 inPosition, Vector3 inTarget)
    {
        return (inTarget - inPosition).sqrMagnitude;
    }

    public float CalculateSqrDistanceFromTarget(Vector2 inPosition, Vector2 inTarget)
    {
        return (inTarget - inPosition).sqrMagnitude;
    }

    /// <summary>
    /// This function returns the direction towards the player.
    /// </summary>
    /// <param name="inPosition"></param>
    /// <returns></returns>
    public Vector3 FindPlayerDirection(Vector3 inPosition)
    {
        return (GlobalReferences.instance.playerMovement.transform.position - inPosition).normalized;
    }

    /// <summary>
    /// Rotates the object on the x-z plane to face the player
    /// </summary>
    /// <param name="inTransform"></param>
    /// <param name="rotationAmount"></param>
    /// <param name="inTurnSpeed"></param>
    public void RotateToFacePlayer(ref Transform inTransform, ref Quaternion rotationAmount, float inTurnSpeed)
    {
        Vector3 currentDirection = ((inTransform.position + inTransform.forward) - inTransform.position).normalized;
        Vector3 playerDirection = FindPlayerDirection(inTransform.position);

        Vector3 nextDirection = Vector3.Slerp(currentDirection, playerDirection, inTurnSpeed * Time.deltaTime);

        rotationAmount.SetFromToRotation(currentDirection, nextDirection);

        inTransform.rotation *= rotationAmount;
    }


    /// <summary>
    /// This function returns the direction towards a target.
    /// </summary>
    /// <param name="inPosition"></param>
    /// <returns></returns>
    public Vector3 FindTargetDirection(Vector3 inPosition, Vector3 targetPosition)
    {
        return (targetPosition - inPosition).normalized;
    }
    public Vector2 FindTargetDirection(Vector2 inPosition, Vector2 targetPosition)
    {
        return (targetPosition - inPosition).normalized;
    }



    /// <summary>
    /// Rotates the object on the x-z plane to face a target
    /// </summary>
    /// <param name="inTransform"></param>
    /// <param name="rotationAmount"></param>
    /// <param name="inTurnSpeed"></param>
    public void RotateToFaceTarget(ref Transform inTransform, ref Quaternion rotationAmount, float inTurnSpeed, Vector3 targetPosition)
    {
        Vector3 currentDirection = ((inTransform.position + inTransform.forward) - inTransform.position).normalized;
        Vector3 targetDirection = FindTargetDirection(inTransform.position, targetPosition);

        Vector3 nextDirection = Vector3.Slerp(currentDirection, targetDirection, inTurnSpeed * Time.deltaTime);

        rotationAmount.SetFromToRotation(currentDirection, nextDirection);

        inTransform.rotation *= rotationAmount;
    }

    /// <summary>
    /// Checks if a point is within the bounds of a collider.
    /// </summary>
    /// <param name="inCollider">The collider</param>
    /// <param name="inPoint">The point</param>
    /// <returns></returns>
    public bool CheckPointInBounds(Collider inCollider, Vector3 inPoint)
    {
        if (inCollider.bounds.Contains(inPoint))
        {
            return true;
        }

        return false;
    }


    public bool CheckAnimationPlaying(Animator inAnimator, string inName)
    {
        if (inAnimator.GetCurrentAnimatorStateInfo(0).IsName(inName)) return true;

        return false;
    }
}
