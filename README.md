### Tutorial on combining MetaQuest with Unity and ZeroMQ 

## Game premise

Our game is going to be very simple.  Our laptop running Python code  will create a server, and our headset will connect to this server. We will have a coin. Once we touch the coin, it will disappear and signal to the Python side that the coin  was touched. Python will play a sound and send a response back to the headset. Once the headset gets the response from Python, it will create a new coin. 
We will have four C# scripts and one Python script. The Python script will host a server that waits for the request from a headset, plays  a sound when the request arrives, and sends a reply.
The first Python script will dtect when the coin is touched. The second Python script will communicate this to the thread responsible for sending requests to the server. The third script will be running the thread that sends information to the Python server. The fourth script will handle the response from the Python server.


## If you want to make it fully by yourself
If you would rather figure it out by yourself rather than going over  a premade project, here is some barebones advice to get you started:

1) This is  how to set the Unity project for MetaQuest: https://youtu.be/4kGD8q5kEx8?si=AF9NicktIufs01nO (until he starts using Interaction SDK)
2) This tutorial is for installing ZMQ and  setting up C# to Python communication: https://vinnik-dmitry07.medium.com/a-python-unity-interface-with-zeromq-12720d6b7288 . You might want to use the Request-Reply Pattern.
3) For the hand tracking, you will need OVRCameraRigInteraction. You can find game objects corresponding to the hands under OVRCameraRigInteraction > OVRCameraRig >  TrackingSpace.
4) You will need a Collider object on the object you are touching and another one attached to your hand. At least one of them should also have a RigidBody.
5) Use a separate thread in Unity to run the ZMQ communication; otherwise, it will crash.
6) Use events to communicate between parts of your code.

Good luck!


## Install everything: 

Install MetaQuest Developer Hub and Oculus (Meta Quest Link), login with ICS account details provided in Basecamp 

Install Unity Hub (https://unity.com/games?utm_source=google&utm_medium=cpc&utm_campaign=cc_dd_upr_emea_emea-t2_en_aw_sem-gg_acq_nb-pr_2023-11_cc-dd-solutions-emea-t2-nb_cc3022_ev-nb_id:71700000115996773&utm_content=cc_dd_upr_emea_aw_sem_gg_ev-br_pros_x_npd_cpc_kw_sd_all_x_x_game-develolpment_id:58700008606778974&utm_term=game%20development%20software&&&&&gad_source=1&gclid=CjwKCAjwgdayBhBQEiwAXhMxtleat1bob7-2pmd7BiDLxmATifpBxzg-8Xcr_7dJT7ayoZD6RMdqSBoCh5gQAvD_BwE&gclsrc=aw.ds) 

 

## Install NuGet (ZeroMQ for Unity) from https://github.com/GlitchEnzo/NuGetForUnity/releases/ 

Install ZeroMQ for Python 

Cmd: pip install pyzmq 

Or from: https://zeromq.org/languages/python/ 

Tutorial on how to install ZeroMQ for Unity: https://vinnik-dmitry07.medium.com/a-python-unity-interface-with-zeromq-12720d6b7288 

 





## Setup your Unity project
Open Unity Hub (Install Android Build Support) 

From Unity Asset Store Install Meta XR All-in-One SDK. Open it in Unit. Install it in the Package Manager. Restart Unity Editor if prompted. 

https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269657 
![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/3d630486-56ab-4f2d-98c8-fae1e6d56283)

  

Alternatively, if you have already installed it for a different project, you can Go to Window – Package Manager – My Assets. Install Meta XR All-in-One SDK and Restart Editor if prompted.

![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/2bede7c0-c2d0-480f-be9c-74ff81708863)

 

Go to File – Build Settings - Under Platform select Android – Press Switch Platform 
![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/8ae6065b-6a7d-45b8-8b99-a7e3e945819b)

 

Go to Edit – Project Settings – XR Plugin Management  - Install XR Plugin Managment 

![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/404b745e-880b-4a27-b3d5-d4048f5a3580)


The actual Game  

In the Hierarchy window, you can see your Game Objects.  
![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/ead3e903-facb-4db2-9df7-2da6ab737477)


Install OVRCameraRigInteraction  

Delete Main Camera – It must be replaced by an OVRCameraRigInteraction. The OVRCameraRigInteraction object holds information about the position of your headset, controllers and/or hands. 
Go to  OVRCameraRigInteractio-OVRCameraRig and configure OVRCameraRig in the Inspector window
Select Target device as Quest 3, Tracking Origin type  as Eye level, and Hand Tracking support as Hands only or Controllers and Hands
![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/bfb95638-2199-48bf-bcee-03f5efc2f6be)
![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/50c6eaea-406b-46f2-8fa6-ab63596b8015)

If you want Passthrough to be enabled, also select Passthrough support - Supported and check Enable passthrough

 
## Let’s make a coin! 

In Hierarchy, right-click and select 3D object – cylinder. In the Transform attribute, modify the position, the rotation and the size of the cylinder such that it resembles a coin. For example like this: 

![](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/707fe895-a76f-4dfe-9bb0-8927fa88bfc9)

Set up a Collider object. Collider objects are the important attributes of Game Objects that allow different objects to interact with each other.
Important! There is supposed to be a default capsule collider object attached to your coin object when you create it. Delete it and replace it with a Box Collider. Do that by clicking Add Component and searching Box Collider. Unfortunately, it will not be perfectly aligned with the coin object because Unity has no cylinder collider objects. 
We need this collider to detect when the Hand object collides with the coin.

To make the coin react to the collision, we must write a C# script and attach it to the coin Game Object. Let's call the script TouchCoin.
In our game, we will Destroy and Instantiate multiple coins. To make communicating with the Python side easier, we will also make a separate static class called  TouchCoinStatic.
It is not going to be attached to any game object. Its only job is tracking whether a coin was touched and communicating it to the Python code. 

To make sure that a Hand object can produce a collision, we must create an empty object under OVRCameraRigInteraction -  OVRCameraRig - TrackingSpace - LeftHandAnchor (or Right or both) - Create empty and name HandCollider and attach a sphere collider to it. 
Hand![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/7ac67321-8eb4-4634-89dc-9bc80b981b99)

Use the following parameters:
![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/e9e392f3-7be1-4462-b185-2f94091aef47)





Let's get back to the TouchCoin script. We are going to use Unity's in-built way to handle collisions. We will use the OnTriggerEnter function: when the Box Collider attached to the coin collides with something, it calls this function. The function will check what the Box Collider collided with, record the collision in a static class TouchCoinStatic, and delete the coin object.

![A screenshot of the TouchCoin script](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/blob/master/Images/TouchCoin.png)



So, our plan is as follows: when we touch the coin, this is recorded by the TouchCoinStatic and passed to the ZMQ  so that it can send this information to the Python side of the code.
Now, let's see how TouchCoinStatic looks like. 

![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/1e958412-a573-4f96-ba0b-00d29d4825da)

Once again, it is a static class; it can be modified from anywhere within the script, and it is not attached to any game objects.
To notify ZMQ, we will use events. It is a very convenient broadcast system. Whenever the OnTouchedCoin event is Invoked (line 23), other C# classes that subscribed to it get informed and react accordingly.
Thus, whenever anywhere in the code, the TouchCoinStatic.coinState  is set to true, the OnTouchedCoin event is invoked. In the next section, we will see how this is used to communicate with Python.


## ZMQ Unity
The best resource on ZMQ: https://zeromq.org/

First and foremost, you must remember that you can't use ZMQ in Unity's main thread. Unity's main thread is responsible for all the rendering and game logic, and it can't wait for the communication between devices. 
So, we set up a separate Unity Thread that will be responsible for communicating with the Python side. For that, we will use ZMQ, a universal messaging library that allows us to establish our communication network. We will use the Request (client) – Reply (server) pattern. Our headset will do the Requesting, and the Python side will do the Replying. 
First, we will create an empty object in Unity's Hierarchy window called Thread. The script running the thread will be attached to this object. We need to create a separate object to subscribe to an OnPythonResponse event later on from our last script.

Below, you can see all private and public variables declared for the ZMQ class. Most notable ones are server_address that must correspond to your laptop's IP  address, OnPythonResponse event that triggers our last C# script, and boolTouchedCoin that will control the behavior in our main function that is being run on the thread. 

![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/e7907231-a445-47e2-b1f3-17768542939b)

Then, we need to create a function that will subscribe to the abovementioned OnCoinStateChanged event. This  function is ReactChangedCoin. 

![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/26a4416d-360a-47e3-b9d4-bf754fd73c29)

Whenever the player touches the coin, the OnCoinStateChanged event will be invoked in TouchCoinStatic.cs, and the function that we subscribe to in our ZMQ.cs script will be called. In this script, invocation of the event leads to switching the boolTouchedCoin to true. This is needed because we must encapsulate the thread’s job into a single function that runs in a while loop.

We also need to use Unity’s Awake function, which is executed before any other game event. In this function, we create a thread, define which function it will be running, and subscribe ReactChangedCoin to the event in the TouchCoinStatic.cs. By the way, this is why we have two separate scripts for TouchCoin.cs and TouchCoinStatic.cs. TouchCoin.cs is attached to coin objects that are created and destroyed repeatedly. It would be more complicated to subscribe to their individual instances. Thus, we just subscribe to an event of a static class that keeps track of a current coin state without being attached to any specific coin.

![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/97da97d5-aa43-4756-bf11-c4a746cadc48)


Most importantly, our RequestFunc function that will be running on a thread repeatedly and exchange messages with the Python side when boolTouchedCoinvariable is set to true.

![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/4f11b70b-ac66-4c4c-83bd-0c2e6731e0c9)

To ensure the connection, we have to know the IP address of the Python server. We then need to create a Request socket object and connect it to the Python’s Reply socket at the provided port and IP address. We use “using new RequestSocket() as socket” in order to ensure that we handle closing the socket correctly, like with Python’s "with open(..) as file" syntax. 

The OnDestroy function is use to handle interrupting and closing a thread, unsubscribiung fro, events and so on.

![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/4ed7ce86-f32f-4b93-a73c-25f735dff9b5)


Thus, when the coin is touched, the clause in the thread is executed, which leads to the headset sending a “Touched coin!” message to the Python side and waiting for an answer.
Then, it waits for an answer. Upon receiving an answer, a second event in our project – OnPythonResponse - is invoked. This event, as you might already suspect, has a subscriber in our last script: WhenPythonResponds.cs. 

The job of this script is to generate a new coin in a Random position. Create an empty game object called Spawner and attach this script to it.
You can see the script for this below.

![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/3a95f2ef-2acf-4d11-bbc1-1cdd2ba79c65)


When all those parts are finished, you just need to assign the correct objects and properties to the public variables of your  WhenPythonResponds.cs.
You need to do it in the Unity UI.
First, create a Coin Prefab (prefabricated game object). Drag the Coin object from the Hierarchy into your Assets folder and save it as Coin. 
Now, drag the coin prefab object to the Coin prefab section of the  Spawner game  object, and drag Thread gameobject to Zmq segment. It should look like this:

![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/a228e5bb-4b76-4bf2-b6b1-18de479df460)


Upload the game to the headset by plugging it in and going to File - Build Settings - Android. Press Add open scenes in the top panel. Under Run device select your headset. Press Build and Run.
![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/4d6d116c-e4b9-4c0a-be6f-e072ba3f26d8)


## ZMQ Python
This concludes our discussion of the C# scripts. Now, let’s look at our Python script.
We will need to use pyzmq, playsound and os Python packages for this script.
The code will be pretty similar to ZMQ on the C# side, except we have to bind a server instead of connecting to a server.  Additionally,  we will be using the Reply socket type for the Python side.

![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/d745cf65-3b30-461d-82f7-c5f5b6d96396)

To indicate that the server has received the request, we will use a playsound Python package to play a sound whenever we touch a coin. The sound can be found in this project as sound_coin.mp3.

![image](https://github.com/EZ-maas2/MetaQuest_ZMQ_ICS/assets/85937429/1f826940-ff95-455a-abc6-8f0b2247439c)


Now, when you are running the Python code and the game on the headset simultaneously, provided they are on the same WiFi and you specified the IP address of the laptop in ZMQ.cs, the game should work.
Whenever you touch the coin, it should disappear, your laptop should play a coin sound, and a new coin should appear.

This should be it! Good luck!
