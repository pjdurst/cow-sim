def search_grid(grid, row, col):
    #grid - the grass field
    #row,col - the cow's x,y location inside the field
    
    # Find the cow's neighboring nodes
    neighbors = []
    for dr in [-1, 0, 1]:
        for dc in [-1, 0, 1]:
            if dr == 0 and dc == 0:
                continue  # Skip the current node
            r = row + dr
            c = col + dc
            if 0 <= r < len(grid) and 0 <= c < len(grid[0]):
                neighbors.append((r,c))
                #print('neighbors', neighbors)

    # Find the location of the highest neighboring node
    max_value = float('-inf')  # Initialize max_value with negative infinity
    max_location = None  # Initialize max_location as None
    
    for j in range(8):
        location = neighbors[j]
        x = location[0]
        y = location[1]
        if grid[x,y] > max_value:
            max_value = grid[x,y]
            max_location = [x,y]
    
    #print('max location = ', max_location)
    return max_location