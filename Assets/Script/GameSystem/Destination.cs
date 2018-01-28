using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public int Score = 1;
    private void Start()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Ball>() && other.GetComponent<Ball>().LastController)
        {
            var ball = other.GetComponent<Ball>();
            ball.LastController.Score += Score;
            ball.GetComponent<MeshRenderer>().material = ball.LastController.GetComponentInChildren<SkinnedMeshRenderer>().material;
            Destroy(ball);
        }
    }
}