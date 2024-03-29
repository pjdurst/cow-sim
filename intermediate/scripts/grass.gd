extends Area2D

var pos = Vector2(50,50)
var amount: int = 5

# Called when the node enters the scene tree for the first time.
func _ready():
	print('hi, i am grass!')
	position = pos

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	if amount < 5:
		amount += 1
		print('i am regrowing')
