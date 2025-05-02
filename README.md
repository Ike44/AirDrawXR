# AirDrawXR

AirDrawXR is a Unity-based augmented reality (AR) application for gesture-based 3D drawing using hand tracking. This project showcases two demo scenes built using Unity’s AR Foundation and MediaPipe Hands (via the Homuler Unity plugin).

## Requirements

- **Unity Editor**: 6000.0.41f1 (Apple Silicon-compatible, LTS)  
- **Xcode**: 14.3 or later (with iOS 16+ SDK)  
- **AR-capable iOS device** (for Demo1)  
- **Apple Developer account** and device provisioning  
- **Packages**:
  - AR Foundation 5.1.3 or later  
  - ARKit XR Plugin  
  - Homuler’s MediaPipe Unity Plugin  
    – https://github.com/homuler/MediaPipeUnityPlugin

---

## Demo Scenes

PATH: Assets/Scenes/

### Demo1: AR Drawing Scene  
3D drawing in real-world space using AR Foundation and hand gestures. Requires an ARKit-capable iOS device.

### Demo2: Hand Gesture Drawing (Editor Mode)  
Detects hand landmarks via the Homuler MediaPipe Unity plugin. Run in the Unity Editor without deploying to a device.

---

## Building to iOS

### Here is a Google Drive link to a built project. Just download the folder as a zip, extract, run xcodeproj file and then sign the build as explained below and run the project.
### https://drive.google.com/drive/folders/11tmVBEcCXhbHU8OIOD0kTI0ulvLMJT6F?usp=sharing 



Follow these steps to build and run **Demo1** on your iOS device:

1. **Install & Configure Unity**  
   - Open Unity Hub, install version **6000.0.41f1** (Apple Silicon).  
   - Create or open the AirDrawXR project.  
   - In **Package Manager**, ensure AR Foundation **5.1.3+**, ARKit XR Plugin, and MediaPipeUnityPlugin are installed.

2. **Switch Build Target**  
   - Go to **File > Build Settings**.  
   - Select **iOS** and click **Switch Platform**.  
   - In **Player Settings > Other Settings**, set:
     - **Scripting Backend**: IL2CPP  
     - **Architecture**: ARM64  
     - **Bundle Identifier**: e.g. `com.yourcompany.airdrawxr`  
     - **Camera Usage Description**: “This app requires camera access for AR drawing.”  

3. **Enable XR Plugins**  
   - In **Project Settings > XR Plug-in Management > iOS**:
     - Check **ARKit**.  
     - Under **Subsystems**, enable **ARKit Face Tracking** and **ARKit Plane Detection** if needed.

4. **Configure Signing & Capabilities**  
   - In **Project Settings > Player > iOS**:
     - Under **Identification**, select your **Team**.  
     - Ensure **Automatic Signing** is enabled.  

5. **Prepare Scenes & Assets**  
   - In **Build Settings > Scenes In Build**, add:
     - `Assets/Scenes/Demo1.unity`  
   - Confirm the **ARCamera** in the scene uses the **ARCamera Background** component.

6. **Build Xcode Project**  
   - In **Build Settings**, set **Development Build** (optional) and click **Build**.  
   - Choose an output folder and wait for Unity to generate the Xcode project.

7. **Open & Configure in Xcode**  
   - Launch the generated `.xcworkspace` in Xcode.  
   - Select your connected device as the build target.  
   - In **Signing & Capabilities**, verify your Team and provisioning profiles.  
   - Under **Build Settings**, ensure **iOS Deployment Target** ≥ 14.0.

8. **Run on Device**  
   - Click **Run** (▶) in Xcode to install and launch the app on your device.  
   - Grant camera and motion permissions when prompted.

9. **Troubleshooting**  
   - If provisioning errors appear, verify your Apple Developer portal certificates and profiles.  
   - Ensure device is in **Developer Mode** (Settings > Privacy & Security).  
   - Clean the build folder in Xcode (Product > Clean Build Folder) before rebuilding.

---

## Helpful Links

- **Unity → iOS AR Build Tutorial**  
  [https://www.youtube.com/watch?v=8xvLjh3Xn18](https://www.youtube.com/watch?v=BV7H8jQPfYk)

- **Homuler MediaPipe Unity Plugin**  
  https://github.com/homuler/MediaPipeUnityPlugin 

---

## Project Notes

- Development done on **Apple Silicon Mac** using Unity **6000.0.41f1**.  
- Tested with iOS 16+ on iPhone 13 and newer.  
- For **Demo2**, open `Demo2.unity` and run in Play Mode inside the Unity Editor.

---

Feel free to fork or contribute—pull requests are welcome!
