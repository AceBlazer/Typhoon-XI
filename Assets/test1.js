

var mytarget : GameObject;
var opposingMinion : GameObject[];
var myTransform : Transform; 
var controller : CharacterController; 
var attackRange : int=1;
var moveSpeed =3; 
var i:int=1;
var j:int=0;
var maxdistance : int = 1000; // dummy large distance

function Awake() {
    myTransform = transform; 
}

function Start() {

    
}

function Update() {
    kill();
}

function kill () {
    
    var targets = GameObject.FindGameObjectsWithTag("minion2"); 
    for (var enemy : GameObject in targets)
    {
        //work out which item is the closest and shoot at it.
 
         var distance = Vector3.Distance(enemy.transform.position, transform.position);
         if (distance < maxdistance) {
              mytarget = enemy; 
              attack(mytarget);
    } 
}
}

function attack(col : GameObject) {
    transform.LookAt(col.transform);
    controller.SimpleMove(myTransform.forward*moveSpeed);
    yield WaitForSeconds(2);
    Destroy(col);
    Destroy(col);
}

