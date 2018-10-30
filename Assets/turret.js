#pragma strict
var maxdistance : int = 1000; // dummy large distance
var mytarget : GameObject;
var cannon : GameObject;
var projectile : GameObject;
var firing : boolean=false;
var range : double;
var basic : GameObject;
function Start () {

}

function Update () {
var opposing = GameObject.FindGameObjectsWithTag("BlueTeamTriggerTag"); 
//mytarget=GameObject.FindWithTag("BlueTeamTriggerTag"); 

    for (var enemy : GameObject in opposing) {
         var distance = Vector3.Distance(enemy.transform.position, transform.position);
         if ((distance < maxdistance) && (Vector3.Distance(transform.position,enemy.transform.position)<=range))
              mytarget = enemy;  
                 
    }

    if(mytarget) {
    if((Vector3.Distance(transform.position,mytarget.transform.position)<=range ) )
    attack(mytarget);
    }
     
    if(mytarget && basic) {
    basic.transform.LookAt(mytarget.transform);
    if (Vector3.Distance (basic.transform.position, mytarget.transform.position) < 0.3f) {
				DestroyObject (basic); 
				//DestroyObject (mytarget);   //bech nrj3oha mba3d  
				mytarget.GetComponent.<playerStats1>().getHit(100,0,0,0,10,0) ;

				}
	}
  
}

function attack(col : GameObject ) {
if (!firing) {
firing=true;
basic = Instantiate(projectile,cannon.transform.position,cannon.transform.rotation ) as GameObject;
yield WaitForSeconds(1);
firing=false;
}
}