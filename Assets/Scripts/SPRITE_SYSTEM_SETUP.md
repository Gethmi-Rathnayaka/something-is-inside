# Sprite Switching System - Setup Guide

This guide explains how to set up and use the new sprite switching system for dynamic visual changes based on game events.

## Overview

The system consists of several components:

- **SpriteVariants.cs** - Holds all sprite references for a single doll
- **DollSpriteState.cs** - Tracks special visual states (blood, long hair, wrapped ribbon, etc.)
- **DollVisuals.cs** (updated) - Applies the appropriate sprite based on current state
- **ScreenEffectsManager.cs** - Handles screen effects (blood splash, vignette, flashes)
- **BackgroundManager.cs** - Manages background sprite changes
- **DayEventManager.cs** (updated) - Integrates everything into day events

---

## Setup Steps

### 1. Create Your Sprites

Create sprite images for each visual variant:

**Elizabeth:**
- `elizabeth_normal` - Default state
- `elizabeth_sad` - Low mood
- `elizabeth_corrupted` - High corruption
- `elizabeth_dirty` - Low cleanliness
- `elizabeth_with_blood` - Blood on dress (Day 8)
- `elizabeth_long_hair` - Hair grown longer (Day 7+)
- `elizabeth_distorted_face` - Distorted expression (Day 3+)
- `elizabeth_watching` - Eyes following you (Day 4)

**Oliver:**
- `oliver_normal` - Default state
- `oliver_sad` - Low mood
- `oliver_corrupted` - High corruption
- `oliver_dirty` - Low cleanliness
- `oliver_wet` - Wet from crying (Day 2+)
- `oliver_watching` - Eyes following you

**Marie:**
- `marie_normal` - Default state
- `marie_sad` - Low mood
- `marie_corrupted` - High corruption
- `marie_dirty` - Low cleanliness
- `marie_wrapped_in_ribbon` - Ribbon wrapping around her (Day 5+, high corruption)
- `marie_watching` - Eyes following you

**Background:**
- `bg_normal` - Default peaceful shelf
- `bg_creepy` - Darker, unsettling (corruption > 55)
- `bg_blood_stained` - Worst case (corruption > 75)
- `bg_final_day` - Day 10 atmosphere

### 2. Set Up Doll Sprites in Inspector

For each doll GameObject (Elizabeth, Oliver, Marie):

1. Select the doll in the Hierarchy
2. Find the **DollVisuals** component
3. In the Inspector, under "Sprite Variants", assign your sprites:
   - **Base Sprites**: defaultSprite, sadSprite, corruptedSprite, dirtySprite
   - **Special State Sprites**: withBloodSprite, longHairSprite, wrappedInRibbonSprite, wetSprite, distortedFaceSprite, eyeFollowingSprite

Example for Elizabeth:
```
Sprite Variants:
  - Default Sprite: elizabeth_normal
  - Sad Sprite: elizabeth_sad
  - Corrupted Sprite: elizabeth_corrupted
  - Dirty Sprite: elizabeth_dirty
  - With Blood Sprite: elizabeth_with_blood
  - Long Hair Sprite: elizabeth_long_hair
  - Distorted Face Sprite: elizabeth_distorted_face
  - Eye Following Sprite: elizabeth_watching
```

### 3. Set Up Screen Effects Manager

1. Create a new UI Panel (if you don't have one already)
   - Right-click Canvas → UI → Panel
   - Name it "ScreenEffectsOverlay"
   - Set its color to black with 0 alpha (transparent)

2. Create an empty GameObject to hold the ScreenEffectsManager script
   - Add the **ScreenEffectsManager.cs** script
   - Assign the panel to the "screenOverlay" field

3. Optional: Set the Image color to something else (red for blood effects, black for vignette, etc.)

### 4. Set Up Background Manager

1. Select your background GameObject (the one with the shelf/room image)
2. Add the **BackgroundManager.cs** script
3. Get the SpriteRenderer component reference (it should auto-detect)
4. In the Inspector, assign background sprites:
   - Normal Background: `bg_normal`
   - Creeky Background: `bg_creepy`
   - Blood Stained Background: `bg_blood_stained`
   - Final Day Background: `bg_final_day`

### 5. Wire Up DayEventManager

1. Select the DayEventManager GameObject
2. The script now automatically finds BackgroundManager and ScreenEffectsManager via singletons
3. Nothing else to assign—it's ready to go!

---

## How It Works

### Sprite Priority System

When `UpdateVisuals()` is called on a doll, sprites are chosen in this priority:

1. **Special states** (highest priority)
   - Has blood? → Use `withBloodSprite`
   - Has long hair? → Use `longHairSprite`
   - Wrapped in ribbon? → Use `wrappedInRibbonSprite`
   - Is wet? → Use `wetSprite`
   - Has distorted face? → Use `distortedFaceSprite`
   - Is watching? → Use `eyeFollowingSprite`

2. **State-based sprites** (if no special states apply)
   - Corruption > 60? → Use `corruptedSprite`
   - Mood < 30? → Use `sadSprite`
   - Cleanliness < 30? → Use `dirtySprite`

3. **Default sprite** (fallback)

### Event-Based Sprite Changes

The DayEventManager automatically triggers sprite changes:

- **Day 2**: Oliver's sprite changes to "wet" if mood < 50
- **Day 3**: Elizabeth's sprite shows "distorted face" if mood < 50
- **Day 5**: Marie's sprite wraps in ribbon if corruption > 70 and ribbon is left alone
- **Day 7**: Elizabeth's sprite shows "long hair" if mood/cleanliness < 40
- **Day 8**: Elizabeth's sprite shows "blood" + blood splash screen effect plays
- **Day 10**: Background changes to final day atmosphere

### Screen Effects

Currently available:
- `PlayBloodSplashEffect()` - Red flash that fades out (for Day 8)
- `PlayVignetteEffect()` - Darkening vignette for creepy atmosphere
- `PlayFlash()` - Generic flash with custom color

### Background Changes

The background updates based on average corruption:
- **Corruption < 55**: Normal background
- **Corruption 55-75**: Creepy background
- **Corruption > 75**: Blood-stained background
- **Day 10**: Final day background (always)

---

## Customization

### Add a New Sprite State

1. Add a field to `DollSpriteState` in **SpriteVariants.cs**
2. Add a corresponding sprite field in `SpriteVariants` class
3. Update `GetAppropriateSprite()` to check your new state
4. Update `SetSpriteFlag()` in **DollVisuals.cs** to handle the new flag name
5. Call `doll.visuals.SetSpriteFlag("yourFlag", true/false)` when the event happens

Example: Adding an "isScreaming" sprite:
```csharp
// In DollSpriteState:
public bool isScreaming = false;

// In SpriteVariants:
[SerializeField] public Sprite screamingSprite;

// In GetAppropriateSprite():
if (spriteState.isScreaming && screamingSprite != null)
    return screamingSprite;

// In DollVisuals.SetSpriteFlag():
case "isScreaming":
    spriteState.isScreaming = value;
    break;

// In your event code:
doll.visuals.SetSpriteFlag("isScreaming", true);
doll.visuals.UpdateVisuals(doll.state);
```

### Add a New Screen Effect

1. Add a new method to **ScreenEffectsManager.cs**
2. Create a coroutine that manipulates the screen overlay
3. Call from **DayEventManager** when needed

Example: Adding a white flash:
```csharp
public void PlayWhiteFlash()
{
    PlayFlash(Color.white, 0.5f);
}

// Then in DayEventManager:
if (screenEffectsManager != null)
{
    screenEffectsManager.PlayWhiteFlash();
}
```

---

## Testing

### Quick Test Checklist

- [ ] Day 2: Oliver should show wet sprite if mood < 50
- [ ] Day 3: Elizabeth should show distorted face if mood < 50
- [ ] Day 5: Marie's ribbon wraps if corruption > 70
- [ ] Day 7: Elizabeth shows long hair if mood/cleanliness < 40
- [ ] Day 8: Blood splash effect plays + Elizabeth shows blood sprite
- [ ] Day 8: Cleaning Elizabeth removes blood sprite
- [ ] Background darkens as corruption increases
- [ ] Day 10: Background changes to final atmosphere

### Debug Tips

- Check Console for sprite loading errors
- Use `Debug.Log()` to verify sprite state changes
- Verify all sprites are assigned in Inspector (no missing references)
- Make sure ScreenEffectsManager has a panel assigned
- Verify BackgroundManager is on the background GameObject

---

## Advanced: Conditional Sprite Changes

You can trigger sprite changes based on any game state. For example:

```csharp
// In DayEventManager or InteractionManager:

// Change sprite based on interaction
if (elizabeth.state.corruption > 80 && elizabeth.visuals != null)
{
    elizabeth.visuals.SetSpriteFlag("hasDistortedFace", true);
    elizabeth.visuals.UpdateVisuals(elizabeth.state);
}

// Clear sprite when no longer needed
if (marie.state.corruption < 50 && marie.visuals != null)
{
    marie.visuals.SetSpriteFlag("isWrappedInRibbon", false);
    marie.visuals.UpdateVisuals(marie.state);
}

// Chain multiple sprite changes
if (oliver.state.mood < 10 && oliver.visuals != null)
{
    oliver.visuals.SetSpriteFlag("isWet", true);
    oliver.visuals.SetSpriteFlag("hasDistortedFace", true);
    oliver.visuals.UpdateVisuals(oliver.state);
}
```

---

## Summary

- Create all your sprite variants and assign them in the Inspector
- The system automatically picks the right sprite based on game state
- Special visual flags override normal state-based sprites
- Screen effects and background changes add atmosphere
- Everything is easily extensible for new sprites/effects
