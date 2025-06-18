# 🃏 Unity C_M

## 📁 Script Breakdown

### 🔹 `Card.cs`
Handles individual card behavior:
- Stores card metadata (ID, name, grid position)
- Controls card flipping animation
- Triggers match logic in `GameManager`
- Rescales child image to lower power-of-2 sizes for optimization

### 🔹 `CardGridScaler.cs`
Manages grid layout:
- Dynamically creates and positions cards
- Adjusts spacing and scaling based on screen resolution
- Randomly assigns sprites to cards in pairs
- Validates grid config (even number of cards)

### 🔹 `GameManager.cs`
Controls core game flow:
- Singleton pattern
- Manages card selection and matching logic
- Tracks score and resets
- Handles difficulty selection and layout configuration
- Manages "Play Again" trigger

### 🔹 `ScoreHandler.cs`
Updates UI for:
- Match count
- Turn count
- Resets values when game restarts

### 🔹 `SoundManager.cs`
Central sound system:
- Background music loop (low volume)
- SFX for flip, button, and radio toggle
- Singleton-based, with customizable volume levels

### 🔹 `PlayAgainAnimation.cs`
Animates the "Play Again" button:
- Moves the button into view with easing
- Uses `Transform.Lerp` over two phases for polish

### 🔹 `ChangeRadioButton.cs`
Handles difficulty radio toggle visuals:
- Activates visual toggle based on saved difficulty setting (`PlayerPrefs`)

---

## 🎚️ Difficulty Levels

Defined using `eDifficulty` enum:
- `Easy`: fewer cards, slower visibility ease
- `Medium`: moderate difficulty
- `Hard`: more cards, faster flip animation

Configured via `GameManager → gridLayout`.

---

## 🔊 Audio Support

The following clips are managed in `SoundManager.cs`:
- `backgroundMusic`
- `buttonClickSound`
- `radioButtonSound`
- `flipCardSound`

Attach your clips in the Inspector and tune volume settings.

---

## 🔁 Events Used

- `resizeCardsEvent`: invoked when screen or layout changes
- `GameReset`: called on reset/play again to reinitialize everything

---

## 🧱 Interfaces

### `IReset`
Implemented by scripts that need to reset values on game restart (`CardGridScaler`, `ScoreHandler`, etc.)

---
