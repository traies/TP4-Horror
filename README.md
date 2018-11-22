# DEAD INSIDE: documentation
## Objective
In the game you will have to find the exit to all the levels and tying to survive to all zombies. But be careful, you have limited batteries for your flashlight and limited bullets!

## Controls
- **Movements**: WASD or Arrow keys
- **Aim**: Right Mouse Click
- **Shooot**: Left Mouse Click
- **Change weapon**: Q
- **Reload**: R
- **Use health pack**: V
- **Toggle flashlight**: F
- **Change flashlight batteries**: T
- **Action (pick up object, open door)**: E
- **Run**: Shift

## Generate Levels
1 - Open `Assets/Scenes/GenerationScene.unity`
2 - Select te `GameManager` game object and inside the settings of its script you will find the option `Generate Level`. This option will generate the map (without enemies or items) based in the values in `LevelGenerator` (Nb of rooms, room spawn rate,  map side dimension, decrease rate). When the game starts, it will generate enemies and items in their respective generation points and based on their values found in `EnemyGenerator` and `ItemGenerator`.
3 - Save the generated scene.
4 - Go to `Window/AI/Navigation` and bake with agent height equals to 1. This will calculate the walkable map for the enemies.
5 (Optional) - To run the level standalone you will have to add the script `Assets/Scripts/CrossScenesData.cs` somewhere inside the scene.
