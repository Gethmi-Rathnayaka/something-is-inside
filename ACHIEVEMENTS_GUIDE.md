# Achievements Guide - Something Is Inside

Complete guide to unlock all achievements and reach different endings.

---

## ENDINGS (Mutually Exclusive)

### 🟢 Good Ending

**Keep all dolls stable until Day 10**

**How to achieve:**

- Maintain **all dolls' mood above 50%** throughout the game
- Clean Elizabeth's blood immediately on Day 8 (do not ignore)
- Do not remove Marie's ribbon
- Comfort Oliver when his mood drops (Day 2+)
- Keep corruption levels low by caring for the dolls
- Reach Day 10 without triggering any bad ending conditions

**Key choices:**

- Always comfort sad dolls
- Clean blood immediately
- Keep ribbon on Marie
- Show care and concern for all dolls

---

### 🟡 Neutral Ending

**Reach Day 10 without a bad ending**

**How to achieve:**

- Reach Day 10 without unlocking any of the bad endings below
- You can have some minor neglect or events, but avoid the critical triggers
- This happens if you don't fully commit to the good ending but avoid disaster

**Key choices:**

- Don't let multiple dolls reach high corruption
- Don't completely ignore any doll for extended periods
- Clean Elizabeth's blood eventually
- Keep ribbon on Marie

---

### 🔴 Bad Ending - Ribbon Removed

**Remove Marie's ribbon before Day 10**

**How to achieve:**

- On Day 5 or later, choose to **remove Marie's ribbon**
- This triggers immediately and locks you into the bad ending path
- Marie becomes corrupted as the ribbon falls off

**Key choices:**

- On Day 5, when prompted about Marie's ribbon, choose to remove it
- Alternatively, let Marie's corruption climb to 70%+ and the ribbon wraps around her involuntarily

---

### 🔴 Bad Ending - Blood Neglect

**Do not clean Elizabeth's blood (ignore for 2+ days)**

**How to achieve:**

- Day 8: Elizabeth's blood spills
- **Do not clean the blood** for 2 consecutive days
- On the 2nd day of ignoring it, Elizabeth triggers a nightmare and the bad ending

**Key choices:**

- Day 8: Ignore the blood notification
- Day 9: Ignore the blood again
- Triggers nightmare and locks bad ending

---

### 🔴 Bad Ending - Neglected Oliver

**Ignore Oliver's comfort needs for 3+ days**

**How to achieve:**

- Oliver's mood must drop below 50% on 3 separate days
- Don't comfort him when his mood is low
- This represents complete emotional abandonment

**Key choices:**

- Day 2: Let Oliver's mood fall below 50, don't comfort him
- Day 3-5: Keep ignoring Oliver when his mood drops
- On the 3rd occurrence, triggers bad ending

---

### 🔴 Bad Ending - High Corruption

**Let 2+ dolls reach 70% corruption**

**How to achieve:**

- Allow corruption to spread to at least 2 dolls
- Each doll must reach 70% or higher corruption independently
- Caused by neglect and negative interactions

**Key choices:**

- Ignore dolls' needs consistently
- Don't clean Elizabeth's blood (increases her corruption)
- Don't keep Marie's ribbon (increases her corruption)
- Don't comfort Oliver when sad
- Let negative events trigger without intervention

---

## SPECIAL EVENTS & ACHIEVEMENTS

### 😢 Sorrow

**Make Oliver's mood drop below 50%**

**How to achieve:**

- Ignore Oliver on Day 2 or later
- His mood will naturally decline
- Simply don't interact with him positively

**Triggers naturally** by not comforting Oliver when sad.

---

### 😱 Distorted

**Let Elizabeth's mood drop below 50%**

**How to achieve:**

- Ignore Elizabeth on Day 3 or later
- Her mood will decline
- Don't interact with her positively
- Once mood drops below 50%, her face becomes distorted

**Key choices:**

- Avoid comforting Elizabeth
- Ignore her concerns
- Let her corruption/sadness grow

---

### 🔍 Discovery

**Inspect Marie's ribbon**

**How to achieve:**

- On Day 5, when Marie's ribbon becomes a focus point
- Choose to **inspect or examine the ribbon** closely
- This is usually a dialogue choice

**Triggers naturally** by examining Marie on Day 5.

---

### 🌀 Corruption Spreads

**Ribbon wraps around Marie's body**

**How to achieve:**

- Let Marie's corruption reach **70% or higher**
- On Day 5 or later, her corruption becomes so severe the ribbon wraps around her
- This can also be triggered by removing the ribbon when corruption is high

**Key choices:**

- Ignore Marie's needs
- Don't comfort her
- Let her corruption climb past 70%

---

### 👻 Unnatural

**Dolls switch places on their own**

**How to achieve:**

- Let **2+ dolls reach 50%+ corruption**
- On Day 6, if conditions are met, dolls will move positions on their own
- This indicates supernatural corruption spreading

**Key choices:**

- Neglect multiple dolls
- Let corruption spread across the group
- Don't maintain stable environment

---

### 🩸 Accident

**Elizabeth's blood spills**

**How to achieve:**

- Reach Day 8
- Elizabeth's blood event triggers automatically
- Cannot be prevented, only managed

**Triggers automatically** on Day 8.

---

### 🩸 Neglect

**Ignore the blood for 2 days**

**How to achieve:**

- Day 8: Elizabeth's blood spills
- Choose to **ignore the blood** notification
- Day 9: Ignore it again
- On the 2nd day of ignoring, triggers Elizabeth's nightmare

**Key choices:**

- Day 8: When prompted about blood, choose to ignore
- Day 9: Ignore again
- Leads to bad ending

---

### ⚫ Chaos

**Average corruption exceeds 50%**

**How to achieve:**

- Calculate the average corruption across **all dolls**
- Keep neglecting them until the average reaches 50%+
- By Day 9, if conditions are met, this achievement triggers

**Key choices:**

- Ignore all dolls equally
- Don't comfort anyone
- Let corruption spread naturally
- By Day 9, average should be 50%+

---

## SPRITE TRANSFORMATIONS & VISUAL CHANGES

⚠️ **CRITICAL: Sprites are ONLY checked at specific day morning events. You must meet conditions BY THAT DAY'S START, not during play.**

### 👦 Oliver's Sprites

#### Wet Sprite
**EXACT trigger: Day 2 morning check**
- Game checks: `if (oliver.state.mood < 50)` when Day 2 starts
- Sprite is ONLY set this one time on Day 2
- Visual: Oliver has wet patches from crying

**How to guarantee it:**
1. Day 1: Ignore Oliver multiple times (each ignore = -15 mood)
2. His default mood is ~50, so ignore him 3-4 times
3. Day 1 night: Mood must be < 50 when day ends
4. Day 2 morning: Game checks → Wet sprite appears ✓

**Why it might not show:**
- You comforted Oliver even once (mood recovers)
- You didn't neglect him enough by end of Day 1
- His mood stayed at 50 or higher

---

### 👧 Elizabeth's Sprites

#### Distorted Face Sprite  
**EXACT trigger: Day 3 morning check**
- Game checks: `if (eli.state.mood < 50)` when Day 3 starts  
- Sprite is ONLY set this one time on Day 3
- Visual: Elizabeth's smile looks unnaturally wide/distorted
- In the current code, Elizabeth can also become distorted earlier from neglect being tracked overnight

**How to guarantee it:**
1. Day 1: Do not clean, brush, gift, or comfort Elizabeth
2. Use all 3 interaction slots on Oliver or Marie instead
3. End Day 1: Elizabeth stays neglected, so her special visual state can already turn on overnight
4. Day 2: Still do not interact with Elizabeth at all
5. End Day 2: Her corruption/mood penalties stack again
6. Day 3 morning: Elizabeth is now guaranteed to be in the distorted state if she was left alone both days ✓

**Why it might not show:**
- You cleaned or brushed her even once (resets mood debt)
- You reached Day 3 with her mood still at 50 or above
- You comforted her

**Key mechanic:** Even one care interaction can save her from < 50 mood!

#### Corrupted Sprite
**EXACT trigger: corruption above 60**
- The generic corruption sprite appears when Elizabeth's corruption is over 60
- However, the distorted-face flag has higher visual priority, so if she is also distorted, that sprite will hide the generic corruption art

**How to get the corrupted look as close as possible:**
1. Leave Elizabeth alone on Day 1 and Day 2
2. Let her corruption rise naturally from neglect
3. By Day 3, corruption will be above 60, but the distorted-face sprite will usually still take priority

**Important limitation:**
- In the current sprite priority order, distorted face overrides the generic corruption sprite
- That means the cleanest way to see the corrupted sprite art is to avoid leaving her in a distorted state first, then push corruption above 60 afterward

#### Long Hair Sprite
**EXACT trigger: Automatic on Day 7**
- Day 7 morning: Elizabeth grows longer hair automatically
- Happens regardless of mood/corruption
- No player action needed

---

### 👗 Marie's Sprites

#### Ribbon Wrapped Sprite  
**EXACT trigger: Day 5 specific choice + corruption > 70**
- Day 5 event: Ribbon inspection happens
- Player chooses: **"Leave it alone"** (not Remove, not Not today)
- Condition: Marie's corruption must be > 70 at that moment
- If both true: Ribbon wraps, sprite changes

**How to guarantee it:**
1. Days 1-4: Ignore Marie constantly
2. Each interaction has ~+2 corruption or more
3. Never comfort her, don't clean her
4. Day 5 morning: Check her corruption stat
5. If > 70%: When ribbon prompt appears, choose "Leave it alone"
6. Ribbon wraps around her ✓

**Why it might not show:**
- Marie's corruption never reached above 70%
- You chose "Remove" instead of "Leave it alone"
- You comforted/cleaned her during Days 1-4

---

## SPRITE CHANGE TIMING TABLE

| Doll | Sprite | Checked When | Condition | Removable |
|---|---|---|---|---|
| Oliver | Wet | Day 2 START | mood < 50 | Yes (comfort removes) |
| Elizabeth | Distorted | Day 3 START | mood < 50 | Yes (clean/brush removes) |
| Elizabeth | Long Hair | Day 7 START | Auto | No |
| Marie | Ribbon | Day 5 choice | corruption > 70 + "Leave it alone" | No |

---

## STEP-BY-STEP: SEE ALL SPRITES

**If you want Elizabeth distorted fast:**
1. Day 1: Spend all three interactions on Oliver and/or Marie
2. Do not click Elizabeth at all
3. Day 2 morning: Elizabeth is already on the distorted path

**If you want the generic corruption sprite as well:**
1. Keep neglecting Elizabeth on Day 1 and Day 2
2. Then clean/brush her once to clear the distorted-face priority
3. Let corruption stay above 60 so the generic corruption sprite can show

**If you want the long-hair sprite too:**
1. Reach Day 7 without resetting Elizabeth’s visual flags
2. The long-hair state appears automatically

**Marie ribbon wrap path:**
1. Ignore Marie every day
2. Reach Day 5 with corruption above 70
3. Choose "Leave it alone" when the ribbon event appears
4. Ribbon wraps ✓

---

## TROUBLESHOOTING

**Oliver not wet on Day 2?**
- Need: Ignore him enough to get mood < 50 by end of Day 1
- Fix: Ignore him 4-5 times in Days 1-2 before Day 2 morning

**Elizabeth not distorted on Day 3?**
- Need: mood < 50 by end of Day 2
- Fix: Ignore her 4-5 times, don't clean/brush even once
- Each ignore = -15 mood starting from 50

**Marie's ribbon not wrapping Day 5?**
- Need: corruption > 70 AND choose "Leave it alone"
- Fix: Ignore her every day Days 1-4, check corruption stat before Day 5

**Sprites vanished mid-game?**
- Distorted removes if: You clean or brush Elizabeth
- Wet removes if: You comfort Oliver
- Long hair/wrapped: Permanent, can't remove

## QUICK REFERENCE: CHOICE PATHS

### For GOOD Ending + All Positive Events:

1. Always comfort sad dolls
2. Clean blood immediately (Day 8)
3. Inspect ribbon (Day 5)
4. Keep ribbon on Marie (corruption stays low)
5. Show consistent care
6. Reach Day 10 successfully

### For BAD ENDING + Specific Events:

**Ribbon Removed Path:**

- Let Marie's corruption reach 70% OR
- Directly remove ribbon on Day 5

**Blood Neglect Path:**

- Ignore blood on Day 8
- Ignore blood again on Day 9
- Triggers nightmare

**Oliver Neglect Path:**

- Don't comfort Oliver on 3 separate occasions
- Let his mood drop each time

**Corruption Path:**

- Ignore all dolls consistently
- Let 2+ reach 70% corruption
- Neglect Elizabeth and Marie especially

---

## TIPS FOR COMPLETION

✅ **For 100% Achievements:**

1. Play through for **Good Ending** first → unlocks positive events
2. Play through for **Bad Ribbon** ending
3. Play through for **Bad Blood** ending
4. Play through for **Bad Oliver** ending
5. Play through for **Bad Corruption** ending
6. **Neutral Ending** unlocks if you don't commit to any path

✅ **Checkpoint moments:**

- Day 2: Oliver emotion check
- Day 3: Elizabeth corruption check
- Day 5: Ribbon decision (inspect = discovery, remove = bad ending)
- Day 6: Doll movement check
- Day 8: Blood event (ignore for bad ending)
- Day 9: Corruption check
- Day 10: Ending trigger

✅ **Tracking progress:**

- Watch dolls' mood/corruption percentages
- Monitor blood/ribbon status
- Track days reached
- Check which ending path you're on

---

## SUMMARY TABLE

| Achievement        | Condition                     | Triggers | Path     |
| ------------------ | ----------------------------- | -------- | -------- |
| Good Ending        | All dolls stable, clean blood | Day 10   | Positive |
| Neutral Ending     | No bad ending reached         | Day 10   | Balanced |
| Ribbon Removed     | Remove/corrupt ribbon         | Day 5+   | Bad      |
| Blood Neglect      | Ignore blood 2 days           | Day 9    | Bad      |
| Oliver Neglect     | Don't comfort 3x              | Any day  | Bad      |
| High Corruption    | 2+ dolls at 70%               | Any day  | Bad      |
| Sorrow             | Oliver mood < 50%             | Day 2+   | Event    |
| Distorted          | Elizabeth mood < 50%          | Day 3+   | Event    |
| Discovery          | Inspect ribbon                | Day 5    | Event    |
| Corruption Spreads | Marie corruption 70%+         | Day 5+   | Event    |
| Unnatural          | 2+ dolls 50%+ corruption      | Day 6    | Event    |
| Accident           | Elizabeth's blood spills      | Day 8    | Event    |
| Neglect            | Ignore blood 2 days           | Day 9    | Event    |
| Chaos              | Average corruption 50%+       | Day 9    | Event    |
