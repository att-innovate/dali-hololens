## Dali-HoloLens Demo

### Prerequisites
- Dali-Server is up and running with a locally reachable address and port. For this example we assume it is reachable on `192.168.2.1:8085`. More information about Dali-Server can be found on [github.com/att-innovate/dali-server](https://github.com/att-innovate/dali-server).
- The Dali-Mgmt iOS application is running, either on the simulator or a real iOS device. More information can be found on [github.com/att-innovate/dali-mgmt](https://github.com/att-innovate/dali-mgmt).
- The Dali-Mgmt application is configured with the correct server address. Required steps can be found in the [User Manual](https://github.com/att-innovate/dali-mgmt/blob/master/docs/manual.md).
- The Dali-HoloLens is built and can be deployed to either the HoloLens Emulator or a HoloLens device. More information can be found in the [Installation Guide](installation-dali.md).

### Start the Dali-HoloLens Application
For the HoloLens Emulator the application can be deployed and started using Visual Studio. On a HoloLens itself it can be started via the HoloLens Start Menu.

### Position Dali-HoloLens
To **mark** a specific location in your space place the application next to it.

![place application](https://github.com/att-innovate/dali-hololens/blob/master/docs/placing.png)

### Click to Augment
To start augmenting a space click on the Augment button.

![startup screen](https://github.com/att-innovate/dali-hololens/blob/master/docs/startup.png)

### Configure Dali-Server
The first time you start the application will prompt you for the connection arguments for the Dali-Server, for example `192.168.2.1:8085`.

![server](https://github.com/att-innovate/dali-hololens/blob/master/docs/server.png)

###  Create New Mark
Next you will be able to either assign a **New** annotation or choose an **Existing** one.

Under **Settings** you can always adapt the Dali-Server connection arguments.  

![main screen](https://github.com/att-innovate/dali-hololens/blob/master/docs/main.png)

### Name the New Mark
The next page asks you for the name for the new mark.

![mark screen](https://github.com/att-innovate/dali-hololens/blob/master/docs/mark.png)

### View new Mark
This new mark isn’t **annotated** yet and that is why you see a default message about using Dali-Mgmt to annotate it.

![undefined mark](https://github.com/att-innovate/dali-hololens/blob/master/docs/undefined.png)

### Use Dali-Mgmt to Annotate
To annotate the new mark you have to switch to the Dali-Mgmt application on your iOS device. Press Refresh and the new mark should show up in the list.

![dali-mgmt](https://github.com/att-innovate/dali-hololens/blob/master/docs/dali-mgmt.png)

Clicking on the Edit button will allow you to **annotate** this new mark.

### Assign an Annotation to the new Mark
For this example we have chosen an Image as the type of annotation and “DontTouch” as the specific image we want to show on this mark. Press Save to store it.

![config-image](https://github.com/att-innovate/dali-hololens/blob/master/docs/configimage.png)

### Our First Augmentation
After a few seconds the visual of the new mark on the HoloLens should automatically update and show the “Don’t Touch” image.

![donttouch](https://github.com/att-innovate/dali-hololens/blob/master/docs/donttouch.png)

### Some Additional Notes
- If you place multiple marks in your space you will encounter that only one mark can be “active”. All the other marks will be suspended and just show a screenshot of the last annotation. If you click on a “suspended” mark the application will first briefly show the annotation of the last active mark before it reloads the correct one for this specific location. This behavior is caused by the way HoloLens handles 2D UWP applications. Detailed information about the 2D app model can be found on Microsoft’s [developer site](https://developer.microsoft.com/en-us/windows/mixed-reality/app_model).
- The visual representation of the different annotations is based on html. The corresponding html templates for each annotation-type can easily be changed on the Dali-Server. The templates can be found in the [templates](https://github.com/att-innovate/dali-server/tree/master/templates) folder.