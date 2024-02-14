# Changelog

All notable changes to this project will be documented to this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Builds]

## [0.1.0] - 2024-02-14

- Player - Players can move, attack and interact with the environment
- Enemy - Enemies can chase and attack players using functional AI
- Local Multiplayer - Up to 4 players can play together
- Quests - Players can complete quests to leave the level
- Environment - A complete environment with trees, rocks, and a carriage for two levels
- Audio - A complete audio system with music and sound effects
- Events - A complete event system for dialogue and game events
- Pickups - Players can pick up and use items along with gold

### Added

## [Internal]

## [0.0.17] - 2024-02-14

### Added

- CHANGELOG.md added
- Music triggers now added to scenes
- Restart system for debugging

### Changed

- Updated Playtest level
- Ranged kills now counts towards kill counter
- Intro sequence now plays correctly and switches to the next scene
- Levels updated to have correct objects
- Dialogue manager updated to have correct dialogue
- Characters weapons now save between scenes
- Characters are no longer stored in a list, but a dictionary
- Players can no longer share controllers when their character dies
- Characters can no longer spam animations

## [0.0.16] - 2024-02-13 

### Added

- Scenes for Playtest 1
- Player now has UI for displaying coins and kills
- There is now a popup for what players should do for objective
- Score Screen added to display after a level
- Ghost animations added and implemented
- Intro Sequence Scene

### Changed

- Character attacks now count as kills
- Rewritten dialogue managers

## [0.0.15] - 2024-02-12

### Added

- Shovel and Sword Texture
- Character now has animations
- Characters now have different material colors based on playerID
- Rocks added
- Different sounds now play depending on textures on the ground
- Sign mesh
- Gold pile mesh
- Cutout mesh for players
- Coin bag icon added for UI

### Changed

- Dirt texture updated
- Carriage hitboxes fixed
- Character configured to use new mesh and textures
- Character can now pickup weapons
- Witch now has different proportions
- Music now changes dynamically based on the internal game timer
- Fixed bugs regarding player movement
- Carriage textures fixed after having been broken

## [0.0.14] - 2024-02-11

### Changed

- Witch Textures updated

## [0.0.13] - 2024-02-10

### Changed

- Tree texture updated to not clash with grass texture
- Grass texture updated.

## [0.0.12] - 2024-02-09

### Added

- Button, Chatbox and Health Bar UI added
- 2D Sprite Editor Package added
- Enemy Ghost mesh added. Shader and Textures added.

### Changed

- Characters are now clamped within the camera. 
- Carriage now takes damage from enemies
- Carriage now has a health bar
- Characters now need to aim before being able to shoot
- Characters are removed from list of active players when dying
- Rumble added to controllers
- Singleton Script updated
- 
## [0.0.11] - 2024-02-08

### Added

- Internal document explaining what prefabs are required for the game-loop to work.
- Dialogue Audio script added to play sounds correctly
- Health UI Added
- Icons added for UI
- Character Health Bar added
- Grass texture added

### Changed

- Interaction popup now works correctly
- Quest objective progress no longer continues after player death
- Character Movement now works in FixedUpdate rather than Update to correctly use the physics engine and avoid stuttering.
- All players can now interact with stuff independently
- Camera bugfixes
- Carriage texture updated to more detail
- Carriage hitboxes fixed
- Dialogue Manager updated
- Dash has been reworked. Previously it never added a speed correctly.

## [0.0.10] - 2024-02-07

### Changed

- Fixed an issue where previously player should not have been able to collide with pickups but still could.
- Character can no longer attack when holding an item
- Character can now drop items
- Character now drops item on death
- Character can no longer move when interacting
- Fixed interact UI on death
- Enemies now spawn correctly after an issue where multiple spawners kept spawning incorrectly
- Carriage textures added
- Carriage mesh fixes
- Bugfixes with Github + Unity integration
- Gold now works correctly when picked up
- Camera now follows players correctly

## [0.0.9] - 2024-02-06

### Added

- Dynamic Enemy Spawner added
- Created turn towards camera script
- Added interaction UI
- Carriage mesh added
- Progress bar for interaction added

### Changed

- Character now loses item when it has been used for a quest objective
- Getting into carriage now only triggers on button press and not on button release
- Players can now leave if there are no required quests on the level to begin with if option is set
- Enemy audio updated
- Updated witch model fixed shading issue, improved topology
- Bugfixes for quests
- Character can now swap held item
- Character no longer collides with pickup
- Cameras can now render UI above everything else
- Updated when interaction UI is seen on carriage and quest objective

## [0.0.8] - 2024-02-05

### Added

- Interface for interacting added

### Changed

- Witch character model updated. Improved topology and redid uvs
- Environment models updated. Added new meshes for fir, spruce, log, stump along with textures.
- Movement and Dash for player completed
- Character can now interact 
- Timer bugfixes

## [0.0.7] - 2024-02-02

### Added

- Quest Manager added
- Final Witch character model added
- Internal timer added

### Changed

- Interface for taking damage bugfixes
- Character now takes damage and shows visual feedback
- Character now has audio for melee and ranged
- Enemy now has audio for its different states
- Camera now follows players correctly
- Dialogue Manager now displays choices
- Character controller now uses quest manager instead of old event manager
- Carriage for leaving the game changed
- Pickup has been rewritten to work properly with new quest manager
- Tree, log and stump meshes updated

## [0.0.6] - 2024-02-01

### Added

- Dash added to Player
- Interfaces added for things that need to take damage

### Changed

- Enemy now resets its path when changing state
- Enemies no longer get stuck in each other
- Enemies can now take damage
- Enemy can now have different speeds in different states
- Enemy can now attack player
- Characters now walk isometrically
- Character attack revised

## [0.0.5] - 2024-01-31

### Added

- Camera for following multiple players
- Added a Singleton Script

### Changed

- Enemy now listens even when chasing
- Updated GameManager for turning off inputs and pausing the game
- Character melee changed to not be able to destroy pickups

## [0.0.4] - 2024-01-30

### Added

- Local Multiplayer support
- Script for triggers created
- States for enemy
- Added the carriage to be able to win the game
- Pickup event
- Dialogue manager created for game events
- Enemy will now only turn towards sound if it actually heard something
- Attack behaviour for player
- Projectile for player to shoot
- AudioManager created

### Changed

- Enemy vision and hearing range now uses on trigger enter
- Enemy with changes such as previous note mentioned and refactoring
- Enemy controller updated to use NavMeshAgent properly
- Enemies "hear" players
- Enemy now chases player correctly
- FMOD Bugfixes
- Changed UnityEvent OnGoldPickup to Add PlayerID. GoldPickup gives gold once when a player interacts before it was multiple times
- Changed how PlayerID and currency is stored. Uses a list of Int which coressponds to an id Very fragile be wary, Each player has now an individual stash of gold


## [0.0.3] - 2024-01-29

### Added

- PlayerController and movement added
- Character mesh added
- Internal test scenes created
- Added initial player spawning
- Pickup script created
- Enemy added
- Utility Script for drawing lines with meshes added
- Ink Package added for dialogue

### Changed

- Bug fixing for FMOD. Re-imported package.

## [0.0.2] - 2024-01-25

### Added

- Test scenes added
- Folder structure added
- Added a default script template
- Added FMOD

## [0.0.1] - 2024-01-24

### Added

- Initial package and settings setup for Unity Project
