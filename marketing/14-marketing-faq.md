# Marketing FAQ — For Buyers

## General Questions

### What is Mobile Terrain Forge?

Mobile Terrain Forge is a Unity terrain generation system that uses Wave Function Collapse (WFC) instead of traditional noise-based approaches. It generates consistent, deterministic, and highly optimized terrain specifically designed for mobile games.

### How is this different from other terrain generators?

Most terrain generators use Perlin or Simplex noise. While these produce smooth gradients, they have three fundamental problems:

1. **Non-determinism:** The same seed can produce different results across runs
2. **Inconsistency:** Adjacent chunks may not connect seamlessly
3. **Un-debuggability:** When something goes wrong, you're guessing

Mobile Terrain Forge uses Wave Function Collapse, which guarantees:
- Deterministic results (same seed, same terrain, every time)
- Guaranteed consistency (no seam issues)
- Debuggable generation (step-by-step tracing)
- Better performance (75% fewer draw calls, 62% less memory, 33% better FPS)

### What is Wave Function Collapse?

Wave Function Collapse is an algorithm from quantum computing, adapted for procedural generation. It works by:

1. **Defining Constraints** — Specifying what terrain types can exist where
2. **Tracking Entropy** — Monitoring remaining possibilities at each grid cell
3. **Collapsing Deterministically** — Resolving cells in order of least entropy
4. **Propagating Consistency** — Ensuring every neighbor respects the result

The result is not random. It is the only terrain that satisfies all constraints.

### Do I need to understand WFC to use this asset?

No. The Quick Start Guide gets you generating terrain in 5 minutes without understanding WFC. However, for advanced customization and debugging, understanding the basics is helpful. The included tutorials cover WFC concepts in depth.

---

## Performance Questions

### What are the performance gains?

Based on benchmarks on a mid-range mobile device (Snapdragon 665, 4GB RAM):

- **75% fewer draw calls** (186 → 47)
- **62% less memory usage** (142 MB → 54 MB)
- **33% better frame rates** (42 FPS → 56 FPS, stable 60)

*(Benchmark: 1024x1024 terrain, 4 texture layers, 4 LOD levels)*

### Will this run on my target device?

Mobile Terrain Forge is optimized for devices with:
- Android: OpenGL ES 3.0+, Vulkan 1.0+
- iOS: Metal support
- RAM: 3GB minimum recommended
- GPU: Adreno 530 / Mali-G71 / Apple A9 or later

Download the free demo to test on your specific hardware before purchasing.

### Can I use this on desktop games?

Yes. While Mobile Terrain Forge is optimized for mobile, it works on Windows, macOS, and Linux as well. The performance gains are still significant, though less dramatic on high-end hardware.

### How does this compare to Unity's built-in terrain system?

Unity's built-in terrain system is general-purpose and not optimized for mobile. Mobile Terrain Forge is specifically designed for mobile constraints:

- **Draw Calls:** Unity terrain can generate hundreds of draw calls; Mobile Terrain Forge combines into 1-10
- **Memory:** Unity terrain loads all chunks at once; Mobile Terrain Forge uses memory pooling and streaming
- **Determinism:** Unity terrain uses noise (non-deterministic); Mobile Terrain Forge uses WFC (deterministic)
- **Customization:** Unity terrain has limited procedural control; Mobile Terrain Forge has full constraint customization

---

## Technical Questions

### What Unity versions are supported?

Unity 2021.3 LTS or later. Tested on 2021.3, 2022.3, and 2023.1.

### Does this work with URP/HDRP?

Yes. Mobile Terrain Forge works with Built-in Render Pipeline, URP (Universal Render Pipeline), and HDRP (High Definition Render Pipeline).

### Can I use my own shaders?

Yes. The terrain material system supports custom shaders. The splatmap-based texture blending is shader-agnostic.

### Does this support 3D terrain (caves, overhangs)?

Currently, Mobile Terrain Forge supports 2D terrain generation. 3D terrain support is planned for v1.1.0.

### How large can the terrain be?

Mobile Terrain Forge can generate terrains of any size, limited only by your device's memory. The chunk-based architecture supports streaming for infinite terrains.

### Can I customize the terrain types?

Yes. You can define custom terrain types (grass, water, rock, forest, desert, etc.) and configure their constraints, textures, and biome rules.

### Does this support biomes?

Yes. Biome transitions are handled through the constraint system. You can define elevation-based, moisture-based, or custom biome rules.

### Can I save and load generated terrain?

Yes. The terrain mesh and texture data can be serialized and saved. You can also save the seed and regenerate deterministically.

---

## Licensing & Pricing

### What does the license include?

- Commercial use allowed
- Unlimited projects
- No attribution required
- Full source code included
- Free updates for 12 months
- Lifetime access to purchased version

### Is this a subscription?

No. This is a one-time purchase of $59.99. You own the asset forever.

### What is the refund policy?

30-day money-back guarantee if the asset doesn't work as described or if you're not satisfied.

### Can I use this in client projects?

Yes. The license allows unlimited commercial projects, including client work.

### Do I need to credit Mobile Terrain Forge?

No attribution is required. You can use the asset freely without credits.

### What happens after 12 months?

After 12 months, you still have lifetime access to the version you purchased. You can continue using it in all your projects. Updates beyond 12 months may require an upgrade fee, but this is at our discretion.

---

## Support & Documentation

### What documentation is included?

- **Quick Start Guide:** Get terrain generating in 5 minutes
- **API Reference:** Full documentation of all classes and methods
- **Tutorial: Building Your First Terrain:** 15-minute walkthrough
- **Tutorial: Custom Constraints:** 20-minute advanced guide
- **Performance Optimization Guide:** Best practices for mobile
- **FAQ:** This document

### Is there a video tutorial series?

Yes. "The Quiet Craft of Optimization" is a 7-episode YouTube series covering:
1. Why WFC? The Mathematics of Consistency
2. Mobile Optimization: Where Draw Calls Go to Die
3. The Hidden Cost of Noise
4. Determinism in Procedural Generation
5. Memory-efficient Terrain Streaming
6. Building a Custom Terrain System
7. Case Study: 60fps on a Budget Device

Link included with purchase.

### How do I get support?

- **Email:** Response within 48 hours
- **Unity Forum:** Community support thread
- **Discord:** Invite included with purchase

### Is the source code included?

Yes. Full C# source code is included. No obfuscated DLLs. You can modify, extend, and learn from the code.

---

## Comparison Questions

### Why not just use Perlin noise?

Perlin noise is great for many things, but it has fundamental limitations for terrain generation:

1. **Non-deterministic:** Same seed can produce different results
2. **No constraints:** Can't enforce "forest must be adjacent to grass"
3. **Inconsistent results:** Adjacent chunks may not connect
4. **Hard to debug:** When terrain looks wrong, you're guessing

WFC addresses all of these issues.

### Is this better than Gaia / Terrain Composer?

Those assets are excellent for what they do — artistic terrain sculpting and multi-biome management. Mobile Terrain Forge focuses on:

1. **Determinism:** Guarantees reproducible results
2. **Mobile Optimization:** Specifically tuned for mobile constraints
3. **Procedural Generation:** Algorithm-based, not hand-sculpted

They can be complementary: Use Mobile Terrain Forge for base terrain generation, then use Gaia for artistic refinement.

### How does this compare to MapMagic?

MapMagic is a powerful node-based terrain generator. Mobile Terrain Forge is simpler, more focused, and optimized for:

1. **Deterministic results** (MapMagic can be non-deterministic depending on nodes)
2. **Mobile performance** (fewer draw calls, less memory)
3. **Debuggability** (step-by-step WFC tracing)

If you need complex node-based workflows, MapMagic may be better. If you need deterministic, optimized terrain for mobile, Mobile Terrain Forge is the choice.

---

## Integration Questions

### How hard is it to integrate into an existing project?

Very easy. Import the asset, add the Terrain Generator component to a GameObject, set your seed, and call `Generate()`. The Quick Start Guide walks you through this in 5 minutes.

### Can I use this with my existing terrain data?

Yes. You can import heightmaps, splatmaps, or other terrain data and use Mobile Terrain Forge to generate or modify regions. The API supports both full generation and incremental updates.

### Does this work with other assets?

Yes. Mobile Terrain Forge generates standard Unity meshes and materials. These work with any asset that consumes meshes (colliders, navmeshes, vegetation systems, etc.).

### Can I modify the generation at runtime?

Yes. The API supports runtime generation, modification, and regeneration. You can change constraints, seed, or parameters and regenerate dynamically.

---

## Learning Curve Questions

### How long does it take to learn?

- **Basic Usage:** 5 minutes (Quick Start Guide)
- **Intermediate Customization:** 30-60 minutes (tutorials)
- **Advanced Customization:** 2-4 hours (API reference + experimentation)

### Do I need to be a programmer?

Basic usage requires no programming (just configure in the Unity Editor). Advanced customization (custom constraints, runtime API) requires some C# knowledge, but the documentation and examples make it accessible.

### Is there a learning community?

Yes. Join the Discord server (invite included) or the Unity Forum thread to connect with other users, share projects, and get help.

---

## Future Development Questions

### What features are planned?

Planned for v1.1.0:
- 3D terrain support (cave systems, overhangs)
- Additional biome types (desert, tundra, swamp)
- Performance profiling improvements
- Additional demo scenes

### Will I get updates?

Yes. Free updates for 12 months after purchase. After that, updates are at our discretion but we strive to support our customers.

### Can I request features?

Yes. Feature requests are welcome through the support channels. We prioritize features based on community demand and technical feasibility.

---

## Demo & Trial Questions

### Is there a free demo?

Yes. Download the free demo from the Asset Store:
- Full WFC algorithm (time-limited generation)
- 2 example scenes
- Complete documentation
- Test on your target hardware

### What's limited in the demo?

The demo has a time limit on terrain generation (to prevent commercial use without purchase). Otherwise, it's fully functional.

### Can I use the demo in a released game?

No. The demo is for evaluation only. Purchase the full asset for commercial use.

---

## Miscellaneous Questions

### Who is this for?

- Mobile game developers targeting 60fps
- Technical artists needing consistent terrain
- Programmers building procedural tools
- Developers who value determinism over "good enough"

### Who is this NOT for?

- Desktop-first games with unlimited hardware (noise may suffice)
- Games requiring completely random, non-deterministic terrain
- Projects needing extreme artistic control (hand-sculpted may be better)
- Developers unwilling to learn WFC concepts

### What if I'm not satisfied?

We offer a 30-day money-back guarantee. If the asset doesn't work as described or you're not satisfied, contact support for a refund.

### Can I share this with my team?

Yes. The license allows use within your team or organization. One purchase covers unlimited projects within your organization.

---

## Still Have Questions?

- **Check the Documentation:** Included with the asset
- **Watch the YouTube Series:** "The Quiet Craft of Optimization"
- **Join the Discord:** Community support and discussion
- **Post on the Unity Forum:** Get help from the community
- **Email Support:** [support email]

We're here to help you generate better terrain.

---

**Last Updated:** Week 3, 2026
**Version:** 1.0.0