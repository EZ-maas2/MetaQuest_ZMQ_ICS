# Checklist for a Unity project set-up
This is an abbreviated version olf the tutorial that is supposed to help with quickly seeing if you missed any essential step of a project setup

- [ ] Did you install Meta XR All-in-One SDK from the Unity Asset Store? https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269657
- [ ] Did you add Meta XR All-in-One SDK to your project via Window – Package Manager – My Assets - Meta XR All-in-One SDK?
- [ ] Did you install XR Plugin Management? Do it via Edit – Project Settings – XR Plugin Management  - Install XR Plugin Management. After it is installed, also select Android - Oculus. 
- [ ] Did you switch the platform to Android via File - Build Settings - Android - Switch Platform?
- [ ] Did you resolve all issues for Meta XR via Edit – Project Settings – Meta XR - Desktop AND Android - Fix all?
- [ ] Did you replace the Main Camera with OVRCameraRigInteraction? If not, go to the search bar, type OVRCameraRigInteraction and select Search: All. Drag the prefab to the Hierarchy.
- [ ] Did you configure OVR Manager?
If not, go to OVRCameraRigInteraction - OVRRig - OVR Manager (Script). Under Target devices select Quest 3. Under tracking Origin type select Eye level. Under Quest features, change Hand Tracking Support select Controllers and Hands or Hands only (depending on your project) and set Hand Tracking Frequency to HIGH.
Are you using Passthrough? Under Quest features, select Passthrough Support - Supported and Insight Passthrough - Enable Passthrough.


# ZMQ
- [ ]  Did you install NetMQ (C# port for ZMQ) from https://github.com/GlitchEnzo/NuGetForUnity/releases/ ? If not, then download the NuGetForUnity.VERSION.unitypackage into your game project folder. 
- [ ]  Did you import the installed package? Go to  Assets - Import package - Custom package - Select NuGetForUnity.VERSION.unitypackage - Restart the editor
- [ ]  Did you install NetMQ in your project? Go to NuGet - Search NetMQ - Select and Install
