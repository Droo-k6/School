**cscd 371 final project: Basic Missile Command**

C#/.NET

---

The .exe should work as a standalone, worked when tested - used embedded resources.
Recommend using the release version as the cursor updates are smoother.

The help button/menu option in the program descibes how to use the program.

Sound for the explosion from
http://soundbible.com/1234-Bomb.html


---
**Shortcomings**

A lot of shortcomings, mainly from a bad setup and then work arounds.
The MainWindow class just serves to link the GUI and the GameLogic class, which was initially built to handle all logic on the game end and then have GameDraw draw what it has.

This later proved too restrictive and I made a GameState/State class to hold the games state for GameLogic
and GameDraw to use, but eventually switched to have it advance the gamestate/levels.
This worked out better but caused a lot of bad workarounds to be implemented such as having an event call
to add/remove shapes/game objects to the GameDraw class, really bad - should've just had a reference to the 
canvas in the GameState class.
There is also alot of areas that are not very event driven, relying on enum values to determine what to do
- since they are acting as a refresh method.
A lot could be cleaned up, MainWindow could still be seperate from GameLogic, then just have GameState
have complete access to GameLogic/GameDraw so it can set what its need while still allowing GameLogic/GameDraw
to do what it needs.
GameDraw is a bit weird, it updates shape positions and creates a lot of the display shapes - but is really not
needed as the primary Draw() call doesn't do alot. This could be replaced entirely.
GameDraw could be more useful to simulate the original games graphics with the pixel redrawing observed
from missile trails/explosion expansion/contracting. This could be done by filling the Canvas with a bunch of 
game rectangles to act as pixels.
As a whole, switching to a more general procedural setup (or rather, having complete access between
GameLogic, GameDraw and GameState/ a more global setup) would work a lot better 
- my approach resulted in a giant mess.

For game mechanics, no cooldown on missile firing. The balancing of incoming missile speed/rate is definately 
off. End of level is drawn out by waiting on missiles flying as no speedup is implemented for the last missiles
that do nothing. Spawning of missiles is also wonky.
There is also no diversity of missile types, initially wanted to do the missile splitting and smart missiles.
Do to time constraints I decided not to go with smart missiles, but then didnt fully implement the splitting 
despite having implemented the values for it.

The friendly missile batteries, used a small icon as a placeholder - didn't end up making it place each individual missile shape.

The colors, the GameSettings/GameDraw is all setup to allow swapping up of the color paleete, 
but I never got around to fully implementing it. Incoming missiles also do not have a strobe effect on the heads.

For the shapes of cities/batteries/terrain/etc. They are generated manually, but I did work out a way to recolor supplied 
images for the icon - so I could've applied this to other shapes and recolored as needed, but I didn't implement it for anything
besides the cursor icon.