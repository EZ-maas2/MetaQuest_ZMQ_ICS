MetaQuest + Unity + ZeroMQ 

 

## Install everything: 

Install MetaQuest Developer Hub and Oculus (Meta Quest Link) 

Install Unity Hub (https://unity.com/games?utm_source=google&utm_medium=cpc&utm_campaign=cc_dd_upr_emea_emea-t2_en_aw_sem-gg_acq_nb-pr_2023-11_cc-dd-solutions-emea-t2-nb_cc3022_ev-nb_id:71700000115996773&utm_content=cc_dd_upr_emea_aw_sem_gg_ev-br_pros_x_npd_cpc_kw_sd_all_x_x_game-develolpment_id:58700008606778974&utm_term=game%20development%20software&&&&&gad_source=1&gclid=CjwKCAjwgdayBhBQEiwAXhMxtleat1bob7-2pmd7BiDLxmATifpBxzg-8Xcr_7dJT7ayoZD6RMdqSBoCh5gQAvD_BwE&gclsrc=aw.ds) 

 

## Install NuGet (ZeroMQ for Unity) from https://github.com/GlitchEnzo/NuGetForUnity/releases/ 

Install ZeroMQ for Python 

Cmd: pip install pyzmq 

Or from : https://zeromq.org/languages/python/ 

Tutorial on how to install ZeroMQ for Unity: https://vinnik-dmitry07.medium.com/a-python-unity-interface-with-zeromq-12720d6b7288 

 

## Make your first Unity AR game: 

When you touch the cube, your Python script will play you a sound.  

One script that waits for the object interaction, one script that runs a communication thread, one script that makes a new object when the previous one is destroyed.  

Open Unity Hub (Install Android Build Support) 

 

From Unity Asset Store Install Meta XR All-in-One SDK 

https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269657 
![]()

 

 

Press Open in Unity. Install in the Package Manager. Restart Unity Editor if prompted. 

 

Alternatively, 

Go to Window – Package Manager – My Assets 

Install Meta XR All-in-One SDK 

Restart Editor if prompted 

 

 

Go to File – Build Settings - Under Platform select Android – Press Switch Platform 

 

 

Go to Edit – Project Settings – XR Plugin Management  - Install XR Plugin Managment 

 

 

 

The actual Game  

In the Hierarchy window, you can see your Game Objects.  

Install OVRCameraRigInteraction  

Delete Main Camera – It must be replaced by an OVRCameraRigInteraction. Let’s call it Rig for brevity. The rig object holds information about the position of your headset, controllers and/or hands. 

A screenshot of a computer

Description automatically generated 

 

A screenshot of a computer program

Description automatically generated 

 

## Let’s make a coin! 

In Hierarchy, right-click and select 3D object – cylinder. In the Transform attribute, modify the position, the rotation and the size of the cylinder such that it resembles a coin. For example like this: 

Set up a Collider object. Collider objects are the important attributes of Game Objects that allow different objects to interact with each other.
Important! There is supposed to be a default capsule collider object attached to your coin object when you create it. Delete it and replace it with a box collider. Do that by clicking Add Component and searching Box Collider. Unfortunately, it will not be perfectly aligned with the coin object because Unity has no cylinder collider objects. 
We need this collider  to detect when the Hand object collides with the coin.

In order to make the coin react to the collision, we need to write a C# script and attach it to the coin. 
We are going to use Unity's inbuilt way to handle collisions. We will use the OnTriggerEnter function so that when the Box Collider attached to the coin collides with something, it calls this function.
The function is going to check what the Box Collider collided with (we will get to it in a second), record it in a static class TouchCoinStatic, and delete the coin object.
![](![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/4631681e-53ea-466f-8702-a5e38974dea3))


