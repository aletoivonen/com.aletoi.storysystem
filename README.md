## Story System

A simple framework for creating branching storylines for Unity Games. Heavily relies on ScriptableObjects and offers interfaces for easy extendability.

### Installation for usage

Add the following line to your project manifest
  "com.aletoi.storysystem": "https://github.com/aletoivonen/com.aletoi.storysystem.git"

### Installation for contribution

Add this repo to a folder inside the Assets folder of a Unity project, i.e "Assets/StorySystem/"

### Example extension

#### StoryConditions

Make a class extending the *StoryCondition* class if you want custom conditions, for example if you have an inventory system and you want to require specific items to progress the story.
