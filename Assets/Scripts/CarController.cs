using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public void Move(float direction)
    {
        transform.Translate(Vector3.right * direction * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1.5f, 1.5f), transform.position.y , transform.position.z);
    }
}
