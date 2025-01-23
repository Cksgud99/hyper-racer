using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public void Move(float direction)
    {
        transform.Translate(Vector3.right * direction * Time.deltaTime);
    }
}
