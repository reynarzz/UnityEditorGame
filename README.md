A game that runs in the Unity editor, without using Unity's play mode at all.

## How does this work?
This is accomplished thanks to a custom engine created with C# using Unity's types, such as Texture and the old GUI system; no custom internal/native functions were created as part of my main goal.

## Features:
- A simple physics system to detect AABB collisions and ray casting.
- Uses the audio library "NAudio" to play SFX and music.
- Sprite Rendering uses the old GUI, just calling drawTexture, and simple math (Not optimal, but for this project, the goal was to not use internal graphics functions but 100% Unity's ones)
- Simple component system that resembles Unity3D's.
- Level editor to create, edit, and paint worlds in a tilemap system.


## How to play?
Open the game scene and click the "Game" game object. Ensure the inspector is visible!

![Gameplay Demo](gamepreview.gif)

Tested on Unity3D ver 2021.3.11f1. Please use this one for initial testing.
