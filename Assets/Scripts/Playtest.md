
# **COMPLETE GAME FLOW GUIDE FOR TESTING**

## **STARTING STATS**
```
Elizabeth (Angry Doll):
  Mood: 50/100    Cleanliness: 80/100    Corruption: 40/100

Oliver (Weeping Doll):
  Mood: 30/100    Cleanliness: 80/100    Corruption: 10/100

Marie (Ribbon Doll):
  Mood: 90/100    Cleanliness: 100/100   Corruption: 30/100
```

---

## **DAY-BY-DAY PROGRESSION**

### **DAY 1** - Introduction
**Message:** 
> "It smells of dust in here... What's this huge box? Oh — a note fell! 'Sweetie, take care of them. They were my closest companions.' Hmm. Grandma took good care of these."

**Choices:**
- Care for Elizabeth
- Care for Oliver  
- Care for Marie

**What happens:** You have **3 interaction slots**. Use them wisely—each interaction has different stat changes.

**Key interactions:**
- **Elizabeth → Clean**: +30 cleanliness, +5 mood, -5 corruption
- **Elizabeth → Brush**: +20 mood, -10 corruption
- **Oliver → Comfort**: +15 mood, -10 corruption
- **Oliver → Gift (ribbon/clover)**: +20 mood
- **Oliver → Gift (wrong item)**: -10 mood
- **Marie → Clean**: +30 cleanliness, +5 mood, -5 corruption

---

### **DAY 2** - Observation & Warnings
**Message (base):**
> "Hmm. Why is Oliver's face wet? Just like tears."

**Additional messages appear if:**
- Elizabeth mood < 50 → "Elizabeth's expression looks... tighter than yesterday."
- Any doll cleanliness < 50 → "There's dust gathering on the shelf."
- Marie corruption > 50 → "Marie's ribbon twitches. Did it... move?"
- **NIGHTMARE FLAG SET** → "...Elizabeth was in your dreams last night. She looked angry." ⚠️

**Stat decay happens at night:**
- Elizabeth: -20 cleanliness (harsh!)
- Oliver & Marie: -10 cleanliness each
- **If mood < 30 AND cleanliness < 30**: +20 corruption
- **If mood < 30 OR cleanliness < 30**: +10 corruption

---

### **DAY 3** - Escalation
**Message (base):**
> "The morning is quiet. The dolls sit on the shelf."

**Additional messages:**
- Oliver mood < 30 → "Oliver is... definitely crying. There are tiny wet streaks on his cheeks."
- Elizabeth mood < 50 → "Elizabeth's face looks distorted. Like a smile that's too wide."

**What to watch:** If you neglected Oliver on Day 1-2, he should show signs now.

---

### **DAY 4** - They're Watching
**Message:**
> "They're watching me. I walked across the room and — their eyes followed. Left. Right. Left again. ...They're just dolls."

**Choices:** Standard care options.

**Tension point:** This is when the horror vibe escalates. Good time to increase care if dolls are declining.

---

### **DAY 5** - MARIE'S RIBBON DECISION POINT ⚠️
**Message:**
> "Marie's ribbon is dark red today. Wet. It smells... rusty. Like blood. Should I remove it?"

**Critical Choices:**
1. **"Remove the ribbon"** → **BAD END IMMEDIATELY**
   - Marie corruption: +50
   - Marie mood: -20
   - Game over with message: "The ribbon unravels. Something in the air changes."

2. **"Leave it alone"** → Continue (no interaction spent)
   - Marie: No mood change, counts as interaction

3. **"Not today"** → Skip all interactions

**Important:** This is a hard point—once you remove the ribbon, game ends.

---

### **DAY 6** - THE SWITCH
**Message (if 2+ dolls corruption > 50):**
> "They've switched places. Elizabeth is where Oliver was. Oliver where Marie sat. I didn't touch them."

**Special Choices:**
1. **"Switch them back"** → Both swapped dolls +5 corruption each
2. **"Leave them"** → No interaction
3. **"Interact with them"** → Normal choices return

**If corruption is low:** Just shows "A quieter day. The shelf looks the same as you left it."

---

### **DAY 7** - HAIR GROWS
**Message (if Elizabeth mood < 40 OR cleanliness < 40):**
> "Elizabeth's hair is longer. That's impossible. But it is. It trails over the shelf edge now."

**Message (if Elizabeth is healthy):**
> "Day 7. More than halfway through grandma's '10 days'."

**Stat warning:** You're past halfway. If dolls are very corrupted, it's getting dangerous.

---

### **DAY 8** - BLOOD SPLASH ⚠️ CRITICAL
**Message:**
> "I reached for something on the shelf and— A paper cut. My hand is bleeding. The blood drops hit Elizabeth's dress."

**Critical Choices:**
1. **"Clean Elizabeth immediately"** → Removes blood, avoids bad end
2. **"It's fine, ignore it"** → Blood stays, sets `bloodNotCleanedFlag` ⚠️
3. **"Interact with others"** → Doll choices (but blood still on Elizabeth!)

**If you ignore the blood:** On Day 8 night → **BAD END TRIGGERED**
- Message: "Something spreads from Elizabeth's dress. You should have cleaned it."

---

### **DAY 9** - INTENSIFICATION
**Message (base):**
> "The house is quiet. Too quiet."

**Message (if avg corruption > 50):**
> "They're all staring at me. I can feel it even when my back is turned."

**Last chance:** This is your final day to improve stats before judgment.

---

### **DAY 10** - FINAL JUDGMENT
**Message:**
> "Day 10. Grandma's note said '10 days'. The dolls are all looking at me at once. Whatever happens today — it ends today."

**You have 3 final interactions.** Then night falls and ending is determined.

---

## **ENDING SYSTEM**

After Day 10 night processing, the game checks in this order:

### **BAD ENDINGS** (checked first):
1. **Marie's ribbon removed** (Day 5) → "The ribbon unravels completely. The room goes dark."
2. **Elizabeth's blood not cleaned** (Day 8) → "Something spreads from Elizabeth's dress."
3. **Oliver not comforted 3+ days** → "Oliver's crying fills the house. There is nothing left to comfort."
4. **World collapse** (2+ dolls corruption ≥ 70) → "Something is very wrong. The dolls are no longer just dolls."

### **GOOD ENDING**:
All three dolls MUST have:
- Mood ≥ 60
- Corruption < 40

Message: "The dolls smile at you. You are welcome to stay. — GOOD END —"

### **NEUTRAL ENDING**:
Everything else (fallback).

Message: "The dolls stare. Neither acceptance nor rejection. — NEUTRAL END —"

---

## **TESTING SCENARIOS**

### **Test 1: GOOD END** ✅
Days 1-10: **Always use Comfort/Clean/Gift** → Maximize mood, minimize corruption
- Elizabeth: Clean daily + Brush
- Oliver: Comfort every day + occasional Gift
- Marie: Clean regularly
- **Expected:** All stats green → GOOD END

### **Test 2: NEUTRAL END** 
Days 1-10: **Mixed care** → Some days good, some bad
- Use only 1-2 interactions per day
- Ignore dolls occasionally
- **Expected:** Stats mediocre → NEUTRAL END

### **Test 3: Oliver Bad End** 🔴
Days 1-4: **Ignore Oliver entirely**
- Never use Comfort
- Use other interactions only
- Day 5-10: Can care for him, but too late
- **Expected:** Day after Day 10 → Bad end message

### **Test 4: Marie Bad End** 🔴
Day 5: **Remove the ribbon**
- **Expected:** Immediate bad end screen

### **Test 5: Elizabeth Blood Bad End** 🔴
Day 8: **Ignore blood** (choose "It's fine, ignore it")
- Days 9-10: Can still play
- **Expected:** After Day 10 night → Bad end

### **Test 6: World Collapse** 🔴
Days 1-10: **Neglect all dolls heavily**
- Ignore everyone
- Let corruption build to 70+ for 2 dolls
- **Expected:** Bad end on Day 10

---

## **STAT TRACKING CHECKLIST**

Watch these metrics each night:

| Metric | Daily Change | Trigger |
|--------|--------------|---------|
| Cleanliness | -10 (Elizabeth -20) | Passive decay |
| Mood | -10 (if ignored) | Ignore action |
| Corruption | +2-20 | Low mood/cleanliness |
| Neglect counter | Increments if ignored | Track for Oliver |
| Marie ignore days | Increments, +2 corruption/day | Passive |

---

## **QUICK START FOR YOUR FIRST TEST RUN**

**Goal:** Reach GOOD END

1. **Day 1:** Clean Elizabeth, Comfort Oliver, Clean Marie
2. **Day 2-4:** Comfort Oliver daily (reset counter), Clean Elizabeth when mood drops
3. **Day 5:** Leave ribbon alone (not today if unsure)
4. **Day 6-7:** Keep all dolls happy
5. **Day 8:** MUST clean Elizabeth's blood
6. **Day 9-10:** Final pushes to get all mood ≥60, corruption <40

**Success = All dolls smiling at you! 🎉