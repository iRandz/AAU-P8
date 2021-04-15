using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tobii.Gaming;
using UnityEngine;

public static class PlayerGaze
{
    private static float sphereRadius = 0.2f;

    public static RaycastHit FindPlayerGaze(Camera cam, bool wantTobii) // Finds the gazepoint of the player
    {
        Ray ray;
        if (wantTobii) // Allows us to avoid the expensive Tobii computations when we don't need them.
        {
            // Get gazepoint from tobii and create ray
            GazePoint gazePoint = TobiiAPI.GetGazePoint();
            if (gazePoint.IsValid && gazePoint.IsRecent())
            {
                Vector3 gazePosition = new Vector3(gazePoint.Screen.x, gazePoint.Screen.y, 0);
                ray = cam.ScreenPointToRay(gazePosition);
            }else // Use mouse position if gaze position is unavailable
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);
            }
        }
        else
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
        }
        
             
        // Find first intersection from light in direction of mouse in world space
        RaycastHit hit;
        Physics.SphereCast(ray.origin, sphereRadius, ray.direction, out hit);
        return hit;
    }
}
