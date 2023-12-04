## Story System

A simple framework for creating branching storylines for Unity Games. Heavily relies on ScriptableObjects and offers interfaces for easy extendability.

### How does it work?

The framework revolves around Phases, Exits, Goals, Conditions and Flags.

#### Phases
Different "chapters" of the story. These could be different days, different scenes etc. There is always exactly 1 active *Phase*.

#### Exits
*Exits* are ways to leave a *Phase* and move on to another *Phase*. A *Phase* may have multiple *Exits*, or none if its the final *Phase*.

#### Goals
*Goals* are what the player must complete to activate an *Exit*. *Goals* have one or more *Conditions* that must be fulfilled. An *Exit* may have 1 or more *Goals*. A *Goal* may have prerequisites that need to be fulfilled before the goal can be progressed (in case you want to use *Goals* as sort of Quests and display in-progress ones). 

#### Conditions
*Conditions* answer the question "what needs to be done" for a *Goal* to be completed. By default, Conditions have 2 lists of *Flags* that either need to be True or False for the Condition to be met. Conditions can easily be extended to interact with other systems.

#### Flags
*Flags* are booleans that keep track of what has happened in the story, i.e. a character died or a place was visited.

### Installation for usage

Add the following line to your project manifest
  "com.aletoi.storysystem": "https://github.com/aletoivonen/com.aletoi.storysystem.git"

### Installation for contribution

Add this repo to a folder inside the Assets folder of a Unity project, i.e "Assets/StorySystem/"

### Example extension

More accurate examples coming.

#### StoryConditions

Make a class extending the *StoryCondition* class if you want custom conditions, for example if you have an inventory system and you want to require specific items to progress the story.
