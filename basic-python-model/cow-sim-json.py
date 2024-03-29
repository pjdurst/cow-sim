# Standard imports
import json
import numpy as np

# Custom functions
import search_grid as search

# Read the simulation definition .json file and store the model parameters
filePath = 'input.json'
simulationDefinitions = open(filePath)
simulationData = json.load(simulationDefinitions)
fieldSize = simulationData['field-size']
grassHeight = simulationData['grass-height']
growthRate = simulationData['growth-rate']
noCows = simulationData['number-cows']
searchDistance = simulationData['search-nodes']
eatRate = simulationData['eat-rate']
steps = simulationData['sim-time']

# Create the field and cow arrays
field = np.full((fieldSize,fieldSize), grassHeight)
cows = np.random.randint(1, fieldSize, size=(noCows,2))
print('initial cow positions:\n', cows)

for t in range(steps):
    
    for i in range(noCows):
        # Resolve cow movements
        cow_X = cows[i,0]
        cow_Y = cows[i,1]
        #print('current cow position: ', cow_X, cow_Y)
        [cowX_prime, cowY_prime] = search.search_grid(field,cow_X,cow_Y)
        if cow_X - cowX_prime == 0 and cow_Y - cowY_prime == 0: 
            field[cowX_prime,cowY_prime] -= eatRate
            print('new grass height at ', cowX_prime, cowY_prime, '= ', field[cowX_prime,cowY_prime])
        cows[i,0] = cowX_prime
        cows[i,1] = cowY_prime
        
    print('new cow positions:\n', cows)

    # Regrow grass
    for i in range(fieldSize):
        for j in range(fieldSize):
            if field[i,j] < 5:
                field[i,j] += growthRate
    
