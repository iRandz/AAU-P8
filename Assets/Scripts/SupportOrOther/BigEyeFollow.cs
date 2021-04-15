using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEyeFollow : MonoBehaviour
{
    [SerializeField] private Transform Player;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player);
    }
}
