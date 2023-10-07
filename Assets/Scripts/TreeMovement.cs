using System.Collections;
using System.Collections.Generic;
using AmplifyShaderEditor;
using UnityEngine;

public class TreeMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;

    void Update(){
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }
}
