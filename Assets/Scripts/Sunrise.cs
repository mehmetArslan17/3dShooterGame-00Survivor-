using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunrise : MonoBehaviour
{
  [Range(0,20) ] [SerializeField] float _sunriseSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(new Vector3(250, 0, 250),Vector3.right, _sunriseSpeed * Time.deltaTime);
    }
}
