extends Node2D

# Initialize tracking data
var tracking_data = []
var current_frame = 0

func _ready():
	pass

func _process(_delta):
	# Data recording rate
	current_frame += 1 # Set to 1 for this simple example
	
	# Data recording timestep, 60 frames for this simple example
	if current_frame % 60 == 0:
		record_timestep()

func record_timestep():
	var timestep_data = {
		"frame": current_frame,
		"time": Time.get_ticks_msec() / 1000.0,  # Time in seconds
		"cows": [],
		"total_grass": 0
	}
	
	# Track each Cow position
	var cows = get_tree().get_nodes_in_group("Cow")
	for cow in cows:
		timestep_data["cows"].append({
			"name": cow.name,
			"position": cow.position
		})
	
	# Track the total available grass amount in the scene
	var grass_nodes = get_tree().get_nodes_in_group("Grass")
	for grass in grass_nodes:
		timestep_data["total_grass"] += grass.amount
	
	tracking_data.append(timestep_data)

func _notification(what):
	if what == NOTIFICATION_WM_CLOSE_REQUEST:
		# Close the simulation and log data
		output_tracking_data()
		get_tree().quit()

func output_tracking_data():
	# Create and open a data tracking file
	print("\n========== GAME TRACKING DATA ==========")
	print("Total timesteps recorded: ", tracking_data.size())
	print("\nDetailed Data:")
	
	for data in tracking_data:
		print("\n--- Frame ", data["frame"], " (", data["time"], "s) ---")
		print("Cows:")
		for cow in data["cows"]:
			print("  ", cow["name"], ": ", cow["position"])
		print("Total grass remaining: ", data["total_grass"])
	
	print("\n========================================\n")
	
	# Save the data to an output file
	save_to_file()

func save_to_file():
	var file = FileAccess.open("user://game_data.txt", FileAccess.WRITE)
	if file:
		file.store_string("========== GAME TRACKING DATA ==========\n")
		file.store_string("Total timesteps: " + str(tracking_data.size()) + "\n\n")
		
		for data in tracking_data:
			file.store_string("\n--- Frame " + str(data["frame"]) + " (" + str(data["time"]) + "s) ---\n")
			file.store_string("Cows:\n")
			for cow in data["cows"]:
				file.store_string("  " + cow["name"] + ": " + str(cow["position"]) + "\n")
			file.store_string("Total grass: " + str(data["total_grass"]) + "\n")
		
		file.close()
		print("Data saved to: ", OS.get_user_data_dir(), "/game_data.txt")
