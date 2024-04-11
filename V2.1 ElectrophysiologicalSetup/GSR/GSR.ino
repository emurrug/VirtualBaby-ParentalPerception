const int GSR=A0;
int sensorValue=0;
int gsr_average=0;

char dataStr[100]= "";
char buffer[7];
 
void setup(){
  Serial.begin(9600);
}
 
void loop(){
  dataStr[0] = 0; //clean out string
  long sum=0;
  for(int i=0;i<10;i++)           //Average the 10 measurements to remove the glitch
      {
      sensorValue=analogRead(GSR);
      sum += sensorValue;
      delay(5);
      }

   ltoa( millis(),buffer,10); //convert long to charStr
   strcat(dataStr, buffer); //add it to the end
   strcat(dataStr, ", "); //append the delimiter
  
   gsr_average = sum/10;
   dtostrf(gsr_average, 2, 1, buffer);
   strcat(dataStr, buffer); //add it to the end
   strcat(dataStr, 0); //terminate correctly 
   Serial.println(dataStr);
}
