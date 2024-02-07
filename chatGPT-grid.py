def get_neighbors(grid, row, col):
    neighbors = []
    for dr in [-1, 0, 1]:
        for dc in [-1, 0, 1]:
            if dr == 0 and dc == 0:
                continue  # Skip the current node
            r = row + dr
            c = col + dc
            if 0 <= r < len(grid) and 0 <= c < len(grid[0]):
                neighbors.append((r, c))
    return neighbors

# Example usage
grid = [[1, 2, 3],
        [4, 5, 6],
        [7, 8, 9]]

row = 1
col = 1
print(get_neighbors(grid, row, col))

def find_max_location(grid):
    max_value = float('-inf')  # Initialize max_value with negative infinity
    max_location = None  # Initialize max_location as None
    
    for row_idx, row in enumerate(grid):
        for col_idx, value in enumerate(row):
            if value > max_value:
                max_value = value
                max_location = (row_idx, col_idx)
    
    return max_location

# Example usage
grid = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
]

print(find_max_location(grid))  # Output: (2, 2)

    # Calculate the cow's new grid location
    deltaX = 0
    deltaY = 0
    if row_idx == 0:
        deltaY = 1
    if row_idx == 2:
        deltaY = -1
    if col_idx == 0:
        deltaX = -1            
    if col_idx == 2:
        deltaX = 1
