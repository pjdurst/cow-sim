import numpy as np



# Create a 100x100 field of grass
field = np.full((101,101), 5, dtype=int)

# Creat 5 cows placed randomly within the field
cows = np.random.randint(1, 100, size=(5,2))
print('starting cow positions: ', cows)

# Main model loop
for time in range(5):

    # Move cows to the highest neighboring grass node
    # If the current node is the highest, eat one grass
    for i in range(5):
        cow_X = cows[i,0]
        cow_Y = cows[i,1]
        # The cow looks for the tallest grass
        if field[cow_X-1,cow_Y] > field[cow_X,cow_Y]:
            cows[i,0] = cow_X-1
            cows[i,1] = cow_Y
        if field[cow_X+1,cow_Y] > field[cow_X,cow_Y]:
            cows[i,0] = cow_X+1
            cows[i,1] = cow_Y
        if field[cow_X,cow_Y-1] > field[cow_X,cow_Y]:
            cows[i,0] = cow_X
            cows[i,1] = cow_Y-1
        if field[cow_X,cow_Y+1] > field[cow_X,cow_Y]:
            cows[i,0] = cow_X
            cows[i,1] = cow_Y+1
        else:
            field[cow_X,cow_Y] -= 1

print('ending cow positions at time ', time, cows)
