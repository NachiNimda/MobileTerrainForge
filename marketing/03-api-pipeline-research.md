# API Pipeline Research: Canva & ElevenLabs

## Overview

This document outlines the integration strategy for using Canva and ElevenLabs in the video production pipeline for "The Quiet Craft of Optimization" series.

## ElevenLabs — Voiceover Generation

### Purpose
Generate professional, consistent narration for all episodes without recording equipment or voice talent scheduling.

### API Integration

**Authentication:**
```bash
# Get API key from ElevenLabs dashboard
export ELEVENLABS_API_KEY="your_api_key_here"
```

**Recommended Voice Model:**
- Model: `eleven_multilingual_v2` or `eleven_turbo_v2_5` (faster, good quality)
- Voice Profile: Custom trained voice (warm, authoritative, unhurried — ~45 years old, slightly educated tone)
- Settings: Stability 0.75, Similarity 0.85

**API Workflow:**

1. **Generate Voice Sample:**
```bash
curl -X POST "https://api.elevenlabs.io/v1/text-to-speech/{voice_id}" \
  -H "xi-api-key: $ELEVENLABS_API_KEY" \
  -H "Content-Type: application/json" \
  -d '{
    "text": "This is a sample of the narration style for Mobile Terrain Forge.",
    "model_id": "eleven_multilingual_v2",
    "voice_settings": {
      "stability": 0.75,
      "similarity_boost": 0.85
    }
  }' --output sample.mp3
```

2. **Generate Full Script:**
- Split script into logical sections (intro, concept, demo, outro)
- Generate each section separately for easier editing
- Add 1-2 second pauses between sections during assembly

**Pros:**
- Consistent voice across all episodes
- No recording equipment needed
- Easy to regenerate sections if script changes
- API rate: ~50,000 characters/month on free tier

**Cons:**
- Limited emotional range compared to human voice actor
- Requires careful script writing to sound natural
- May need trial-and-error to find perfect voice settings

**Cost Estimate:**
- Free tier: 10,000 characters/month (sufficient for ~8-10 minute episode)
- Starter tier ($5/mo): 30,000 characters/month
- Creator tier ($22/mo): 100,000 characters/month

**Recommendation:** Start with free tier, upgrade if voice quality or character limit becomes limiting.

## Canva — Visual Asset Creation

### Purpose
Create professional diagrams, charts, and overlays without Adobe Creative Suite.

### API Integration

**Authentication:**
```bash
# Get API key from Canva Developers portal
export CANVA_API_KEY="your_api_key_here"
```

**Use Cases:**

1. **Diagram Templates:**
- Create reusable diagram templates (grids, charts, flowcharts)
- Store in Canva Design Library
- Export as SVG or PNG for video editing

2. **Chart Generation:**
- Use Canva's chart builder for performance metrics
- Style charts to match series palette
- Export as high-resolution PNG or SVG

3. **Thumbnail Creation:**
- Build thumbnail templates with consistent branding
- Update text overlay for each episode
- Export at 1280x720

**Alternative Approach (Recommended):**
Given Canva's API limitations for programmatic design generation, use Canva as a **visual design tool** rather than direct API integration:

**Workflow:**

1. **Manual Design Phase:**
- Design all static diagrams in Canva web interface
- Use consistent brand colors and typography
- Save as templates for reuse across episodes

2. **Export Phase:**
- Export diagrams as SVG (preferred) or PNG
- Organize in project asset folders
- Import into video editing software

3. **Script Automation (Optional):**
- Use Canva CLI or unofficial APIs if batch generation is needed
- For simple text updates, manual editing in Canva is faster

**Pros:**
- Professional design quality without learning complex tools
- Brand consistency through templates
- Free tier is generous (Canva Pro not strictly necessary)
- Export options include SVG (scalable)

**Cons:**
- API limited compared to full design automation
- Manual process for each new diagram
- Limited animation capabilities compared to After Effects

**Cost Estimate:**
- Free tier: Sufficient for all needs
- Pro tier ($12.99/mo): Brand Kit, advanced export options, background remover

**Recommendation:** Use free tier. Upgrade to Pro only if Brand Kit or specific Pro features become essential.

## Alternative Tools Considered

### Audio Alternatives
- **Amazon Polly:** More robotic, cheaper ($4/1M chars)
- **Google Cloud TTS:** Natural voices, pricing similar to ElevenLabs
- **OpenAI TTS:** High quality but $15/1M chars (expensive for series)
- **Local TTS (espeak, festival):** Free but quality not professional

### Visual Alternatives
- **Figma:** Better for team collaboration, similar capabilities
- **Adobe Express:** More templates, subscription model
- **Inkscape:** Free/open-source SVG editor, steeper learning curve
- **Manim (Python):** Mathematical animation library, requires coding

## Production Pipeline Integration

### Phase 1: Script & Voice
```
Script Markdown → ElevenLabs API → Audio Files (WAV)
```

### Phase 2: Visual Design
```
Concept Sketch → Canva Design → Export SVG/PNG → Visual Assets
```

### Phase 3: Animation
```
Visual Assets → Video Editor (DaVinci Resolve / Premiere) → Animations
```

### Phase 4: Assembly
```
Audio + Visuals → Sync/Edit → Export → Upload
```

## Technical Considerations

### File Formats
- **Audio:** WAV or AIFF (lossless) for editing, compress for final export
- **Visuals:** SVG for diagrams (scalable), PNG for raster assets
- **Video:** H.264 for compatibility, H.265 for space efficiency

### Version Control
- Store scripts in Git
- Keep generated audio/visual assets in organized folders with version suffixes
- Document voice settings and Canva template names in project README

### Backup Strategy
- ElevenLabs: Generated audio only available from API — download immediately
- Canva: Designs stored in Canva account; export source files (.canva)
- Video project files: Back up to cloud storage (Google Drive, Dropbox)

## Next Steps

1. Set up ElevenLabs account and test voice generation
2. Create Canva account and design initial diagram templates
3. Produce sample assets for Episode 1 test render
4. Refine workflow based on first production cycle
5. Document lessons learned for Episode 2 production