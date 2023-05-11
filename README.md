# Car Configurator - Brief Script Descriptions

_Assets/Scripts/EventManager.cs_
The EventManager class contains all events used throughout the project. Most of these are invoked the user interacting with the UI, to which both the stage and the UI then react.

_Assets/Scripts/FaceCamera.cs_
Can be attached to an object to keep it facing the camera regardless of the objects position and rotation. Only used in experimental component selector buttons (see ComponentSelector.cs)

_Assets/Scripts/GameManager.cs_
Exits the game on ESC

_Assets/Scripts/NavDirection.cs_
Defines directions for ui navigation (next/prev)

_Assets/Scripts/Stage/AccessoryPool.cs_
Only used to tag the accessory pool object so that we can find it via code

_Assets/Scripts/Stage/StageManager.cs_
The StageManager class is responsible for controlling what gets presented to the camera on stage. It can set up the next car that is going to be shown, and then rotate the stage to present it in front of the camera. The StageManager also controls accessory presentation via the currently selected car.

_Assets/Scripts/Stage/Car/Car.cs_
The Car class is used to store the car's details, set its color, as well as accessory slots that can be populated with accessories from the accessory pool.

_Assets/Scripts/Stage/Car/CarColorMaterial.cs_
Defines materials that can be applied to cars or accessories. We generally refer to these as 'color'

_Assets/Scripts/Stage/Car/ColorableCarPart.cs_
A collection of models that make up the car, which get coloured on the Car Color Selector canvas. This is set up because we wanted to color individual car parts independently, however due to time constraints this feature is incomplete and was left out. Currently any components of the car that are colourable should be set up as a single part.

_Assets/Scripts/Stage/Car/ColorableCarpartComponent.cs_
Attach this to any component of the car that should be colorable. We use this to allow the ColorableCarPart class to only accept ColorableCarPartComponents in the inspector as opposed to gameObjects or Transforms, to avoid accidentally dragging in non-colorable parts or any other gameobjects

_Assets/Scripts/Stage/Car/Spin.cs_
Can be attached to a gameobject to slowly rotate it around the y-axis. Currently used by cars as they are presented on stage.

_Assets/Scripts/Stage/Car/Accessories/Accessory.cs_
Used to store some information about each accessory such as price, and for setting colors

_Assets/Scripts/Stage/Car/Accessories/AccessoryPart.cs_
Stores the default color for each part of an accessory

_Assets/Scripts/Stage/Car/Accessories/AccessorySlot.cs_
Attached to a car gameobject, used to select or deselect accessories. Its transform determines how an accessory in the slot should be positioned relative to the car.

_Assets/Scripts/UI/Catalogue.cs_
Controls visibility of the car catalogue and populates the car/accessory list at runtime based on the cars and accessories available in the scene

_Assets/Scripts/UI/NavArrowBtn.cs_
Used to toggle whether a navigation arrow can be clicked or not. We want to disable it when there's only one car or one accessory available

_Assets/Scripts/UI/SpecsPanel.cs_
Used to visualise the visible car's specs with animated bars on transitions between cars

_Assets/Scripts/UI/UIManager.cs_
Manages the UI and which canvas is being shown to the user

_Assets/Scripts/UI/CanvasTextManagers/AccessorySelectorCanvasManager.cs_
Subclass of CanvasUpdateManager. Manages event subscriptions for the Accessory Selector Canvas

_Assets/Scripts/UI/CanvasTextManagers/CanvasUpdateManager.cs_
Defines methods used by its subclasses to update different UI elements based on values on stage (for example, fetching a car price and updating the price text)

_Assets/Scripts/UI/CanvasTextManagers/AccessorySelectorCanvasManager.cs_
Subclass of CanvasUpdateManager. Manages event subscriptions for the Accessory Selector Canvas

_Assets/Scripts/UI/CanvasTextManagers/CarColorSelectorCanvasManager.cs_
Subclass of CanvasUpdateManager. Manages event subscriptions for the Car Color Selector Canvas

_Assets/Scripts/UI/CanvasTextManagers/CarSelectorCanvasManager.cs_
Subclass of CanvasUpdateManager. Manages event subscriptions for the Car Selector Canvas

_Assets/Scripts/UI/CanvasTextManagers/CongratulationsCanvasManager.cs_
Subclass of CanvasUpdateManager. Currently unused, but included for completeness and in case of future changes

_Assets/Scripts/UI/CanvasTextManagers/AccessorySelectorCanvasManager.cs_
Subclass of CanvasUpdateManager. Manages event subscriptions for the Review Order Selector Canvas. Also sets up text in a scrollable panel to print details about the selected car and accessories.

_Assets/Scripts/UI/ColorPanel/AccessoryColorPanel.cs_
A color panel containing a group of mutually exclusive toggles used to select a color for accessories or to deselect them

_Assets/Scripts/UI/ColorPanel/CarColorPanel.cs_
Subclass of ColorPanel. A color panel containing a group of mutually exclusive toggles used to select a color for the car

_Assets/Scripts/UI/ColorPanel/ColorDeselectToggle.cs_
Toggle used to deselect the color of an accessory. Handles clicks and invokes an event if the toggle was set to On

_Assets/Scripts/UI/ColorPanel/ColorPanel.cs_
Parent of AccessoryColorPanel and CarColorPanel. Sets up toggle colors based on CarColorMaterials set in the inspector

_Assets/Scripts/UI/ColorPanel/ColorSelectToggle.cs_
Toggle used to select the color of an accessory. Handles clicks and invokes an event if the toggle was set to On. 

_Assets/Scripts/UI/ColorPanel/ComponentSelector/ComponentSelector.cs_
Experimental. Designed to contain selector buttons that can be used to select different parts of a car to be colored. 

_Assets/Scripts/UI/ColorPanel/ComponentSelector/ComponentSelectorBtn.cs_
Experimental. A button that can be placed in the scene ComponentSelector. To be used with FaceCamera.

