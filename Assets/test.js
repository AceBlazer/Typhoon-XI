

var mytarget : GameObject;
var opposingMinion : GameObject[];
var myTransform : Transform; 
var controller : CharacterController; 
var attackRange : int=1;
var moveSpeed =3; 
var i:int=1;
var j:int=0;
var maxdistance : int = 5; // dummy large distance
var target : GameObject;

function Awake() {
    myTransform = transform; 
}

function Start() {

   
}



function Update () {
    var targets = GameObject.FindGameObjectsWithTag("minion1"); 
 





    for (var enemy : GameObject in targets)
    {
        //work out which item is the closest and shoot at it.
 
         var distance = Vector3.Distance(enemy.transform.position, transform.position);
         if (distance < maxdistance) {
              mytarget = enemy; 
        //   attack(mytarget); 
    }

    } 

if(mytarget==null) {

transform.LookAt(target.transform);
controller.SimpleMove(myTransform.forward*moveSpeed);
}
else {
    attack(mytarget);
}
}



function attack(col : GameObject) {
    transform.LookAt(col.transform);
    controller.SimpleMove(myTransform.forward*moveSpeed);
    yield WaitForSeconds(2);
    Destroy(col);
}

