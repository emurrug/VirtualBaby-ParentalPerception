#include "IRremote.h" //library needs install

const int GSR=A0; //defines signal for GSC
int sensorValue=0;
int gsr_average=0;

const int receiver = 11; //defines signal for infared
unsigned long key_value = 0;
String inputlog = "";
IRrecv irrecv(receiver);     // create instance of 'irrecv'
decode_results results;      // create instance of 'decode_results'

//the data string that gets printed each row
String dataStr= "";
 
void setup(){

  Serial.begin(9600);
  irrecv.enableIRIn(); 
}

void loop(){
  dataStr = ""; //clean out string
  long sum=0;

 if (irrecv.decode(&results)){
 
        if (results.value == 0XFFFFFFFF)
          results.value = key_value;

        switch(results.value){
          case 0xFFA25D:
          inputlog = "CH";
          break;
          case 0xFF629D:
          inputlog = "CH";
          break;
          case 0xFFE21D:
          inputlog = "CH";
          break;
          case 0xFF22DD:
          inputlog = "foward";
          break;
          case 0xFF02FD:
          inputlog = "backward";
          break ;  
          case 0xFFC23D:
          inputlog = "back";
          break ;               
          case 0xFFE01F:
         inputlog = "minus";
          break ;  
          case 0xFFA857:
          inputlog = "plus";
          break ;  
          case 0xFF906F:
          inputlog = "test";
          break ;  
          case 0xFF6897:
          inputlog = "0";
          break ;  
          case 0xFF30CF:
          inputlog = "1";
          break ;
          case 0xFF18E7:
          inputlog = "2";
          break ;
          case 0xFF7A85:
          inputlog = "3";
          break ;
          case 0xFF10EF:
          inputlog = "4";
          break ;
          case 0xFF38C7:
          inputlog = "5";
          break ;
          case 0xFF5AA5:
          inputlog = "6";
          break ;
          case 0xFF42BD:
          inputlog = "7";
          break ;
          case 0xFF4AB5:
          inputlog = "8";
          break ;
          case 0xFF52AD:
          inputlog = "9";
          break ;      
        }
        key_value = results.value;
        irrecv.resume(); 
  }

  for(int i=0;i<10;i++)           //Average the 10 measurements to remove the glitch
      {
      sensorValue=analogRead(GSR);
      sum += sensorValue;
      delay(5);
      }

   dataStr.concat(millis()); //convert long to charStr
   dataStr.concat(", ");
   gsr_average = sum/10;
   dataStr.concat(gsr_average);
   dataStr.concat(", ");
   dataStr.concat(inputlog);
   
   Serial.println(dataStr);
   //delay(100); The current rate of sampling is 5ms, which seems a bit excessive. 
   //consider reducing to 100ms to promote processing speed
