# InvisibleShooter

## How to add a level
I've made adding a new level very simple in a recent commit. All you have to do now is follow these steps:
* Create a new Scene
* In the "PlayerSelectScene", select "MenuManager" and go to it's component "MenuManagerScript". In this component, go to the "Levels" attribute and expand it. There, increase the size by one and in the new place, insert the name of the scene you've just created
* In the new Scene you've created, insert the prefab "GameManager"

You're done! The level is now playable! I'll then suggest to move the camera in a top-down position, adding a background and walls (IMPORTANT! Check that the walls are at y_position = 0 and y_scale = 3, while the floor must be at y_position = -1 and y_scale = 1 AND IT SHOULD **NOT** have a box collider), and changing the positions of the player spawns (you can find them under GameManager). To get a similar style to the already existing level, I'd suggest to go there and copy the camera position
