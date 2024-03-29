import numpy as np

# Create a 100x100 array of integers to represent the grass field
field = np.full((101, 101), 5, dtype=int)
# Create a 5x5 array of cow locations
cows = np.random.randint(1, 100, size=(5, 2))
print('cow position before sim:')
print(cows)

# Create main simulation loop
for time in range(5):
    # Move the cows to find the tallest neighboring patch of grass
    for i in range(5):
        cow_x_position = cows[i,0]
        cow_y_position = cows[i,1]
        # Search for the tallest grass at each cow's location
        if field[cow_x_position-1, cow_y_position] > field[cow_x_position, cow_y_position]:
            cows[i,0] = cow_x_position-1
            cows[i,1] = cow_y_position
        if field[cow_x_position+1, cow_y_position] > field[cow_x_position, cow_y_position]:
            cows[i,0] = cow_x_position+1 
            cows[i,1] = cow_y_position
        if field[cow_x_position, cow_y_position-1] > field[cow_x_position, cow_y_position]:
            cows[i,0] = cow_x_position 
            cows[i,1] = cow_y_position-1    
        if field[cow_x_position, cow_y_position+1] > field[cow_x_position, cow_y_position]:
            cows[i,0] = cow_x_position 
            cows[i,1] = cow_y_position+1 
        else:
            field[cow_x_position, cow_y_position] -= 1
            print('field height at position ', cow_x_position, cow_y_position, '= ', field[cow_x_position, cow_y_position])            
        
    print('cow position after sim step: ', time)
    print(cows)    



        
        





