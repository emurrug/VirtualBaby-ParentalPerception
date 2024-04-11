*****************************
***Electrophysiological Setup***
*****************************

In the original Virtual Baby paradigm, it was not clear if participants were physiologically experiencing arousal or stress. Given the importance of arousal for directing attention, it is important to know whether the paradigm is valid in inducing stress in parents. 

To measure arousal (as a proxy for stress), we have decided to record Galvanic Skin Conductance (GSC) while exposing parents to the original stimuli. GSC can be used in tandem with the original paradigm or updated versions. 

****************
***Hardware***
****************
The GSC signal is collected from the fingers using a Grove GSR Sensor connected to an Arduino microcomputer. For more information, please see the Seeed Grove wiki: <https://wiki.seeedstudio.com/Grove-GSR_Sensor/>

Notably, the signal output is measured in units of Human Resistance (HR), which approximate units in ohms, but is slightly different. 

Human Resistance = ((1024 + 2(Serial_Port_Reading)) x 10000) / (512-Serial_Port_Reading); Serial_Port_Reading is the value display on Serial Port(between 0~1023)

An Elegoo infrared receiver system was also added to the Arduino. This allows researchers to log events through use of a remote.

****************
***Software***
****************
Arduino is written in C++. The Ardino IDE is free and publicly available at <https://www.arduino.cc/en/software>. Although using it is optional, the Arduino IDE has libraries for popular sensors already built in.

Serial output from the device was written to a local csv is through PuTTY. Note, this is only for Windows. Those who are comfortable could also just SSH into the device and store it using a microSD (not currently enabled). For more information and downloading PuTTY: <https://www.putty.org/>. When using PuTTY, researchers should make sure that: 

* serial line: (name of device port, e.g., COMS3)
* connection type: serial
* session logging: printable output
* file name = file path + ".csv"

****************
***File Navigation***
****************
Only three files are saved: 

* "GSR_button": This includes code to log GSC signal, time (ms), and button input
* "GSR": This includes code to log GSC signal and time (ms)
* "GSR_wo_MS": This includes code only to log GSC signal (helpful for visualizing plots and debugging)
