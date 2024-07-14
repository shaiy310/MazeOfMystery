import os
import random
import sys
import time


def get_boss_empty_maze():
    return [   
        ['#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'], 
        ['#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', 'X', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#'], 
        ['#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', ' ', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#'], 
        ['#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', ' ', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#'], 
        ['#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'], 
        ['#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#'], 
        ['#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'], 
        ['#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#'], 
        ['#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'], 
        ['#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#'], 
        ['#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'], 
        ['#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#'], 
        ['#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'], 
        ['#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#'], 
        ['#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'], 
        ['#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#'], 
        ['#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'], 
        ['#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#'], 
        ['#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'], 
        ['#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', ' ', ' ', 'S', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#'], 
        ['#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'],
    ]

def initialize_empty_maze(rows, cols):
    max_row, max_col = (2 * rows + 1, 2 * cols + 1)
    maze = []
    for r in range(max_row):
        row = []
        for c in range(max_col):
            if 0 < r < max_row - 1 and 0 < c < max_col - 1:
                row.append('#' if r % 2 == 0 or c % 2 == 0 else ' ')
            else:
                row.append('#')
        
        maze.append(row)
            
    # maze = [
        # [
            # ' ' if 0 < r < max_row - 2 and 0 < c < max_col - 2 else '#'
            # for c in range(max_col)
        # ] for r in range(max_row)
    # ]
    return maze

def is_valid_cell(maze, row, col):
    rows, cols = len(maze), len(maze[0])
    return 0 < row < rows - 1 and 0 < col < cols - 1
    
def get_unvisited_neighbors(maze, cell, visited):
    neighbors = []
    x, y = cell
    directions = [(2, 0), (-2, 0), (0, 2), (0, -2)]
    for dx, dy in directions:
        nx, ny = x + dx, y + dy
        if is_valid_cell(maze, nx, ny) and (nx, ny) not in visited:
            neighbors.append((nx, ny))
            
    random.shuffle(neighbors)
    return neighbors[0] if neighbors else None

def _get_unvisited_neighbors(maze, cell, visited, target_mark):
    neighbors = []
    x, y = cell
    directions = [(1, 0), (-1, 0), (0, 1), (0, -1)]
    for dx, dy in directions:
        nx, ny = x + dx, y + dy
        if is_valid_cell(maze, nx, ny) and (nx, ny) not in visited and maze[nx][ny] in {' ', target_mark}:
            yield (nx, ny)

def remove_wall_between_cells(maze, cell1, cell2):
    x1, y1 = cell1
    x2, y2 = cell2
    maze[(x1 + x2) // 2][(y1 + y2) // 2] = ' '

def randomly_select_start_point(maze):
    rows, cols = len(maze), len(maze[0])
    return random.choice([(i, 1) for i in range(1, rows - 1, 2)])

def randomly_select_end_point(maze, start_point):
    rows, cols = len(maze), len(maze[0])
    end_point = random.choice([(i, cols - 2) for i in range(1, rows - 1, 2) if maze[i][cols - 2] == ' '])
    while end_point == start_point:
        end_point = random.choice([(i, cols - 2) for i in range(1, rows - 1, 2) if maze[i][cols - 2] == ' '])
    
    return end_point

def generate_maze(rows, cols):
    maze = initialize_empty_maze(rows, cols)
    start_point = randomly_select_start_point(maze)
    end_point = randomly_select_end_point(maze, start_point)
    
    def dfs(current_cell, visited):
        visited.add(current_cell)

        while neighbor := get_unvisited_neighbors(maze, current_cell, visited):
            remove_wall_between_cells(maze, current_cell, neighbor)
            dfs(neighbor, visited)
    
    dfs(start_point, set([]))

    maze[start_point[0]][start_point[1]] = 'S'
    maze[end_point[0]][end_point[1]] = 'E'
    
    return maze, start_point, end_point
    
def generate_boss_maze():
    maze = get_boss_empty_maze()
    def dfs(current_cell, visited):
        visited.add(current_cell)

        while neighbor := get_unvisited_neighbors(maze, current_cell, visited):
            remove_wall_between_cells(maze, current_cell, neighbor)
            dfs(neighbor, visited)
    
    start_point = (19, 11)
    end_point = (1, 10)
    dfs(start_point, set([]))
    
    maze[start_point[0]][start_point[1]] = 'S'
    maze[end_point[0]][end_point[1]] = 'X'
    
    return maze
    
def generate_doors(maze, start, end, door_num):
    def dfs_search_path(current_cell, visited, target_mark, path):
        row, col = current_cell
        # Mark the current cell as visited
        visited.add(current_cell)

        # If we reach the end point, stop the search
        if maze[row][col] == target_mark:
            return True

        # Explore the neighboring cells
        for neighbor in _get_unvisited_neighbors(maze, current_cell, visited, target_mark):
            if dfs_search_path(neighbor, visited, target_mark, path):
                path.append(neighbor)
                return True
        
        return False
        
    def dfs_reachable_cells(current_cell, visited, target_mark, reachables):
        row, col = current_cell
        # Mark the current cell as visited
        visited.add(current_cell)

        # If we reach the end point, stop the search
        if maze[row][col] == target_mark:
            return
            
        reachables.append(current_cell)

        # Explore the neighboring cells
        for neighbor in _get_unvisited_neighbors(maze, current_cell, visited, target_mark):
            dfs_reachable_cells(neighbor, visited, target_mark, reachables)
        

    path = []
    dfs_search_path(start, set([]), 'E', path)
    door = random.randint(1, len(path) - 1)
    maze[path[door][0]][path[door][1]] = 'A'  
    # Door -> A
    # Key -> a
    # Hint -> 1
    
    reachables = []
    dfs_reachable_cells(start, set([]), 'A', reachables)
    key, hint = random.sample(reachables, 2)
    maze[key[0]][key[1]] = 'a'
    maze[hint[0]][hint[1]] = '1'
    
    # for i, (row, col) in enumerate(path[1:]):
        # maze[row][col] = f'{i % 10}'

def print_maze(maze):
    for row in maze:
        print(''.join(row))
        
    # for line in s.splitlines(): print("{'" + "', '".join(line) + "'},")

def save(maze, file_path):
    os.makedirs(os.path.dirname(file_path), exist_ok=True)
    
    with open(file_path, 'w') as f:
        f.write('\n'.join(''.join(row) for row in maze))

def main(rows, cols, file_path, is_boss_level):
    if is_boss_level == 'yes':
        maze = generate_boss_maze()
    else:
        # Generate and print the maze
        maze, start_point, end_point = generate_maze(rows, cols)
        generate_doors(maze, start_point, end_point, 2)
    print_maze(maze)
    file_path = file_path or r'\Assets\Levels\Maze.txt'
    save(maze, file_path)

if __name__ == '__main__':
    t = time.time()
    main(10, 10, *sys.argv[1:])
    print(time.time() - t)