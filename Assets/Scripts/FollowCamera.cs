﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
   
    [SerializeField] Transform target; // We need to know the position, that`s why it is of type Transform


 
    void Update(){
        transform.position = target.position; // The position of object this script is attached to = position of our target (player)
    }
}
