extends CharacterBody2D
 
var direction
var speed = 100

# Called when the node enters the scene tree for the first time.
func _ready():
	print('hi, i am hungry cow!')


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	
	# traditional player movement
	var direction = Input.get_vector("left", "right", "up", "down")
	velocity = direction * speed
	move_and_slide()
