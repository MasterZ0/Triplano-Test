# Triplano Test

## Summary

This project is a replica of two levels of The Legend of Zelda: Ocarina of Time, developed in Unity. The player controls the character in 3rd person and can perform actions such as walking, crouching, carrying boxes, collecting coins, and controlling the camera. The objective of the game is to complete two levels. In the first level, the player must solve a puzzle by moving boxes to escape the room. In the second level, the player must pass through guards undetected, otherwise they will be sent back to the first level. If the player successfully passes through the second level undetected, they will be greeted with a victory screen.

Links:

- Test: https://docs.google.com/document/d/1yXnvXRF1gIWvYrJHC9842tk8v-cRhBZdg-7Me4P8a54/edit# 


- Download Build: https://drive.google.com/file/d/15pjh93y5KlZHvZeCx41oyqK6sW2oVSfj/view?usp=sharing
- Video: https://drive.google.com/file/d/1BV-ysWpMTnwIullQA48Nb6WUoDoNOQKc/view?usp=share_link

Stage 1

![stage 1](https://user-images.githubusercontent.com/64444068/212126851-6f0f75c6-71a0-4d0d-a8dc-bd862aafb65c.gif)

Stage 2

![stage 2](https://user-images.githubusercontent.com/64444068/212129247-00f84b55-2821-46b8-a83b-3b0a52f11435.gif)

## Inputs

To control the game, Unity's new Input System was used. The commands are:

* Player

WASD - move
Space - Jump
Ctrl - Crouch
E or Left Mouse Button - Interact with the boxes
Move the mouse - Move the camera (First Level Only)
I - Invisible (God Mode)

* UI

Esc - Pause and Cancel
WASD - Navigate
Enter - Submit

## Environment

The environment was built using ProBuilder and Unity's built-in basic 3D objects. The character, box, coins and sounds were included in the test files.

## UI

There are 5 canvases present in the game

- Pause: In the gameplay scene
- HUD: In the player's prefab
- Victory Screen: In the victory scene
- Dialog box: In the gameplay scene
- Scene transition: In the application manager scene

## Editor Windows

The "Triplano Test" menu can be viewed on the taskbar, where it is possible to open the windows.

* Game Design: Used set game design values
* Development Tools: Used to help center transforms

## Level System

To create new levels you must create a prefab variant of the Prefab "Room Template". 

After creating a room you must add "Room Connections" referring to the connection of next rooms. 

Each connection has its own name reference, the player's spawn point, the trigger to access new scenes, the scene and connection the player will be sent after being triggered.

## Scripts

* AI: Guard and vision detector logic
* ApplicationManager: Control application and scene transition
* Audio: Adapter to manages all audio and active instances
* Data: All scriptable objects for design fit
* Editor: Used to build game editor screens
* Gameplay: All gameplay features, with coins, crates, room management, etc...
* Inputs: Adapter to facilitate the use of inputs
* ObjectPooling: Prefab resource management
* Persistence: Used to save the state of crates and coins
* Player: All player control architecture logic and your states
* Shared: Extension methods, Utils, Project Paths and others...
* StateMachine: State machine logic used in the player
* UI: Generic UI Objects

## Conclusion

Overall, this test was a great opportunity to showcase my abilities in game development using Unity. I am proud of the result I was able to achieve and believe that I have demonstrated a strong understanding of the necessary skills and techniques. 
