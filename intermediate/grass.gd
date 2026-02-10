extends Area2D

var amount: int = 5 # How much Grass is available
var max_amount: int = 5
var is_fully_grown = true
var grow_timer = 0.0
var grow_rate = 1.0 # How fast the Grass regrows

func _ready():
	print('hi, i am grass!')
	add_to_group("Grass")

func _process(delta):
	if amount < max_amount:
		# Regrow at each time step
		grow_timer += delta 
		
		if grow_timer >= grow_rate:
			amount += 1
			grow_timer = 0.0
			print('i am regrowing, amount: ', amount)
			
			# Visualize growth rate
			var growth_progress = float(amount) / float(max_amount)
			scale = Vector2(growth_progress, growth_progress)
			modulate.a = 0.3 + (growth_progress * 0.7)
			
			if amount >= max_amount:
				is_fully_grown = true
				scale = Vector2(1.0, 1.0)
				modulate.a = 1.0
				print('grass fully regrown!')

func eat_grass():
	# When the Cow finishes eating
	amount = 0
	is_fully_grown = false
	grow_timer = 0.0
	scale = Vector2(0.2, 0.2)
	modulate.a = 0.3
	print('grass was eaten!')

func is_grown():
	return is_fully_grown and amount >= max_amount
