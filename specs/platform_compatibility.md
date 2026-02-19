# Platform Compatibility Notes

**Version:** 1.0
**Last Updated:** 2026-02-19

---

## Overview

Mobile Terrain Forge supports multiple platforms with varying optimization strategies. This document outlines platform-specific requirements, configurations, and known limitations.

---

## Supported Platforms

| Platform | Status | Minimum Requirements | Recommended Requirements |
|----------|--------|---------------------|-------------------------|
| Android | ✅ Full | Android 7.0 (API 24) | Android 11 (API 30+) |
| iOS | ✅ Full | iOS 12.0 | iOS 15+ |
| Windows | ✅ Full | Windows 10 | Windows 11 |
| macOS | ✅ Full | macOS 10.15 | macOS 13+ |
| Linux | ⚠️ Experimental | Ubuntu 18.04 | Ubuntu 22.04 |
| WebGL | ⚠️ Limited | Modern browser | Chrome/Firefox 2024+ |

---

## Android

### Requirements

**Minimum Device Specs:**
- CPU: Quad-core 1.8 GHz (ARMv8)
- RAM: 2GB
- GPU: OpenGL ES 3.0 support
- Storage: 50MB additional space

**Recommended Device Specs:**
- CPU: Octa-core 2.4 GHz+ (Snapdragon 8 Gen 2/3, Tensor G2/G3)
- RAM: 4GB+
- GPU: OpenGL ES 3.2+, Vulkan support
- Storage: 100MB additional space

### Unity Configuration

**Player Settings:**

```
Player Settings > Other Settings:
├── Graphics APIs: [Vulkan, OpenGLES3.1+]  (Remove OpenGLES2.0)
├── Scripting Backend: IL2CPP
├── API Compatibility Level: .NET Standard 2.1
├── Target Architectures: ARM64
├── Multithreaded Rendering: Enabled
├── Static Batching: Enabled
└── Dynamic Batching: Enabled
```

**Android Manifest:**

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android"
    package="com.yourcompany.game">

    <uses-permission android:name="android.permission.INTERNET" />

    <uses-feature android:glEsVersion="0x00030001" android:required="true" />
    <uses-feature android:name="android.hardware.touchscreen" android:required="false" />

    <uses-sdk
        android:minSdkVersion="24"
        android:targetSdkVersion="33" />

    <application
        android:allowBackup="true"
        android:icon="@mipmap/app_icon"
        android:label="@string/app_name"
        android:theme="@style/UnityThemeSelector">

        <activity
            android:name="com.unity3d.player.UnityPlayerActivity"
            android:configChanges="mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|orientation|screenLayout|uiMode|screenSize|smallestScreenSize|fontScale|layoutDirection|density"
            android:hardwareAccelerated="true">
            <intent-filter>
                <action android:name="android.intent.action.MAIN" />
                <category android:name="android.intent.category.LAUNCHER" />
            </intent-filter>
        </activity>

        <meta-data android:name="unity.build-id" android:value="..." />
    </application>
</manifest>
```

### Texture Settings

**Compression Format:**
- Primary: ASTC 6x6 (recommended)
- Alternative: ETC2 (legacy fallback)
- Avoid: PVRTC (iOS only), DXT (desktop only)

**Platform Overrides:**

```
Texture Import Settings > Android:
├── Format Override: ASTC 6x6
├── Compression: Normal Quality
├── Max Size: 1024 (textures), 512 (splatmaps)
└── Generate Mipmaps: Enabled
```

### Performance Tuning

**Optimized LOD for Android:**

```csharp
// Conservative LOD settings for varied Android hardware
LODLevel[] androidLOD = new LODLevel[]
{
    new LODLevel
    {
        level = 0,
        switchDistance = 10,  // Closer switch than iOS
        meshResolution = 17,  // Lower resolution
        textureResolution = 0.5f,
        enableCulling = false
    },
    new LODLevel
    {
        level = 1,
        switchDistance = 25,
        meshResolution = 9,
        textureResolution = 0.25f,
        enableCulling = true
    },
    new LODLevel
    {
        level = 2,
        switchDistance = 50,
        meshResolution = 5,
        textureResolution = 0.125f,
        enableCulling = true
    }
};
```

**Quality Settings:**

```csharp
// Apply mobile-optimized quality settings
QualitySettings.SetQualityLevel(0);  // Fastest

QualitySettings.pixelLightCount = 0;
QualitySettings.shadowDistance = 0;
QualitySettings.shadowCascades = 0;
QualitySettings.shadowResolution = ShadowResolution.Low;
QualitySettings.shadowProjection = ShadowProjection.CloseFit;

QualitySettings.antiAliasing = 0;
QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
QualitySettings.softParticles = false;
QualitySettings.softVegetation = false;

QualitySettings.realtimeReflectionProbes = false;
QualitySettings.reflectionProbeBoxProjection = false;
```

### Known Issues

**Issue:** ASTC not supported on older Android versions
- **Affected:** Android 7.0-7.1 (API 24-25)
- **Solution:** Fallback to ETC2 or runtime detection

**Issue:** Performance variance across devices
- **Affected:** Low-end Android devices
- **Solution:** Use dynamic quality adjustment

**Issue:** Vulkan compatibility issues
- **Affected:** Some Android 7-8 devices
- **Solution:** Prefer OpenGL ES 3.1 as primary API

### Device-Specific Notes

| Device Category | Recommended Settings | Expected FPS |
|----------------|---------------------|--------------|
| High-end (S24, Pixel 8) | Full quality | 60+ |
| Mid-range (Pixel 7, Galaxy A54) | Medium LOD | 55-60 |
| Low-end (budget phones) | Low LOD, reduced textures | 40-50 |

---

## iOS

### Requirements

**Minimum Device Specs:**
- iPhone 6s or later
- iPad Air 2 or later
- iOS 12.0+
- Metal support required

**Recommended Device Specs:**
- iPhone 13 or later
- iPad Pro 2021 or later
- iOS 15+
- A14 Bionic or later

### Unity Configuration

**Player Settings:**

```
Player Settings > iOS Settings:
├── Graphics API: Metal (required)
├── Scripting Backend: IL2CPP
├── API Compatibility Level: .NET Standard 2.1
├── Target Device: iPhone + iPad
├── Architecture: ARM64
├── Camera Usage Description: [Not required]
└── Target Minimum iOS Version: 12.0
```

**Info.plist Configuration:**

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleDevelopmentRegion</key>
    <string>$(DEVELOPMENT_LANGUAGE)</string>

    <key>CFBundleDisplayName</key>
    <string>Your Game</string>

    <key>CFBundleExecutable</key>
    <string>$(EXECUTABLE_NAME)</string>

    <key>CFBundleIdentifier</key>
    <string>com.yourcompany.game</string>

    <key>CFBundleInfoDictionaryVersion</key>
    <string>6.0</string>

    <key>CFBundleName</key>
    <string>$(PRODUCT_NAME)</string>

    <key>CFBundlePackageType</key>
    <string>APPL</string>

    <key>CFBundleShortVersionString</key>
    <string>1.0</string>

    <key>CFBundleVersion</key>
    <string>1</string>

    <key>LSRequiresIPhoneOS</key>
    <true/>

    <key>UIRequiredDeviceCapabilities</key>
    <array>
        <string>arm64</string>
        <string>metal</string>
    </array>

    <key>UISupportedInterfaceOrientations</key>
    <array>
        <string>UIInterfaceOrientationPortrait</string>
        <string>UIInterfaceOrientationLandscapeLeft</string>
        <string>UIInterfaceOrientationLandscapeRight</string>
    </array>

    <key>UIInterfaceOrientation</key>
    <string>UIInterfaceOrientationLandscapeRight</string>
</dict>
</plist>
```

### Texture Settings

**Compression Format:**
- Primary: ASTC 6x6 (recommended for iOS 11+)
- Alternative: PVRTC 4bpp (fallback for older devices)
- Avoid: ETC2 (not supported on iOS)

**Platform Overrides:**

```
Texture Import Settings > iOS:
├── Format Override: ASTC 6x6
├── Compression: Normal Quality
├── Max Size: 1024 (textures), 512 (splatmaps)
└── Generate Mipmaps: Enabled
```

### Performance Tuning

**Optimized LOD for iOS:**

```csharp
// Higher quality settings for iOS
LODLevel[] iosLOD = new LODLevel[]
{
    new LODLevel
    {
        level = 0,
        switchDistance = 15,  // Further than Android
        meshResolution = 25,  // Higher resolution
        textureResolution = 0.75f,
        enableCulling = false
    },
    new LODLevel
    {
        level = 1,
        switchDistance = 40,
        meshResolution = 13,
        textureResolution = 0.5f,
        enableCulling = true
    },
    new LODLevel
    {
        level = 2,
        switchDistance = 80,
        meshResolution = 7,
        textureResolution = 0.25f,
        enableCulling = true
    },
    new LODLevel
    {
        level = 3,
        switchDistance = 150,
        meshResolution = 5,
        textureResolution = 0.125f,
        enableCulling = true
    }
};
```

**Metal Shader Optimizations:**

```csharp
// Metal-specific optimizations
void ConfigureForMetal()
{
    // Enable Metal frame debugger in development
    #if UNITY_EDITOR
    UnityEditor.MetalFrameCapture.enabled = true;
    #endif

    // Use Metal API validation for debugging
    // Disable in production builds
}
```

### Known Issues

**Issue:** ASTC format not supported on iOS 10 and earlier
- **Affected:** iOS 10 and older devices
- **Solution:** Use PVRTC 4bpp as fallback

**Issue:** PVRTC artifacts on low-contrast textures
- **Affected:** Older iOS devices using PVRTC
- **Solution:** Increase texture quality or use ASTC when available

**Issue:** Metal shader compilation fails on some devices
- **Affected:** iPad Pro 2017 and similar
- **Solution:** Update iOS version or use fallback shaders

### Device-Specific Notes

| Device Category | Recommended Settings | Expected FPS |
|----------------|---------------------|--------------|
| High-end (iPhone 15 Pro, iPad Pro M2) | Ultra quality | 60+ |
| Mid-range (iPhone 13, iPad Air 5) | High quality | 60 |
| Low-end (iPhone 8, iPad Air 2) | Medium quality | 45-55 |

---

## Windows

### Requirements

**Minimum Device Specs:**
- OS: Windows 10 (version 1809)
- CPU: Quad-core 2.0 GHz
- RAM: 4GB
- GPU: DirectX 11 support

**Recommended Device Specs:**
- OS: Windows 11
- CPU: Hexa-core 3.0 GHz+
- RAM: 8GB+
- GPU: DirectX 12 support

### Unity Configuration

**Player Settings:**

```
Player Settings > PC, Mac & Linux Standalone:
├── Graphics API: [Direct3D12, Direct3D11]  (D3D12 preferred)
├── Scripting Backend: IL2CPP
├── API Compatibility Level: .NET Standard 2.1
├── Architecture: x64
└── Target Platform: Windows
```

### Texture Settings

**Compression Format:**
- Primary: BC7 (high quality)
- Alternative: DXT5 (legacy)

```
Texture Import Settings > Standalone:
├── Format Override: DXT5
├── Compression: Normal Quality
├── Max Size: 2048 (textures), 1024 (splatmaps)
└── Generate Mipmaps: Enabled
```

### Performance Tuning

**Optimized LOD for Windows:**

```csharp
// High quality settings for desktop
LODLevel[] windowsLOD = new LODLevel[]
{
    new LODLevel
    {
        level = 0,
        switchDistance = 20,
        meshResolution = 33,  // Full resolution
        textureResolution = 1.0f,
        enableCulling = false
    },
    new LODLevel
    {
        level = 1,
        switchDistance = 50,
        meshResolution = 17,
        textureResolution = 0.5f,
        enableCulling = true
    },
    new LODLevel
    {
        level = 2,
        switchDistance = 100,
        meshResolution = 9,
        textureResolution = 0.25f,
        enableCulling = true
    }
};
```

---

## macOS

### Requirements

**Minimum Device Specs:**
- OS: macOS 10.15 (Catalina)
- CPU: Intel Core i5 or Apple M1
- RAM: 4GB
- GPU: Metal support

**Recommended Device Specs:**
- OS: macOS 13 (Ventura)
- CPU: Apple M2 or Intel i7+
- RAM: 8GB+
- GPU: Metal 2 support

### Unity Configuration

**Player Settings:**

```
Player Settings > PC, Mac & Linux Standalone:
├── Graphics API: [Metal]
├── Scripting Backend: IL2CPP
├── API Compatibility Level: .NET Standard 2.1
├── Architecture: Apple Silicon + Intel (Universal)
└── Target Platform: macOS
```

### Texture Settings

**Compression Format:**
- Primary: ASTC 6x6 (Apple Silicon)
- Alternative: PVRTC (Intel)

---

## WebGL

### Limitations

WebGL support is experimental with the following limitations:

- **No LOD system:** Limited to single mesh resolution
- **No texture compression:** Uses PNG/JPEG (larger downloads)
- **Reduced terrain size:** Max 32x32 grid
- **No chunk streaming:** Single load only
- **Memory limited:** <50MB total

### Configuration

**Player Settings:**

```
Player Settings > WebGL:
├── Graphics API: WebGL 2.0
├── Scripting Backend: IL2CPP
├── API Compatibility Level: .NET Standard 2.1
├── Compression Format: Brotli
└── Memory Size: 256MB
```

**Recommended Settings:**

```csharp
// Simplified setup for WebGL
solver.gridWidth = 16;  // Smaller grid
solver.gridHeight = 16;
solver.enableRotations = false;  // Disable for performance
solver.chunkSize = 16;  // Single chunk
```

---

## Cross-Platform Code

### Platform Detection

```csharp
public class PlatformConfigurator : MonoBehaviour
{
    public LODManager lodManager;
    public SplatmapOptimizer splatmapOptimizer;

    void Start()
    {
        ConfigureForPlatform();
    }

    void ConfigureForPlatform()
    {
        RuntimePlatform platform = Application.platform;

        switch (platform)
        {
            case RuntimePlatform.Android:
                ConfigureForAndroid();
                break;

            case RuntimePlatform.IPhonePlayer:
                ConfigureForIOS();
                break;

            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                ConfigureForWindows();
                break;

            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                ConfigureForMacOS();
                break;

            case RuntimePlatform.WebGLPlayer:
                ConfigureForWebGL();
                break;

            default:
                ConfigureDefault();
                break;
        }
    }

    void ConfigureForAndroid()
    {
        // Android-specific settings
        splatmapOptimizer.SetCompressionFormat(TextureCompressionFormat.ASTC_6x6);
        lodManager.ConfigureLODLevels(GetAndroidLODLevels());
        QualitySettings.SetQualityLevel(0);
    }

    void ConfigureForIOS()
    {
        // iOS-specific settings
        splatmapOptimizer.SetCompressionFormat(TextureCompressionFormat.ASTC_6x6);
        lodManager.ConfigureLODLevels(GetIOSLODLevels());
        QualitySettings.SetQualityLevel(1);
    }

    void ConfigureForWindows()
    {
        // Windows-specific settings
        splatmapOptimizer.SetCompressionFormat(TextureCompressionFormat.BC7);
        lodManager.ConfigureLODLevels(GetDesktopLODLevels());
        QualitySettings.SetQualityLevel(3);
    }

    LODLevel[] GetAndroidLODLevels()
    {
        return new LODLevel[]
        {
            new LODLevel { level = 0, switchDistance = 10, meshResolution = 17, textureResolution = 0.5f },
            new LODLevel { level = 1, switchDistance = 25, meshResolution = 9, textureResolution = 0.25f },
            new LODLevel { level = 2, switchDistance = 50, meshResolution = 5, textureResolution = 0.125f }
        };
    }

    LODLevel[] GetIOSLODLevels()
    {
        return new LODLevel[]
        {
            new LODLevel { level = 0, switchDistance = 15, meshResolution = 25, textureResolution = 0.75f },
            new LODLevel { level = 1, switchDistance = 40, meshResolution = 13, textureResolution = 0.5f },
            new LODLevel { level = 2, switchDistance = 80, meshResolution = 7, textureResolution = 0.25f },
            new LODLevel { level = 3, switchDistance = 150, meshResolution = 5, textureResolution = 0.125f }
        };
    }

    LODLevel[] GetDesktopLODLevels()
    {
        return new LODLevel[]
        {
            new LODLevel { level = 0, switchDistance = 20, meshResolution = 33, textureResolution = 1.0f },
            new LODLevel { level = 1, switchDistance = 50, meshResolution = 17, textureResolution = 0.5f },
            new LODLevel { level = 2, switchDistance = 100, meshResolution = 9, textureResolution = 0.25f }
        };
    }
}
```

---

## Runtime Quality Adjustment

```csharp
public class QualityAdjuster : MonoBehaviour
{
    public LODManager lodManager;

    void Update()
    {
        float fps = 1.0f / Time.deltaTime;

        if (fps < 30)
        {
            ReduceQuality();
        }
        else if (fps > 58)
        {
            // Try to increase quality if headroom available
            IncreaseQuality();
        }
    }

    void ReduceQuality()
    {
        // Reduce LOD quality
        LODLevel[] levels = lodManager.GetLODLevels();
        foreach (var level in levels)
        {
            level.switchDistance *= 0.9f;
            level.meshResolution = Mathf.Max(5, level.meshResolution - 2);
        }
        lodManager.ConfigureLODLevels(levels);
    }

    void IncreaseQuality()
    {
        // Increase LOD quality
        LODLevel[] levels = lodManager.GetLODLevels();
        foreach (var level in levels)
        {
            level.switchDistance *= 1.05f;
            level.meshResolution = Mathf.Min(33, level.meshResolution + 2);
        }
        lodManager.ConfigureLODLevels(levels);
    }
}
```

---

## Summary

| Platform | Optimization Strategy | Target FPS | Memory Limit |
|----------|---------------------|------------|--------------|
| Android | Conservative LOD, ASTC compression | 55-60 | 15MB |
| iOS | High quality, Metal optimization | 60+ | 15MB |
| Windows | Maximum quality, D3D12 | 60+ | 50MB |
| macOS | High quality, Metal | 60+ | 30MB |
| WebGL | Simplified, no LOD | 30+ | 50MB |

---

**Copyright © 2026 Mobile Terrain Forge. All rights reserved.**
