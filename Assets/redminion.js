﻿
var target : GameObject; 
var mytarget : GameObject;
var bh: GameObject;
var myTransform : Transform; 
var controller : CharacterController; 
var viewRange : int=10;
var attackRange : int=2;
var moveSpeed = 1; 
var health : int;
var damage=100;
var maxdistance : int = 1000; // dummy large distance
var nav : NavMeshAgent;
var canmove : boolean= true;

function Awake() {
    myTransform = transform; 
}

function Start() {
    health = 500;
    target = GameObject.FindWithTag("node3");  
    
}

function Update () {
///ki no9tlohom lkol win nemchiw 
// avoid collision


    var opposingMinion = GameObject.FindGameObjectsWithTag("BlueTeamTriggerTag"); 

    for (var enemy : GameObject in opposingMinion) {
         var distance = Vector3.Distance(enemy.transform.position, transform.position);
         if ((distance < maxdistance) && (Vector3.Distance(transform.position,enemy.transform.position)<=attackRange))
              mytarget = enemy;  
               //   target=mytarget;
    }
          


    canmove=true;
    if(canmove == true) {

        if(nav.destination != target.transform.position)
        nav.destination = target.transform.position;
        } 


  /* if((Vector3.Distance(transform.position,mytarget.transform.position)<=viewRange) && mytarget){
    canmove=false;
    }*/

    if(mytarget) {
    if((Vector3.Distance(transform.position,mytarget.transform.position)<=attackRange) && mytarget) {
    transform.LookAt(mytarget.position);
    nav.destination = mytarget.transform.position;
    DestroyObject(mytarget);   //bech nraj3oha mba3d   
   }
   else {
   nav.destination = target.transform.position;
   }
   }
}







    /*
    
        if(nav.destination != target.transform.position)
        nav.destination = target.transform.position;
             
       */