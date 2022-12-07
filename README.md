# Otherside by Spooky Squad 
<img width="720" alt="image" src="https://user-images.githubusercontent.com/53448490/206074833-ed8180aa-d5a3-4b63-bf4f-cec6235c0be6.png">

Made in Unity `2021.3.10f1` for Emory University CS370: Computer Practicum Fall 2022

Itch.io link: [https://sacredstudios.itch.io/otherside](https://sacredstudios.itch.io/otherside)

### The Spooky Squad

* Product Owner: Evan Del Carmen [@ebdelca](https://github.com/ebdelca)
* Scrum Master (and owner of this repo): Samantha Lin [@Seraclin](https://github.com/Seraclin)
* Team Members: (Clown) Jessie Coleman [@SacredStudios](https://github.com/SacredStudios), James Song [@Sirious1y](https://github.com/Sirious1y), Miguel Simbahan [@Minuslight](https://github.com/Minuslight), Steve Wang [@steve-z-wang](https://github.com/steve-z-wang)


### Credits
* Ghost || Pixel Asset Pack: [https://pop-shop-packs.itch.io/ghost-pixel-asset-pack](https://pop-shop-packs.itch.io/ghost-pixel-asset-pack)
* Pixel_Poem Dungeon Tileset: [https://pixel-poem.itch.io/dungeon-assetpuck](https://pixel-poem.itch.io/dungeon-assetpuck)
* 0x72_DungeonTilesetII: [https://0x72.itch.io/dungeontileset-ii](https://0x72.itch.io/dungeontileset-ii)
* Monster Sounds: [https://pudretediablo.itch.io/16bits-monsters-growls](https://pudretediablo.itch.io/16bits-monsters-growls) 
* Background Music: [https://ryanavx.itch.io/final-quest-music-pack](https://ryanavx.itch.io/final-quest-music-pack) 
* Game Menu Background: [https://www.freepik.com/free-vector/medieval-castle-gate-night-palace-entry-exterior-with-arched-door-burning-torches-fortress-tower-architecture-fairytale-dungeon-building-facade-stone-brick-wall-cartoon-vector-illustration_21957272.htm](https://www.freepik.com/free-vector/medieval-castle-gate-night-palace-entry-exterior-with-arched-door-burning-torches-fortress-tower-architecture-fairytale-dungeon-building-facade-stone-brick-wall-cartoon-vector-illustration_21957272.htm) 


## Why Does This Exist?

This is a semester-long project for CS370: Computer Science Practicum Fall 2022 at Emory University, aiming to practice Scrum methodology and make a team project that both provides experience and teamwork. ***Otherside*** is an online, multiplayer, co-op, rogue-like pixel game about a ghost with the unique ability of possessing enemies. Like many games out there, it aims to give enjoyment, relaxation and to offer you and your friends a fun time.


## Organization

The Unity project was made in version `2021.3.10f1` in C#. Photon and ParrelSync are used online. In the `cs370/Game/Otherside/Assets` folder, are subfolders for sprites, tilesets, sounds, scripts, scenes, etc. named accordingly. Unity has files called “scene” files which store all the data for each level. Assets can be dragged into the scene in the form of `GameObjects`. These `GameObjects` can have a number of components attached to them that further modify their behavior such as scripts, colliders, animators. In the Resources folder, we save players, enemies, items, and abilities as `prefabs`: custom `GameObjects` that can be reused. For example, our `Player` prefab holds many script components for health, interacting, controls/movement, possession, and abilities, various colliders, UI elements, an animator, and audio listener. All prefabs must be stored in `Assets/Resources` and cannot be located in any subfolders. 

Refer to the `Zombie` prefab for creating an enemy. Enemies must follow a certain structure: 
* They must override the `enemy_base_anim` animator controller with their own `Animator Override Controller`. They must also have an idle and run animation.
* Must have two `AbilityHolder` scripts (for primary and secondary abilities) that inherit from the ‘Ability’ ScriptableObject
* Those two `AbilityHolder` scripts must be referenced by the `AbilityArray` script (order matters)

Ability information is inherited from the `Ability` abstract class and is organized in the form of `ScriptableObjects` (attribute containers). For example, the `RangedAbility` inherits attributes/methods from `Ability` and defines other custom attributes (e.g. bullet speed). These `ScriptableObjects` can be dragged onto any `AbilityHolder` script to change that GameObject’s ability.

Sprites are designed to be displayed in a 16x16 pixel format.


## Setup

To play on itch.io, you can click the [link](https://sacredstudios.itch.io/otherside) above. There are two menu options: ‘Start Game’ and ‘Tutorial’. The ‘Tutorial’ button will lead you to the tutorial level which is a single player level designed to familiarize you with the controls of the game. 

The ‘Start Game’ will prompt you to enter a Nickname, which must be 3-10 characters, or else you will not be able to proceed. The start button will be highlighted once there are 3-10 characters in the text box. Click on the start button to proceed. 

Another screen will appear with buttons “Join Random Game” and “Create/Join Room”. Under the “Create/Join Room” button, there is an empty text box for inputting a password which is used for joining or creating a room. Other players can join the same room by inputting the same password, and then clicking the “Create/Join Room”. The “Join Random Game” button will find and join a random room regardless of password. 

After room creation and/or joining, the lobby screen will display. This screen will show all the players currently connected to the lobby. Clicking start will begin the game with all players in the lobby

For the Unity project folder itself, you can open it (`cs370/Game/Otherside`) via Unity Hub with editor version `2021.3.10f1` (other editor versions are not guaranteed to work). After loading the project, open the starting scene located at `Assets/Scenes/StartingScene` and press the play button to run the scene. To export the project, we used the WebGL build from Unity and exported a zip file to itch.io.



## Game Instructions

Navigate through a dungeon, defeating and possessing enemies along the way, while finding a way to escape. If your health reaches 0, you respawn back at the beginning of the level. 

Possess an enemy to heal yourself and temporarily gain their powerful abilities. 

#### Controls 

W/A/S/D or arrow keys: to move up/left/down/right

Combat

Use your mouse to aim.

Left Click: Primary Ability

Right Click: Secondary Ability (if not passive)

Space bar: Possess a nearby “dead” enemy (indicated with sparkles); To unpossess, press space again (each enemy corpse can only be possessed once)

E: Interact (e.g. open doors/chests, pickup keys/health potions, unlock doors)

The player is spawned as a ghost by default with a “Will-o-Wisp” primary ability and “Phase” secondary ability. 

Default Abilities:

Will-o-Wisp: shoots out a glowing projectile with low damage
Phase: Become invisible to enemies (can still take damage)

### Enemies

Chort: A demon with “Bite” and “Vamp”
* Bite: a powerful melee attack
* Vamp: heals when dealing damage

Wogol: A demon with “Fireball” and “Full Out”

Specter: A flying skull with “Fireball” and “Full Out”

Specter (Elite): A more powerful Specter with advanced “Fireball” and “Full Out”
* Fireball: shoots out a fireball
* Full Out: reduce Primary’s cooldown for a duration

Ice Zombie: A freezed zombie with “Iceball” and “Slow”
* Iceball: shoots out an iceball
* Slow (Passive): targets hit by abilities will be slowed for a duration. 

Zombie: A zombie with “Poison Cloud” and “Poison”

Swampy: a green slime with “Poison Cloud” and “Poison”, brother of Muddy

Muddy: a brown slime with “Poison Cloud” and “Poison”, brother of Swampy
* Poison Cloud: spit out a poison cloud that deals damage over time to targets in the cloud
* Poison (Passive): targets hit by Poison Cloud will become “Poisoned” and lose some health over time.

Necromancer: A dark magician with “Voidball” and “Analyze”
* Voidball: shoots out a bullet full of void energy
* Analyze (Passive): targets hit by abilities will take more damage for a duration

Boss:
Skeleton King: The king of specters
* He can emits his energy in 3 directions, hurting anything that touches it
* He can summon his army of specters around him
