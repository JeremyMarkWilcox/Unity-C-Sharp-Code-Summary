# Unity/ C# Code Summary On Live Project
I created an upgraded 2D version of Space Invaders with the Unity Engine, written in C#

## Introduction

In my final two courses, I delved into game development with Unity/C# and Unreal/C++. Each course culminated in a two-week live project, where I contributed an original game to a larger collaborative effort. My practical understanding of Agile/Scrum methodologies deepened through daily stand-ups and weekly code retrospectives, which bolstered accountability and fine-tuned our code management practices. Mastery of version control was essential due to the frequent pull requests and updates our team managed daily.

Designing my own video game was an invaluable opportunity to explore Unity's game engine intricacies and harness specific C# libraries for game functionality. Debugging and iterative development were constant companions as I expanded the game's features. Among the project's milestones, I am proud to have been the first student in the Bootcamp to implement a finite state machine successfully and the second to engineer a boss battle.

Enclosed are snippets from my code contributions, with the complete codebase accessible in the accompanying files.

## Play On My Personal Website

Here is a link to my portfolio where you can play the mini game in full:


## Game Scenes

## Enemy AI/ Finite State Machine

### Patroll State
The currentState variable plays a pivotal role in the enemy AI's decision-making process, acting as the switchboard between various behavioral states. By default, the AI begins in a Patrolling state upon the player's first encounter with it. This state represents the AI's routine behavior, where it navigates through predefined waypoints or wanders within a designated area.

As the game progresses and the player interacts with the AI, the currentState can transition to other states such as Chasing, Attacking, or Death based on the situation. For example, if the player comes within a certain proximity to the enemy, detected by the chaseTriggerDistance, the AI shifts from Patrolling to Chasing, signifying a change in behavior as it actively pursues the player.

This state-switching mechanism is facilitated by a series of conditional checks within the AI's update loop, with transitions triggered by specific game events or conditions being met. The default Patrolling state ensures that the enemy exhibits autonomous behavior, providing a dynamic and engaging experience from the outset of the encounter.

![PatrollingState](https://github.com/JeremyMarkWilcox/Unity-C-Sharp-Code-Summary/assets/150622088/c547acd4-f892-4031-8929-7baf623e33c2)

## Meteors

## Player Ship







