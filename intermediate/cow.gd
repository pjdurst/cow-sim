extends CharacterBody2D

var speed = 50.0 # How fast the cows are moving
var grass_target = null 
var is_eating = false
var eat_timer = 0.0
var eat_duration = 2.0  # How long it takes to eat the grass

@onready var navigation_agent = NavigationAgent2D.new()

func _ready():
	print('hi, i am hungry cow!')
	
	# Set up NavigationAgent2D for pathfinding
	add_child(navigation_agent)
	navigation_agent.path_desired_distance = 4.0
	navigation_agent.target_desired_distance = 10.0
	
	# Wait for the navigation step to load
	await get_tree().physics_frame
	
	# Start searching for grass
	find_nearest_grass()

func _process(delta):
	if is_eating:
		# Currently eating grass
		eat_timer += delta
		if eat_timer >= eat_duration:
			# Finished eating
			finish_eating()
		return
	
	if navigation_agent.is_navigation_finished():
		# Start eating when the Cow reaches a target piece of Grass
		if grass_target and is_instance_valid(grass_target) and grass_target.is_grown():
			start_eating()
		else:
			# Once the current grass is gone, find new grass
			find_nearest_grass()
		return
	
	# Pathfinding with A*
	var next_position = navigation_agent.get_next_path_position()
	var direction = (next_position - global_position).normalized()
	
	# Move the Cow at each frame
	velocity = direction * speed
	move_and_slide()

func find_nearest_grass():
	# Find all grass nodes that are fully grown
	var grass_nodes = get_tree().get_nodes_in_group("Grass")
	
	# Filter out grass that is currently fully eaten
	var grown_grass = []
	for grass in grass_nodes:
		if grass.is_grown():
			grown_grass.append(grass)
	
	if grown_grass.is_empty():
		print("No grown grass available, waiting...")
		# If there is no available Grass, pause until Grass grows and continue searching
		await get_tree().create_timer(1.0).timeout
		find_nearest_grass()
		return
	
	# A* pathfinding
	var nearest_grass = null
	var min_distance = INF
	
	for grass in grown_grass:
		var distance = global_position.distance_to(grass.global_position)
		if distance < min_distance:
			min_distance = distance
			nearest_grass = grass
	
	if nearest_grass:
		navigation_agent.target_position = nearest_grass.global_position
		grass_target = nearest_grass

func start_eating():
	is_eating = true
	eat_timer = 0.0
	print("Nom nom nom... eating grass!")

func finish_eating():
	# Tell the grass to start regrowing
	if grass_target and is_instance_valid(grass_target):
		grass_target.eat_grass()
	
	grass_target = null
	is_eating = false
	print("Finished eating! Looking for more grass...")
	
	# Search for next grass
	find_nearest_grass()
