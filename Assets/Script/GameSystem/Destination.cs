using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public int Score = 1;
    public AudioSource SoundEffect;
    private void Start()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Ball>() && other.GetComponent<Ball>().LastController)
        {
            SoundEffect.Play();
            var ball = other.GetComponent<Ball>();
            ball.LastController.Score += Score;
            ball.GetComponent<MeshRenderer>().material = ball.LastController.GetComponentInChildren<SkinnedMeshRenderer>().material;
            Destroy(ball);
            GameObject.Find("GameSystem").GetComponent<ObjectManager>().Spawn();
        }
    }
}