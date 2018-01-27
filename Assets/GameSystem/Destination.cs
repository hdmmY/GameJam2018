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

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Ball>() && other.GetComponent<Ball>().LastController)
        {

            other.GetComponent<Ball>().LastController.Score += Score;
        }
    }
}