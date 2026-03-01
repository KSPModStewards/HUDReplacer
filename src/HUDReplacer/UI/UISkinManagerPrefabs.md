# UISkinManager Prefabs

All prefabs are retrieved via `UISkinManager.GetPrefab("name")`.

## Layout Prefabs

### UIVerticalLayoutPrefab
- `VerticalLayoutGroup`
- Default size: 150x700

### UIHorizontalLayoutPrefab
- `HorizontalLayoutGroup`
- Default size: 150x700

### UIGridLayoutPrefab
- `GridLayoutGroup`

## Text & Input

### UITextPrefab
- `TextMeshProUGUI`, `LayoutElement`
- Default size: 242x16.3

### UITextInputPrefab
- `Image`, `TMP_InputField`
- Default size: 160x30
- Children:
  - `Text Area` (`RectMask2D`)
    - `Placeholder` (`TextMeshProUGUI`)
    - `Text` (`TextMeshProUGUI`)

## Buttons & Toggles

### UIButtonPrefab
- `Image`, `Button`, `EventTrigger`
- Default size: 242x24
- Children:
  - `Text` (`TextMeshProUGUI`)

### UITogglePrefab
- `Toggle`, `LayoutElement`, `TooltipController_Text`
- Default size: 242x30
- Children:
  - `Background` (`Image`, 24x24)
    - `Checkmark` (`Image`, 24x24)
  - `Label` (`TextMeshProUGUI`)

### UIToggleButtonPrefab
- `Toggle`
- Children:
  - `Background` (`Image`)
    - `Checkmark` (`Image`)

## Images

### UIBoxPrefab
- `CanvasRenderer`, `Image`
- Default size: 10x10

### UIImagePrefab
- `CanvasRenderer`, `Image`
- Default size: 64x64

### UIRawImagePrefab
- `CanvasRenderer`, `RawImage`
- Default size: 64x64

## Complex

### UIScrollViewPrefab
- `Image`, `LayoutElement`
- Children:
  - `ScrollList` (`ScrollRect`, `Image`, `Mask`)
    - `Content` (content container)
  - `VerticalScrollbar` (`Image`, `Scrollbar`)
    - `Sliding Area`
      - `Handle` (`Image`)
  - `HorizontalScrollbar` (`Image`, `Scrollbar`)
    - `Sliding Area`
      - `Handle` (`Image`)

### UISliderPrefab
- `Slider`, `LayoutElement`, `TooltipController_Text`
- Default size: 200x26
- Children:
  - `Background` (`Image`)
  - `Fill Area`
    - `Fill` (`Image`)
  - `Handle Slide Area`
    - `Handle` (`Image`)

---

## Default Skin: "KSP window 7"

Key background sprites from `UISkinManager.defaultSkin`:

| Style | Normal BG | Text Color |
|-------|-----------|------------|
| window | `rect_round_dark_transparent` 64x64 | `(0.718, 0.996, 0.000)` |
| box | `rect_round_down_dark_veryTransparent` 64x64 | `(0.720, 1.000, 0.000)` |
| button | `rect_round_color` 64x64 | `(0.830, 0.958, 0.993)` |
| button (hover) | `rect_round_highlight` 64x64 | `(1, 1, 1)` |
| button (active) | `rect_round_down_dark_transparent` 69x69 | `(0.902, 0.902, 0.902)` |
| label | (none) | `(0.718, 0.996, 0.000)` |
| textField | `bevel_bg` 64x51 | `(0.992, 0.812, 0.000)` |
| textArea | `rect_round_down_dark_veryTransparent` 64x64 | `(0.902, 0.902, 0.902)` |
| toggle | `ledOff` 128x128 | `(0.891, 0.891, 0.891)` |
| toggle (hover) | `ledYellow` 128x128 | `(1, 1, 1)` |
| toggle (active) | `ledGreen` 128x128 | `(0.890, 0.890, 0.890)` |
| scrollView | `rect_round_down_dark_veryTransparent` 64x64 | — |
| vScrollbar | `rect_round_down_dark_transparent` 69x69 | — |
| vScrollbarThumb | `rect_round_color` 64x64 | — |
| hSlider | `bevel_bg` 64x51 | — |
| hSliderThumb | `rect_round_color` 64x64 | — |

### Custom Styles

| Name | Normal BG | Text Color |
|------|-----------|------------|
| List Item (1) | `rect_round_dark` | `(1.000, 0.776, 0.000)` |
| List Item (2) | (none) | `(0.678, 1.000, 0.000)` |
| List Item (3) | (none) | `(0.739, 0.739, 0.739)` |
| Delete Button | `rect_round_dark` | `(0.806, 0.558, 0.000)` |
| Icon Button | `rect_round_dark_transparent` | `(0.594, 1.000, 0.000)` |
| Block text | (none) | `(1, 1, 1)` |
| List Item 4 | (none) | `(1, 1, 1)` |
