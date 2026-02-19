# Storyboard Breakdown: Episode 1 — "Why WFC? The Mathematics of Consistency"

**Target Duration:** 8-10 minutes
**Pacing:** Deliberate, unhurried — allow concepts to land
**Visual Style:** Clean, mathematical, minimal motion

---

## Segment 0: Opening (0:00 - 0:30)

| Time | Visual | Audio | Notes |
|------|--------|-------|-------|
| 0:00-0:05 | Fade in: Minimal logo "Mobile Terrain Forge" on dark background, subtle grid texture | Ambient piano chord (gentle, minor key) | Establish quiet, professional tone |
| 0:05-0:15 | Cut to: Split screen — fractured, uneven terrain (left) vs smooth, coherent terrain (right) | Voice: "Terrain generation is simple. Getting it right, consistently, is not." | Show the problem visually |
| 0:15-0:25 | Close-up on fractured terrain: draw calls appearing as small red dots, accumulating rapidly | Voice: "The traditional approach? More draw calls, more memory, more compromises." | Animate the cost of traditional methods |
| 0:25-0:30 | Cut to: Text overlay "75% Fewer Draw Calls · 62% Less Memory · 33% Better FPS" | Voice: "But there is another way." | Tease the solution |

---

## Segment 1: The Problem with Traditional Methods (0:30 - 2:00)

| Time | Visual | Audio | Notes |
|------|--------|-------|-------|
| 0:30-0:45 | Diagram: Perlin noise visualization — smooth, continuous gradient | Voice: "Most terrain systems start with noise. It's elegant, mathematically beautiful." | Acknowledge the common approach |
| 0:45-1:00 | Animation: Noise sample generates terrain, but terrain appears random, disconnected | Voice: "But noise alone cannot guarantee consistency. Adjacent tiles may not connect. Biomes may transition abruptly." | Show the limitation |
| 1:00-1:15 | Side-by-side: Two generated terrains — both using same noise parameters, but different results | Voice: "Determinism is lost. The same seed, the same settings, can produce different results each time." | Emphasize the lack of determinism |
| 1:15-1:30 | Code snippet: `mesh = GenerateFromNoise(seed);` with arrow pointing to "non-deterministic" | Voice: "For tools, this is problematic. For games, it's unacceptable." | Connect to developer pain points |
| 1:30-1:45 | Graph: Memory usage climbing as terrain chunks load | Voice: "And the costs accumulate. More meshes, more draw calls, more memory pressure on mobile devices." | Show the performance impact |
| 1:45-2:00 | Cut to: Single text on dark screen — "What if we could do better?" (hold 3 seconds) | Voice: "What if we could guarantee consistency, determinism, and efficiency — all at once?" | Setup for the solution |

---

## Segment 2: Introducing WFC (2:00 - 3:30)

| Time | Visual | Audio | Notes |
|------|--------|-------|-------|
| 2:00-2:15 | Definition card fades in: "Wave Function Collapse" with brief definition | Voice: "Wave Function Collapse. An algorithm from quantum computing, adapted for procedural generation." | Define the core concept |
| 2:15-2:30 | Diagram: Grid visualization — each cell shows multiple possible states (icons of terrain types) | Voice: "At its core, WFC maintains a grid of possibilities. Each cell knows what it could be — but not yet what it is." | Visualize the uncertainty |
| 2:30-2:45 | Animation: Arrows flowing from one cell to adjacent cells, showing constraint propagation | Voice: "And crucially, each cell knows what its neighbors must be. Constraints are not optional — they're mathematical facts." | Show constraints in action |
| 2:45-3:00 | Heatmap animation: Grid colors shift from red (many possibilities) to blue (few possibilities) | Voice: "As constraints propagate, entropy decreases. Possibilities narrow. The system learns what it must be." | Explain entropy reduction |
| 3:00-3:15 | Dramatic collapse: One cell resolves from uncertainty to certainty, ripple effect spreads | Voice: "Then, the collapse. One cell chooses its state. And that choice forces every neighbor to adapt. A cascade of certainty." | The key moment — make it quiet but powerful |
| 3:15-3:30 | Cut to: Fully resolved grid — clean, consistent terrain | Voice: "The result is not random. It's the only terrain that satisfies all constraints. Every time." | Show the deterministic outcome |

---

## Segment 3: The Technical Implementation (3:30 - 5:00)

| Time | Visual | Audio | Notes |
|------|--------|-------|-------|
| 3:30-3:45 | Code panel: C# WFC implementation with syntax highlighting, key lines highlighted | Voice: "The implementation is deceptively simple. A constraint solver, an entropy tracker, a collapse mechanism." | Show it's approachable |
| 3:45-4:00 | Flowchart: Decision tree showing constraint checking logic | Voice: "Each step is mathematically sound. No heuristics, no guesses. Just logic." | Emphasize correctness |
| 4:00-4:15 | Animation: Terrain mesh being built chunk by chunk, all pieces fitting perfectly | Voice: "And the output? A single, unified mesh. No gaps, no seams, no compromises." | Show the practical benefit |
| 4:15-4:30 | Diagram: Splatsmap visualization — texture layers blending smoothly | Voice: "Texture blending becomes a matter of mathematics, not art. Consistent transitions, predictable results." | Explain texture integration |
| 4:30-4:45 | Code snippet: `mesh.CombineMeshes(wfcOutput);` with annotation | Voice: "A single draw call. One GPU pass. The difference is not subtle." | Connect to performance |
| 4:45-5:00 | Text overlay: "Deterministic · Consistent · Efficient" (each word appears sequentially) | Voice: "Deterministic. Consistent. Efficient. These are not trade-offs. They are the design." | Summarize the core values |

---

## Segment 4: Performance Demonstration (5:00 - 6:30)

| Time | Visual | Audio | Notes |
|------|--------|-------|-------|
| 5:00-5:15 | Split screen: Traditional method (left) vs WFC (right), both generating same terrain | Voice: "Let's see it in action. Two systems, same terrain, very different outcomes." | Set up the comparison |
| 5:15-5:30 | Left side: Draw call counter climbing rapidly (1, 5, 12, 28, 64...) | Voice: "The traditional approach: individual meshes for each terrain chunk. Draw calls accumulate." | Show the problem |
| 5:30-5:45 | Right side: Single draw call stays at 1, terrain emerges smoothly | Voice: "With WFC: one mesh. One draw call. The terrain arrives as a cohesive whole." | Show the solution |
| 5:45-6:00 | Side-by-side graphs: Memory usage (WFC line stays flat, traditional climbs) | Voice: "Memory usage reflects the same story. WFC uses 62% less memory on average." | Memory comparison |
| 6:00-6:15 | FPS monitor: WFC maintains stable 60fps, traditional drops to 40fps | Voice: "And on mobile devices, where every millisecond matters, frame rate stability is not optional." | FPS impact |
| 6:15-6:30 | All three metrics displayed together cleanly | Voice: "The mathematics of consistency has a practical cost. A cost that pays dividends in performance." | Summary of benefits |

---

## Segment 5: Real-World Application (6:30 - 7:30)

| Time | Visual | Audio | Notes |
|------|--------|-------|-------|
| 6:30-6:45 | Demo: Unity editor scene, Mobile Terrain Forge component selected | Voice: "In practice, this is not complicated. Add the component, define your constraints, and WFC does the rest." | Show it's easy to use |
| 6:45-7:00 | Inspector view: Clean, minimal settings (seed, biome rules, resolution) | Voice: "The complexity is hidden. You focus on what matters: your game, your vision." | Emphasize ease of use |
| 7:00-7:15 | Animation: Large terrain generating in real-time, smooth, no lag | Voice: "Generate small chunks or massive worlds. The algorithm scales. The performance stays consistent." | Show scalability |
| 7:15-7:30 | Cut to: Mobile device screen, terrain running smoothly at 60fps | Voice: "Mobile games demand optimization. WFC delivers it without sacrifice." | Connect to mobile target |

---

## Segment 6: The Competitive Landscape (7:30 - 8:15)

| Time | Visual | Audio | Notes |
|------|--------|-------|-------|
| 7:30-7:45 | Comparison table: Feature-by-feature (WFC vs noise-based systems) | Voice: "Not all terrain systems are equal. Many claim optimization. Few deliver it." | Acknowledge competition |
| 7:45-8:00 | Table highlights: Consistency (✓ vs ✗), Determinism (✓ vs ✗), Memory (✓ vs ~) | Voice: "True WFC implementation guarantees what noise can only hope to achieve." | Show differentiation |
| 8:00-8:15 | Text overlay: "The mathematics doesn't lie." (hold 3 seconds) | Voice: "The mathematics doesn't lie. And neither do the benchmarks." | Quiet confidence |

---

## Segment 7: Conclusion & Call to Action (8:15 - 9:00)

| Time | Visual | Audio | Notes |
|------|--------|-------|-------|
| 8:15-8:30 | Summary montage: All three key moments (collapse, mesh, performance) in rapid succession | Voice: "Wave Function Collapse. A different approach to terrain generation. One that prioritizes consistency over convenience." | Recap the journey |
| 8:30-8:45 | All three metrics displayed: "75% Fewer Draw Calls · 62% Less Memory · 33% Better FPS" | Voice: "75% fewer draw calls. 62% less memory. 33% better frame rates. These aren't marketing claims. They're the result of doing it right." | Restate the benefits |
| 8:45-8:55 | Fade to: Clean logo with Unity Asset Store button mockup | Voice: "Mobile Terrain Forge. Available now on the Unity Asset Store. See the mathematics in action." | Call to action |
| 8:55-9:00 | Fade to black, piano chord resolves | Ambient music fades out | Clean, quiet ending |

---

## Timing Notes

- **Total Duration:** 9:00 (target: 8-10 minutes)
- **Pacing:** Allow 2-3 second pauses after major concepts
- **Transitions:** Use subtle fades or cuts, avoid flashy effects
- **Text Overlays:** Hold for 3-4 seconds minimum to ensure readability
- **Animations:** Keep them slow and deliberate — let the audience follow the logic

## Key Moments to Emphasize

1. **The Collapse (3:00-3:15):** This is the core WFC concept — make it visually striking but quiet
2. **Performance Comparison (5:15-6:15):** The concrete benefits — show, don't just tell
3. **Mobile Demo (7:15-7:30):** Connect to the target platform

## Audio Notes

- Background music: Minimal piano, never competing with voice
- Volume: Music at -20dB relative to narration
- Pacing: Narrator should speak at ~140 words per minute (conversational but measured)

## Notes for Video Editor

- All diagrams should use the same color scheme (muted blues, earth tones, charcoal)
- Code snippets should use a dark theme for readability
- Performance graphs should be clearly labeled with axes and values
- Text overlays should be large enough to read on mobile (60%+ of viewers)