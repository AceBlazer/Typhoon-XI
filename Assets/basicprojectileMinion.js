﻿#pragma strict

var target : Transform;
var speed = 2;

function Start () {
 target = GameObject.FindWithTag("minion2").transform;  
}

function Update () {
transform.LookAt(target.transform);
//transform.position=target.position ;
transform.Translate(Vector3.forward * 4 * Time.deltaTime);

if(Vector3.Distance(this.transform.position,target.position)<0.1f)
Destroy(gameObject);
}