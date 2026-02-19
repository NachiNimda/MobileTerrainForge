# Thumbnail Templates Design Specifications

## Thumbnail Philosophy

Thumbnails should be:
- **Minimal:** Not cluttered or overwhelming
- **Accurate:** Reflect actual content, no clickbait
- **Readable:** Text must be legible at 30% scale (YouTube recommendation)
- **Consistent:** Same style across all episodes

---

## Standard Thumbnail Layout (1280x720)

### Grid-Based Structure

```
┌────────────────────────────────────────────────────┐
│  [60px]    [Main Visual]                          │
│            (70% width)                            │
│                                                    │
│  [60px]    [Secondary Element]                    │
│            (30% width)                            │
│                                                    │
┌───────────────────┬────────────────────────────────┤
│  [Logo/Watermark] │  [Text Overlay]               │
│    (90px)         │   (Main Title)                │
└───────────────────┴────────────────────────────────┘
```

### Element Placement
- **Main Visual:** Left 60-70% of canvas — the most compelling visual from episode
- **Secondary Element:** Right 30-40% — supporting graphic, code snippet, or icon
- **Logo/Branding:** Bottom-left corner, subtle (60-90px height)
- **Text Overlay:** Right side, vertical center, maximum readability

---

## Episode 1 Thumbnail: "Why WFC? The Mathematics of Consistency"

### Concept
Show the moment of collapse — the key WFC concept

### Visual Elements
- **Main Visual (Left 65%):**
  - Grid of terrain tiles in mid-collapse
  - One tile resolved (solid), others still uncertain (semi-transparent)
  - Subtle arrows showing constraint propagation
  - Color palette: Deep blues, teal for resolved state

- **Secondary Element (Right 35%):**
  - Clean code snippet: `grid.Collapse();`
  - Dark background, syntax highlighting
  - Font: JetBrains Mono or similar

- **Text Overlay:**
  - Primary text: "Wave Function Collapse" (large, bold, white)
  - Secondary text: "75% Fewer Draw Calls" (smaller, teal accent)
  - Placement: Right side, stacked vertically

### Color Scheme
- Background: Dark blue gradient (`#1A365D` to `#2D3748`)
- Text: White (`#F7FAFC`) and teal accent (`#38B2AC`)
- Accents: Orange for emphasis (`#ED8936`)

---

## Episode 2 Thumbnail: "Mobile Optimization: Where Draw Calls Go to Die"

### Concept
Side-by-side comparison showing the problem

### Visual Elements
- **Main Visual (Left 65%):**
  - Split screen: traditional (top) vs optimized (bottom)
  - Top: Fractured terrain, red "X" overlay
  - Bottom: Smooth terrain, green checkmark
  - Visual contrast clearly shows the difference

- **Secondary Element (Right 35%):**
  - Performance chart (bar graph)
  - Two bars: "Old" (tall, red) vs "New" (short, green)
  - Labels: "Draw Calls" with actual numbers

- **Text Overlay:**
  - Primary text: "Mobile Optimization" (large, bold)
  - Secondary text: "Where Draw Calls Go to Die" (medium, italicized)
  - Tertiary: "62% Less Memory" (small, teal)

### Color Scheme
- Background: Charcoal gradient (`#2D3748` to `#1A202C`)
- Text: White, with red/green accent bars for comparison
- Accents: Green (`#38B2AC`) for success, red (`#E53E3E`) for problem

---

## Episode 3 Thumbnail: "The Hidden Cost of Noise"

### Concept
Visual noise vs structured generation

### Visual Elements
- **Main Visual (Left 65%):**
  - Perlin noise visualization (top) — smooth but random
  - WFC output (bottom) — structured, consistent
  - Arrow pointing from noise to WFC with label "Better"

- **Secondary Element (Right 35%):**
  - Screenshot of Unity editor
  - Warning icon next to noise-based terrain
  - "Inconsistent Results" text annotation

- **Text Overlay:**
  - Primary text: "The Hidden Cost" (large, bold)
  - Secondary text: "of Noise" (medium, same line or stacked)
  - Tertiary: "Why Perlin Isn't Enough" (small, italic)

### Color Scheme
- Background: Purple-blue gradient (`#4A5568` to `#2D3748`)
- Text: White, with purple accent (`#9F7AEA`)
- Accents: Yellow for warnings (`#ECC94B`)

---

## Episode 4 Thumbnail: "Determinism in Procedural Generation"

### Concept
Same seed, same result — every time

### Visual Elements
- **Main Visual (Left 65%):**
  - Three identical terrain screenshots side-by-side
  - Label above: "Same Seed"
  - Green checkmark overlay on each
  - "100% Reproducible" annotation

- **Secondary Element (Right 35%):**
  - Code snippet: `Seed("12345")`
  - Three checkmarks below, each with timestamp
  - Showing consistency across runs

- **Text Overlay:**
  - Primary text: "Determinism" (large, bold)
  - Secondary text: "Every Time" (medium, below primary)
  - Tertiary: "Same Seed, Same Result" (small)

### Color Scheme
- Background: Deep teal gradient (`#2C7A7B` to `#285E61`)
- Text: White, with teal accent (`#38B2AC`)
- Accents: Green for success indicators

---

## Episode 5 Thumbnail: "Memory-efficient Terrain Streaming"

### Concept
Loading without the lag

### Visual Elements
- **Main Visual (Left 65%):**
  - Terrain chunk appearing smoothly (no pop-in)
  - Loading bar showing memory usage (flat, stable)
  - "Smooth Streaming" label

- **Secondary Element (Right 35%):**
  - Memory graph (line chart)
  - Flat line for WFC (stable)
  - Spiked line for traditional (unstable)
  - Comparison annotation

- **Text Overlay:**
  - Primary text: "No More Pop-in" (large, bold)
  - Secondary text: "Terrain Streaming" (medium)
  - Tertiary: "Smooth Loading" (small, teal)

### Color Scheme
- Background: Slate blue gradient (`#4A5568` to `#2B6CB0`)
- Text: White, with blue accent (`#4299E1`)
- Accents: Green for smooth line, red for spiked line

---

## Episode 6 Thumbnail: "Building a Custom Terrain System"

### Concept
From zero to production-ready

### Visual Elements
- **Main Visual (Left 65%):**
  - Progress visualization: Empty grid → Partial → Complete
  - Three panels showing development stages
  - Arrow between each stage

- **Secondary Element (Right 35%):**
  - Unity Editor screenshot
  - Component inspector panel visible
  - "Production-Ready" badge

- **Text Overlay:**
  - Primary text: "Build Your Own" (large, bold)
  - Secondary text: "Terrain System" (medium)
  - Tertiary: "From Zero to Ship" (small)

### Color Scheme
- Background: Gray-blue gradient (`#4A5568` to `#2D3748`)
- Text: White, with blue accent
- Accents: Gold/yellow for progress indicators

---

## Episode 7 Thumbnail: "Case Study: 60fps on a Budget Device"

### Concept
Real-world results on real hardware

### Visual Elements
- **Main Visual (Left 65%):**
  - Mobile device screenshot (iPhone SE or similar budget phone)
  - Terrain running smoothly on screen
  - "60 FPS" overlay in corner

- **Secondary Element (Right 35%):**
  - Before/after comparison (traditional vs optimized)
  - FPS counters visible on both
  - Performance delta highlighted

- **Text Overlay:**
  - Primary text: "60 FPS" (very large, bold, green)
  - Secondary text: "On a Budget Device" (medium)
  - Tertiary: "Real Results" (small)

### Color Scheme
- Background: Dark gray (`#1A202C`) to black
- Text: White, with bright green accent (`#48BB78`)
- Accents: Green for 60fps, red for lower FPS

---

## General Thumbnail Rules

### Typography
- **Font:** Inter, Roboto, or Open Sans (clean, readable)
- **Primary Text:** 72-96px (bold, maximum readability)
- **Secondary Text:** 48-64px (medium weight)
- **Tertiary Text:** 32-40px (regular or light)
- **Contrast:** Minimum 4.5:1 ratio for accessibility

### Color Guidelines
- **Background:** Dark, muted colors (no neon, no rainbows)
- **Text:** White or off-white (`#F7FAFC`)
- **Accents:** One primary accent color per thumbnail (teal, blue, green)
- **Avoid:** More than 2-3 colors total

### Composition
- **Rule of Thirds:** Place key elements at intersection points
- **Whitespace:** Leave 10-15% margin around edges
- **Focal Point:** One clear focal point per thumbnail
- **Balance:** Visual weight distributed, not lopsided

### Technical Specs
- **Resolution:** 1280x720 (16:9)
- **Format:** PNG (lossless) or high-quality JPG (85%+ quality)
- **File Size:** Under 2MB for fast loading
- **Color Space:** sRGB (YouTube standard)

---

## A/B Testing Strategy

### Test Elements
For critical episodes (Episode 1, 3, 7), create 2 versions:

**Version A:** Concept-focused (emphasize the technical idea)
**Version B:** Result-focused (emphasize the performance win)

### Testing Method
1. Upload both versions as private videos
2. Share with small test audience (10-20 people)
3. Measure: Click-through rate, engagement, watch time
4. Adopt winning version for public release

### Metrics to Track
- **CTR (Click-Through Rate):** Percentage of impressions that result in views
- **AVD (Average View Duration):** How long viewers watch (thumbnail quality affects this)
- **Retention:** Percentage of viewers who watch past 30 seconds

---

## Thumbnail Production Checklist

For each thumbnail:

- [ ] Concept sketched and approved
- [ ] Visual assets prepared (screenshots, diagrams)
- [ ] Main visual composed in Canva/Photoshop
- [ ] Text overlay added (correct hierarchy, readable at 30%)
- [ ] Logo/watermark placed (bottom-left, subtle)
- [ ] Color scheme checked against palette
- [ ] Exported at 1280x720, PNG format
- [ ] File size under 2MB
- [ ] Tested at 30% scale for readability
- [ ] Saved as `episode-XX-thumb.png`

---

## Common Thumbnail Mistakes to Avoid

1. **Too Much Text:** More than 6-8 words total is cluttered
2. **Small Fonts:** Text must be readable at 30% scale
3. **Low Contrast:** Text must pop against background
4. **Inconsistent Branding:** Each thumbnail should clearly belong to the same series
5. **Clickbait:** Don't promise something the video doesn't deliver
6. **Busy Composition:** One focal point, not competing elements
7. **Wrong Aspect Ratio:** 1280x720 (16:9) is YouTube standard
8. **Blurry Images:** Use high-resolution source material

---

## Thumbnail Maintenance

### After Each Episode
- Note which thumbnail style performed best (CTR, AVD)
- Document lessons learned in thumbnail log
- Refine templates for next episode based on data

### Seasonal Updates (Optional)
- If re-releasing episodes, update thumbnails to reflect current branding
- Maintain core concept but refine composition/text
- Avoid complete rebranding between episodes

---

## Thumbnail Templates for Canva

### Create These Canva Templates:
1. `Episode-Standard-Template` — Base layout for all episodes
2. `Comparison-Template` — For before/after thumbnails (Episodes 2, 5, 7)
3. `Code-Snippet-Template` — For technical-focused thumbnails (Episodes 1, 4, 6)
4. `Device-Screenshot-Template` — For mobile-focused thumbnails (Episode 7)

### Template Elements (Save as Components):
- Logo/watermark
- Text overlay boxes (primary, secondary, tertiary)
- Performance chart templates
- Code snippet panel
- Comparison split-screen

Save these as reusable components in Canva for rapid thumbnail production.