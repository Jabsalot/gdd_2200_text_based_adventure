Dialogue System branches three sections:
    1. NPC          - Dialogue that allows conversation
    2. Environment  - Dialogue that allows actions 

Example:

    nodeID: "ENV_OLD_WELL"
    speakerName: "The Old Well"  (or "" for narrator style)
    dialogueText: "A cricky side of the bend with many in which that covet your wears. You notice a Old Crickty Well..."
    choices:
    ├─ choiceText: "Move closer to the well"
    │  nextNodeID: "ENV_OLD_WELL_CLOSER"
    │
    ├─ choiceText: "Talk to Old Man Rick"
    │  nextNodeID: "NPC_OLDMANRICK_INTRO"
    │  requiredFlags: ["OLD_MAN_RICK_PRESENT"]  (optional, if he's not always there)

    -----------------------------------------------------------------------------------------------------------------------------

    nodeID: "ENV_OLD_WELL_CLOSER"
    speakerName: "The Old Well"
    dialogueText: "You move closer to the old well and notice a small black cat very similar to the cat Old Martha described"
    choices:
    ├─ choiceText: "Pick up the cat"
    │  nextNodeID: "ENV_OLD_WELL_CAT_PICKUP"
    │  grantFlags: ["HAS_BLACK_CAT", "QUEST_PROGRESS_MARTHA_CAT"]
    │
    ├─ choiceText: "Kill the cat"
    │  nextNodeID: "ENV_OLD_WELL_CAT_KILLED"
    │  grantFlags: ["CAT_KILLED"]
    │  forbiddenFlags: ["PACIFIST_RUN"]  (optional)
    │
    ├─ choiceText: "Leave the cat"
    │  nextNodeID: "ENV_OLD_WELL_CAT_LEFT"
    │
    └─ choiceText: "Step away from the well"
        nextNodeID: "ENV_OLD_WELL"  ← loops back to entry

    -----------------------------------------------------------------------------------------------------------------------------

    nodeID: "NPC_OLDMANRICK_INTRO"
    speakerName: "Old Man Rick"
    dialogueText: "What do you want child you shouldn't be around these parts..."
    choices:
    ├─ choiceText: "Just rummaging around"
    │  nextNodeID: "NPC_OLDMANRICK_RUMMAGING"
    │
    ├─ choiceText: "Looking for a black cat. Have you seen it?"
    │  nextNodeID: "NPC_OLDMANRICK_CAT_CLUE"
    │  grantFlags: ["ASKED_RICK_ABOUT_CAT"]
    │
    └─ choiceText: "Goodbye"
        nextNodeID: "ENV_OLD_WELL"  ← returns to environment!