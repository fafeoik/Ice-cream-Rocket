using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateObject : MonoBehaviour
{
    [SerializeField] float rotSpeed = 50f;
    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1) * rotSpeed * Time.deltaTime);
    }
}
