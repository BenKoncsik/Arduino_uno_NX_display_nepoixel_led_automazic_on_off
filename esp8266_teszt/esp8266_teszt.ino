#include <SoftwareSerial.h>

SoftwareSerial espSerial(5, 6); // RX, TX
 int timeout = 10;
void setup() {
  Serial.begin(115200);
  espSerial.begin(115200);
  espSerial.println("AT+CWMODE:1");
 }
 
 
void loop() {

String response = "";
  espSerial.println("AT+GMR");
  long int time = millis();
  while( (time+timeout) > millis())
  {
    while(espSerial.available())
    {
    char c = espSerial.read(); // read the next character.
    response+=c;
    }  
  }

    Serial.println(response);
 
 
delay(1000);
  
}
