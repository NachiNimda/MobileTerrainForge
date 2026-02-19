# Visual Asset Requirements: Episode 1 — "Why WFC? The Mathematics of Consistency"

## Core Visual Style

**Palette:** Muted blues, earth tones, charcoal grays — professional, technical, not flashy
**Typography:** Clean sans-serif (Inter, Roboto, or similar) — readable at all sizes
**Animation Style:** Slow, deliberate, mathematical — emphasize process over spectacle

## Static Diagrams

### WFC Concept Diagrams
- **Grid System Visualization:** 4x4 grid showing potential states, annotated with constraint arrows
- **Constraint Network:** Graph-style diagram showing relationships between adjacent tiles
- **Entropy Map:** Heatmap-style visualization showing remaining possibilities at each grid cell
- **Collapse Sequence:** 3-4 panel sequence showing step-by-step wave function collapse

### Performance Comparison Charts
- **Draw Call Bar Chart:** Side-by-side comparison (WFC vs traditional, labeled with percentages)
- **Memory Usage Line Graph:** Memory over time, both methods on same axis
- **FPS Comparison:** Frame rate stability visualization (spline charts)
- **Mobile Frame Budget:** Visual reference showing 16.67ms target for 60fps

### Technical Explanations
- **Code Snippet Panels:** C# code excerpts with syntax highlighting, commented key sections
- **Algorithm Flowchart:** Simplified WFC decision tree
- **Terrain Splatmap:** Visual representation of texture blending layers
- **LOD System Diagram:** Showing distance-based detail reduction

## Animated Sequences

### Introduction (0:00-0:45)
- **Logo Reveal:** Minimal fade-in of Mobile Terrain Forge logo (2-3 seconds)
- **Problem Statement:** Subtle dissolve between fractured terrain and smooth, consistent terrain
- **The Cost:** Animated counter showing draw calls accumulating (1, 2, 4, 8... 256)

### WFC Core Concept (1:30-4:00)
- **Constraint Propagation:** Animated grid showing constraints spreading outward
- **Entropy Reduction:** Heatmap animation showing possibilities narrowing
- **Collapse Moment:** Dramatic but quiet — cell resolves from uncertainty to certainty
- **Cascade Effect:** Watch one collapse trigger neighboring collapses in chain reaction

### Performance Demonstrations (4:00-6:30)
- **Side-by-Side Render:** Split screen showing terrain generation (traditional vs WFC)
  - Left: Individual mesh objects appearing one by one
  - Right: Single coherent mesh emerging smoothly
- **Memory Allocation:** Bar chart growing in real-time, showing WFC staying flatter
- **Frame Rate Graph:** Live graph during generation sequence, WFC stays stable

### Technical Deep Dive (6:30-8:00)
- **Code Flow Animation:** Lines of code highlighting in sequence as execution progresses
- **GPU Instancing Visualization:** Showing multiple instances sharing geometry data
- **Splatmap Animation:** Texture layers blending smoothly across terrain surface

### Outro (8:00-8:30)
- **Summary Metrics:** All three performance wins displayed together cleanly
- **Call to Action:** Simple text fade-in: "See for yourself" with Asset Store button mockup
- **Series Teaser:** Brief glimpse of upcoming episode topics

## Overlays & UI Elements

### Metric Callouts
- **Performance Labels:** "75% Fewer Draw Calls" / "62% Less Memory" / "33% Better FPS"
  - Style: Large, clean sans-serif, subtle gradient background
  - Timing: Appear with supporting visual, hold 3-4 seconds
- **Code Annotations:** Arrow markers pointing to relevant code lines
- **Timeline Markers:** Progress bar showing episode position at bottom

### Text Panels
- **Definition Cards:** When introducing WFC, brief definition with subtle animation
- **Comparison Table:** Feature-by-feature comparison (WFC vs noise-based)
  - Consistency: ✓ vs ✗
  - Determinism: ✓ vs ✗
  - Memory efficiency: ✓ vs ~
- **Key Takeaway Boxes:** End-of-section summaries in bordered boxes

## File Specifications

### Video Assets
- **Resolution:** 1920x1080 minimum, 3840x2160 preferred
- **Frame Rate:** 30fps (smooth but not excessive)
- **Duration:** 8-12 minutes total
- **Codec:** H.264 or H.265
- **Bitrate:** 8-12 Mbps for 1080p, 20-30 Mbps for 4K

### Image Assets
- **Diagrams:** SVG preferred (scalable), fallback PNG at 2x target resolution
- **Screenshots:** Lossless PNG, captured at 1080p or higher
- **Thumbnails:** 1280x720 (16:9), text readable at 30% scale

### Audio Assets
- **Voiceover:** WAV or AIFF, 44.1kHz or 48kHz, 24-bit
- **Background Music:** Lossless, royalty-free, piano or ambient
- **Sound Effects:** Subtle UI sounds, terrain generation ambience

## Asset Organization

```
episode-01/
├── audio/
│   ├── narration.wav
│   ├── music-main.mp3
│   └── sfx-ambient.wav
├── visuals/
│   ├── diagrams/
│   │   ├── grid-system.svg
│   │   ├── constraint-network.svg
│   │   └── entropy-map.svg
│   ├── animations/
│   │   ├── collapse-sequence.mp4
│   │   ├── side-by-side-compare.mp4
│   │   └── fps-graph.mp4
│   └── screenshots/
│       ├── terrain-final.png
│       └── wireframe-view.png
├── overlays/
│   ├── metrics/
│   └── text-panels/
└── thumbnail/
    └── final-thumb.png
```

## Production Notes

- Reusable assets (grids, charts, UI elements) should be saved as templates for future episodes
- All diagrams should use the same color scheme for consistency across the series
- Animated sequences should be modular — start/stop at clean points for easy editing
- Text overlays should avoid jargon where possible; define terms on first use
- Performance metrics should be based on real profiling data, not exaggerated claims