

#include <Adafruit_NeoPixel.h>
// NX kijelző 9600 baud
#include "Nextion.h"
//#include <SoftwareSerial.h>

//SoftwareSerial wifiSerial(5, 6); // RX, TX

//
//bool DEBUG = false;
//int responseTime = 10;

#define PIN 7

#define NUMPIXELS 30

Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);

int delayval = 0; // timing delay in milliseconds

int redColor = 255;
int greenColor = 255;
int blueColor = 255;
int green[3] = {0, 138, 5};
int default_redColor = 125;
int default_greenColor = 125;
int default_blueColor = 125;
int led_start = 5;
int led_finish = 25;
int led_having = 0;
int led_trim[8] = {2, 3, 5, 4, 6, 7, 10, 15};

bool auto_led = true;

float referenciaEllenalas = 10000;
float meres;
float ellenalas;
float volt;

//Nexon Kijelző NexText(page id, component id, component név)
NexButton autoBe = NexButton(0, 4, "auto_be"); 
NexButton autoKi = NexButton(0, 8, "auto_ki"); 
NexButton ledBe = NexButton(0, 2, "led_be"); 
NexButton LedKi = NexButton(0, 3, "led_ki"); 
NexButton billFelett = NexButton(0, 5, "bill_felett"); 
NexButton felezes = NexButton(0, 6, "felezes"); 
NexButton elsoInditas = NexButton(0, 9, "elso_inditas");
 
NexText ledSate = NexText(0, 10, "led_state"); 

NexTouch *nex_listen_list[] = {
  &autoBe,
  &autoKi,
  &ledBe,
  &LedKi,
  &billFelett,
  &felezes,
  &elsoInditas,
  NULL
};

void autoBePopCallback(void *ptr) {
  ledSate.setText("LED: Auto");
    auto_led = true;
}
void autoKiPopCallback(void *ptr) {
  ledSate.setText("LED: Man");
    auto_led = false;
}
void ledBePopCallback(void *ptr) {
  ledSate.setText("LED: Be");
       auto_led = false;
      led_on();
}
void LedKiPopCallback(void *ptr) {
  ledSate.setText("LED: Ki");
        auto_led = false;
      led_off();
}
void billFelettPopCallback(void *ptr) {
  ledSate.setText("LED: 1Ind");
       auto_led = false;
      led_only_table();
}
void felezesPopCallback(void *ptr) {
   auto_led = false;
      if (led_having == 8) {
        led_having = 0;
      }
      int led_number = led_halving_on();
       ledSate.setText("LED: Be"+led_number);
      led_having++;
}
void elsoInditasPopCallback(void *ptr) {
  ledSate.setText("LED: 1Ind");
    led_first_on();
}

void setup() {
  Serial.begin(9600);
  //  wifiSerial.begin(115200);
  // pinMode(4, OUTPUT);
  pinMode(13, OUTPUT);
  pixels.begin();
  led_first_on();
  Serial.println("Be indult.");
  // sendToWifi("AT+CWMODE=2",responseTime,DEBUG); // configure as access point
  //  sendToWifi("AT+CIFSR",responseTime,DEBUG); // get ip address
  //  sendToWifi("AT+CIPMUX=1",responseTime,DEBUG); // configure for multiple connections
  //  sendToWifi("AT+CIPSERVER=1,80",responseTime,DEBUG); // turn on server on port 80
  // wifiSerial.print("AT+GMR");
  //  sendToUno("Wifi connection is running!",responseTime,DEBUG);


//  NX kijelző
  nexInit();
autoBe.attachPop(autoBePopCallback, &autoBe);
autoKi.attachPop(autoKiPopCallback, &autoKi);
ledBe.attachPop(ledBePopCallback, &ledBe);
LedKi.attachPop(LedKiPopCallback, &LedKi);
billFelett.attachPop(billFelettPopCallback, &billFelett);
felezes.attachPop(felezesPopCallback, &felezes);
elsoInditas.attachPop(elsoInditasPopCallback, &elsoInditas);
}
void loop() {
  led_main();
}

void led_main() {
  String incomingString;
  if (Serial.available() > 0) {
    incomingString = Serial.read();
    Serial.print("Parancs");
    //    Serial.print(incomingString);
    if (incomingString.equals("48")) {
      auto_led = true;
      Serial.println("-->Automatikus led vezérlés.");
    }
    if (incomingString.equals("49")) {
      auto_led = false;
      Serial.println("-->Manuális led vezérlés.");
    }
    if (incomingString.equals("50")) {
      auto_led = false;
      led_on();
      Serial.println("-->Manuális led vezérlés. Bekapcsolt led!");
    }
    if (incomingString.equals("51")) {
      auto_led = false;
      led_off();
      Serial.println("-->Manuális led vezérlés. Kikapcsolt led!");
    }
    if (incomingString.equals("52")) {
      auto_led = false;
      led_only_table();
      Serial.println("-->Manuális led vezérlés. Csak a billentyűzet felett bekapcsolt led!");
    }
    if (incomingString.equals("53")) {
      auto_led = false;
      if (led_having == 8) {
        led_having = 0;
      }
      int led_number = led_halving_on();
      Serial.print("-->Manuális led vezérlés. Csak ");
      Serial.print(led_number);
      Serial.println(" darab bekapcsolt led!");
      led_having++;
    }
    if (incomingString.equals("54")) {
      Serial.println("-->Automatikus led vezérlés. Led első inditás. ");
      led_first_on();
    }
    nexLoop(nex_listen_list);
  }

  meres = analogRead(A0);
  volt = meres * (5.0 / 1024.0);
  ellenalas = volt * referenciaEllenalas / (5.0 - volt);

  if (auto_led) {
    if (ellenalas >= 11000.0) {
      led_on();
    } else {
      led_off();
    }
  }

  pixels.show();
  delay(100);
}
void led_on() {
  digitalWrite(4, HIGH);
  digitalWrite(13, HIGH);
  for (int i = 0; i < NUMPIXELS; i++) {
    pixels.setPixelColor(i, pixels.Color(redColor, greenColor, blueColor));
     pixels.show();
  }
}
void led_off() {
  //    Serial.println("ki");
  digitalWrite(4, LOW);
  digitalWrite(13, LOW);
  for (int i = 0; i < NUMPIXELS; i++) {
    pixels.clear();
  }
}
void led_only_table() {
  led_off();
  for (int i = led_start; i < led_finish; i++) {
    pixels.setPixelColor(i, pixels.Color(redColor, greenColor, blueColor));
     pixels.show();
  }
}
int led_halving_on() {
 
  led_off();
  int led_number = 0;
  for (int i = 0; i < NUMPIXELS; i++) {
    if (i % led_trim[led_having] == 0) {
      led_number++;
      pixels.setPixelColor(i, pixels.Color(redColor, greenColor, blueColor));
    }
  }
  return led_number;
}
void led_first_on() {
  for (int i = 0; i < NUMPIXELS; i++) {
    pixels.clear();
    pixels.setPixelColor(i, pixels.Color(green[0], green[1], green[2]));
     pixels.show();
    delay(10);
  }
  delay(100);
  led_on();
  delay(1000);
  led_off();
}
//void wifi_main(){
//   if(Serial.available()>0){
//     String message = readSerialMessage();
//    if(find(message,"debugEsp8266:")){
//      String result = sendToWifi(message.substring(13,message.length()),responseTime,DEBUG);
//      if(find(result,"OK"))
//        sendData("\nOK");
//      else
//        sendData("\nEr");
//    }
//  }
//  if(wifiSerial.available()>0){
//
//    String message = readWifiSerialMessage();
//
//    if(find(message,"esp8266:")){
//       String result = sendToWifi(message.substring(8,message.length()),responseTime,DEBUG);
//      if(find(result,"OK"))
//        sendData("\n"+result);
//      else
//        sendData("\nErrRead");               //At command ERROR CODE for Failed Executing statement
//    }else
//    if(find(message,"HELLO")){  //receives HELLO from wifi
//        sendData("\\nHI!");    //arduino says HI
//    }else if(find(message,"LEDON")){
//      //turn on built in LED:
//      digitalWrite(13,HIGH);
//    }else if(find(message,"LEDOFF")){
//      //turn off built in LED:
//      digitalWrite(13,LOW);
//    }
//    else{
//      sendData("\nErrRead");                 //Command ERROR CODE for UNABLE TO READ
//    }
//  }
//}
//
//
//
//
//
///*
//* Name: sendData
//* Description: Function used to send string to tcp client using cipsend
//* Params:
//* Returns: void
//*/
//void sendData(String str){
//  String len="";
//  len+=str.length();
//  sendToWifi("AT+CIPSEND=0,"+len,responseTime,DEBUG);
//  delay(100);
//  sendToWifi(str,responseTime,DEBUG);
//  delay(100);
//  sendToWifi("AT+CIPCLOSE=5",responseTime,DEBUG);
//}
//
//
///*
//* Name: find
//* Description: Function used to match two string
//* Params:
//* Returns: true if match else false
//*/
//boolean find(String string, String value){
//  return string.indexOf(value)>=0;
//}
//
//
///*
//* Name: readSerialMessage
//* Description: Function used to read data from Arduino Serial.
//* Params:
//* Returns: The response from the Arduino (if there is a reponse)
//*/
//String  readSerialMessage(){
//  char value[100];
//  int index_count =0;
//  while(Serial.available()>0){
//    value[index_count]=Serial.read();
//    index_count++;
//    value[index_count] = '\0'; // Null terminate the string
//  }
//  String str(value);
//  str.trim();
//  return str;
//}
//
//
//
///*
//* Name: readWifiSerialMessage
//* Description: Function used to read data from ESP8266 Serial.
//* Params:
//* Returns: The response from the esp8266 (if there is a reponse)
//*/
//String  readWifiSerialMessage(){
//  char value[100];
//  int index_count =0;
//  while(wifiSerial.available()>0){
//    value[index_count]=wifiSerial.read();
//    index_count++;
//    value[index_count] = '\0'; // Null terminate the string
//  }
//  String str(value);
//  str.trim();
//  return str;
//}
//
//
//
///*
//* Name: sendToWifi
//* Description: Function used to send data to ESP8266.
//* Params: command - the data/command to send; timeout - the time to wait for a response; debug - print to Serial window?(true = yes, false = no)
//* Returns: The response from the esp8266 (if there is a reponse)
//*/
//String sendToWifi(String command, const int timeout, boolean debug){
//  String response = "";
//  wifiSerial.println(command); // send the read character to the esp8266
//  long int time = millis();
//  while( (time+timeout) > millis())
//  {
//    while(wifiSerial.available())
//    {
//    // The esp has data so display its output to the serial window
//    char c = wifiSerial.read(); // read the next character.
//    response+=c;
//    }
//  }
//  if(debug)
//  {
//    Serial.println(response);
//  }
//  return response;
//}
//
///*
//* Name: sendToUno
//* Description: Function used to send data to Arduino.
//* Params: command - the data/command to send; timeout - the time to wait for a response; debug - print to Serial window?(true = yes, false = no)
//* Returns: The response from the esp8266 (if there is a reponse)
//*/
//String sendToUno(String command, const int timeout, boolean debug){
//  String response = "";
//  Serial.println(command); // send the read character to the esp8266
//  long int time = millis();
//  while( (time+timeout) > millis())
//  {
//    while(Serial.available())
//    {
//      // The esp has data so display its output to the serial window
//      char c = Serial.read(); // read the next character.
//      response+=c;
//    }
//  }
//  if(debug)
//  {
//    Serial.println(response);
//  }
//  return response;
//}
