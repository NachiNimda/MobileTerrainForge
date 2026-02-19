# Video Templates Structure

## Template Categories

Each video in "The Quiet Craft of Optimization" series follows a consistent structure for branding, pacing, and audience familiarity.

---

## Episode Template (8-12 minute format)

### 1. Opening Sequence (30-45 seconds)

**Standard Elements:**
- Logo reveal (2-3 seconds, minimal animation)
- Problem statement visualization (15-20 seconds)
- Performance tease (10-15 seconds)

**Reusable Assets:**
- `intro-logo.mp4` — Animated logo with grid texture
- `intro-transition.mp4` — Fade from logo to problem statement
- `metrics-overlay.png` — Metric callout template (75%, 62%, 33%)

**Customizable:**
- Problem visualization (episode-specific)
- Teaser metrics (episode-specific wins)

### 2. Content Segments (6-9 minutes total)

**Segment Structure (repeated 2-4 times per episode):**

```
[Segment Title Overlay - 3 seconds]
├── Problem Explanation (45-90 seconds)
│   ├── Diagram/Animation
│   └── Narration
├── Technical Deep Dive (45-90 seconds)
│   ├── Code snippet or flowchart
│   └── Narration
└── Solution/Demonstration (45-90 seconds)
    ├── Live demo or comparison
    └── Performance metrics
```

**Reusable Assets:**
- `segment-title-overlay.png` — Template for segment titles
- `code-panel-template.png` — Dark theme code overlay frame
- `diagram-bg.svg` — Grid pattern background for diagrams
- `metrics-template.png` — Standardized metric display

**Customizable:**
- Segment title text
- Diagram content
- Code snippets
- Demo footage

### 3. Comparison Sequence (60-90 seconds)

**Standard Elements:**
- Side-by-side layout (traditional vs optimized)
- Synchronized metric graphs
- Before/after split screens

**Reusable Assets:**
- `split-screen-template.mp4` — Animated split-screen transition
- `graph-template.svg` — Baseline axis and grid for graphs
- `comparison-overlay.png` — Labels and annotations

**Customizable:**
- Comparison footage (episode-specific)
- Graph data
- Annotation text

### 4. Conclusion Sequence (30-45 seconds)

**Standard Elements:**
- Summary montage (key moments from episode)
- Final metrics display
- Call to action (Asset Store link)

**Reusable Assets:**
- `outro-montage-template.mp4` — Transition sequence for summary
- `final-metrics.png` — Full metrics display template
- `cta-overlay.png` — Call to action button template

**Customizable:**
- Montage content (episode-specific key moments)
- Final metrics values

### 5. Closing (5-10 seconds)

**Standard Elements:**
- Logo fade-in
- Subscribe prompt
- Series teaser

**Reusable Assets:**
- `outro-logo.mp4` — Logo animation
- `subscribe-prompt.png` — "Subscribe for more" overlay
- `teaser-next-episode.png` — Next episode title

**Customizable:**
- Teaser text (next episode title)

---

## Short Video Template (60-90 seconds for YouTube Shorts)

### Structure

**[0:00-0:05]** Hook — Single striking visual with 3-word title
**[0:05-0:30]** Problem — Quick demonstration of issue
**[0:30-0:60]** Solution — Code snippet or technique
**[0:60-0:75]** Result — Before/after comparison
**[0:75-0:90]** CTA — "Watch the full episode" + link

### Reusable Assets
- `shorts-hook-overlay.png` — Title template
- `shorts-comparison.mp4` — Split-screen transition
- `shorts-cta.png` — Call to action button

---

## Tutorial Video Template (15-20 minutes)

### Structure (for future, longer-form content)

**Part 1: Conceptual Overview (2-3 minutes)**
- Problem statement
- Theoretical background
- Key concepts

**Part 2: Implementation (8-12 minutes)**
- Code walkthrough
- Step-by-step explanation
- Common pitfalls

**Part 3: Optimization (2-3 minutes)**
- Performance considerations
- Alternatives and trade-offs

**Part 4: Demo & Results (2-3 minutes)**
- Live demonstration
- Performance metrics
- Summary

### Reusable Assets
- All episode template assets
- `tutorial-code-overlay.png` — Larger code panel for detailed explanation
- `tutorial-timeline.png` — Section markers for longer videos

---

## Asset Organization

```
templates/
├── intro/
│   ├── logo-reveal.mp4
│   ├── transition.mp4
│   └── metrics-overlay.png
├── segments/
│   ├── title-overlay.png
│   ├── code-panel.png
│   ├── diagram-bg.svg
│   └── metrics-template.png
├── comparison/
│   ├── split-screen.mp4
│   ├── graph-template.svg
│   └── overlay.png
├── outro/
│   ├── montage-template.mp4
│   ├── final-metrics.png
│   ├── cta-overlay.png
│   └── logo-fade.mp4
└── shorts/
    ├── hook-overlay.png
    ├── comparison.mp4
    └── cta.png
```

---

## Color Palette (Consistent Across All Templates)

### Primary Colors
- **Deep Blue:** `#1A365D` (backgrounds, text)
- **Charcoal:** `#2D3748` (secondary backgrounds)
- **Off-White:** `#F7FAFC` (text, diagrams)

### Accent Colors
- **Teal:** `#38B2AC` (success, positive metrics)
- **Orange:** `#ED8936` (warnings, challenges)
- **Red:** `#E53E3E` (problems, errors)

### Transparency Levels
- **Overlays:** 70% opacity (readable over footage)
- **Watermarks:** 10% opacity (subtle branding)
- **Gradients:** 40-60% opacity (transitions)

---

## Typography Standards

### Font Family
- **Primary:** Inter or Roboto (clean, readable)
- **Secondary:** JetBrains Mono or Fira Code (for code snippets)
- **Accent:** Source Sans Pro or Open Sans (titles, emphasis)

### Sizes (for 1080p video)
- **Main Titles:** 72-96px (bold)
- **Segment Titles:** 48-64px (medium)
- **Body Text:** 28-36px (regular)
- **Code Snippets:** 24-32px (monospace)
- **Captions:** 32-40px (for accessibility)

### Weight & Style
- **Bold:** Titles, emphasis, key metrics
- **Medium:** Headings, labels
- **Regular:** Body text, explanations
- **Italic:** Technical terms, variable names

---

## Animation Standards

### Transition Speeds
- **Fade In/Out:** 0.5-1.0 seconds (smooth, not abrupt)
- **Slide Transitions:** 0.3-0.5 seconds (quick but controlled)
- **Element Reveals:** 0.2-0.4 seconds per element (staggered)

### Motion Principles
- **Slow and Deliberate:** No flashy, fast movements
- **Purposeful Motion:** Every animation should explain, not decorate
- **Minimal Easing:** Use linear or slight ease-in-out, avoid bouncy effects

### Code Animation
- **Syntax Highlighting:** Highlight one line at a time (0.5 seconds per line)
- **Scroll Speed:** 10-15 lines per second maximum (readable)
- **Zoom Level:** Keep code at readable size, avoid excessive zoom

---

## Audio Standards

### Voiceover Levels
- **Peak:** -3dB to -6dB (avoid clipping)
- **Average:** -12dB to -18dB (consistent volume)
- **Floor:** -24dB minimum (above noise floor)

### Background Music
- **Level:** -20dB relative to voiceover
- **Genre:** Minimal piano, ambient electronic (no lyrics)
- **Fade Times:** 1-2 seconds in/out (smooth, not abrupt)

### Sound Effects
- **Volume:** -15dB to -25dB (subtle)
- **Type:** UI sounds, subtle whooshes, ambient clicks
- **Frequency:** Limited to 2-3 per video (avoid overuse)

---

## Quality Control Checklist

### Visual
- [ ] All overlays use consistent color palette
- [ ] Text is readable at 60% scale (mobile test)
- [ ] Code snippets use monospace font
- [ ] No typos or grammatical errors
- [ ] Animations are smooth, not jarring

### Audio
- [ ] Voiceover levels normalized throughout
- [ ] Background music doesn't compete with narration
- [ ] No clipping or distortion
- [ ] Consistent pacing (140-160 words per minute)
- [ ] Proper fade-in/out at start/end

### Branding
- [ ] Logo appears in intro and outro
- [ ] Channel watermark present (after 30 seconds)
- [ ] Color scheme matches channel branding
- [ ] Typography consistent with series
- [ ] No competing brand logos or watermarks

### Content
- [ ] All technical claims have supporting visuals
- [ ] Code examples are accurate and tested
- [ ] Metrics are based on real data
- [ ] No exaggerated claims or marketing fluff
- [ ] Pacing allows concepts to land (2-3 second pauses)

---

## Template Updates

### Version Control
- Store template assets in version control (Git)
- Tag releases: `v1.0`, `v1.1`, etc.
- Document changes in `CHANGELOG.md`

### Improvement Process
After each episode, review:
- Which templates worked well?
- Which need refinement?
- What new templates would help?
- Update templates before next episode

### A/B Testing (Optional)
For critical elements (CTA buttons, thumbnail styles):
- Create two versions
- Test with small audience sample
- Measure click-through and engagement
- Adopt winning version for future videos

---

## Notes for Future Episodes

1. **Episode 1:** All templates created fresh. Save as v1.0.
2. **Episode 2:** Use v1.0 templates. Note improvements needed.
3. **Episode 3:** Update to v1.1 based on Episode 2 feedback.
4. **Ongoing:** Refine incrementally, avoid drastic changes between episodes.

Consistency builds trust. Small, thoughtful improvements over time.