using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class Ball : MonoBehaviour
{
    public Player LastController;

    private void OnCollisionExit(Collision collision)
    {

        if (collision.transform.root.GetComponent<Player>() != null)
        {
            LastController = collision.transform.root.GetComponent<Player>();
        }
    }
}