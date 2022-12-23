#include <Adafruit_NeoPixel.h>


//led control pin
//#define PIN 7
#define PIN 13

#define NUMPIXELS 30

Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);

float referencia_resistor = 10000;
float measurement_pin;
int resistor;
float volt;
float led_resistor_on = 11000.0;

// red, green, blue
int color[3] = {255, 255, 255};
int green[3] = { 0, 138, 5 };
int led_first = 5;
int led_last = 25;
bool led_light = false;
bool auto_led = true;
 
//segmens
int pinA = 2;
int pinB = 3;
int pinC = 4;
int pinD = 5;
int pinE = 6;
int pinF = 7;
int pinG = 8;
int D1 = 9;
int D2 = 10;
int D3 = 11;
int D4 = 12;


void setup() {
  Serial.begin(9600);
  Serial.setTimeout(50);
  //pinMode(13, OUTPUT);
  //digitalWrite(13, HIGH);
  //delay(1000);
  //digitalWrite(13, LOW);
  pixels.begin();
  

    pinMode(pinA, OUTPUT);     
  pinMode(pinB, OUTPUT);     
  pinMode(pinC, OUTPUT);     
  pinMode(pinD, OUTPUT);     
  pinMode(pinE, OUTPUT);     
  pinMode(pinF, OUTPUT);     
  pinMode(pinG, OUTPUT);   
  pinMode(D1, OUTPUT);  
  pinMode(D2, OUTPUT);  
  pinMode(D3, OUTPUT);  
  pinMode(D4, OUTPUT);  

 digitalWrite(D1, HIGH);
  digitalWrite(D2, LOW);
  digitalWrite(D3, LOW);
  digitalWrite(D4, LOW);
  displayNumbber(11);
  delay(1000);
  digitalWrite(D1, LOW);
  digitalWrite(D2, HIGH);
  digitalWrite(D3, LOW);
  digitalWrite(D4, LOW);
  displayNumbber(11);
   delay(1000);
   digitalWrite(D1, LOW);
  digitalWrite(D2, LOW);
  digitalWrite(D3, HIGH);
  digitalWrite(D4, LOW);
  displayNumbber(11);
   delay(1000);
   digitalWrite(D1, LOW);
  digitalWrite(D2, LOW);
  digitalWrite(D3, LOW);
  digitalWrite(D4, HIGH);
  displayNumbber(11);
  led_first_on();
}

void loop(void) {
  led_main();
}
void led_main() {
  int incomingInt = 0;
  byte incomingByte = 0;
  if (Serial.available() > 0) {
    incomingByte  = Serial.read();

    if (incomingByte == 49 ) {
      Serial.write(incomingByte);
      Serial.flush();
      led_on();
    }

     if (incomingByte == 50) {
        Serial.write(incomingByte);
        led_off();
    }
    if (incomingByte == 51) {
        Serial.flush();
        int mes = (measurement() + 492)/1000;
        Serial.write(mes);
    }

        if (incomingByte == 54) {          
          auto_led = !auto_led;
          Serial.write(auto_led); 
          Serial.flush();
          Serial.begin(9600);
   
    }

    if (incomingByte == 55) {
      displayNumbber(99);
        displayNumbber(0);
        displayNumbber(99);
      Serial.flush();
      int level = 0;
      delay(1000);
      clearSerialBuffer(); 
     if(Serial.available() > 0){
        level = Serial.read();
        displayNumbber(99);
        displayNumbber(level);
        Serial.write(level);      
        for(int i = 0; i < 3; i++){
          color[i] = level;
        }
        if(led_light){
          led_on();
        }
      }
      delay(100);
    }
   clearSerialBuffer();
  }
}

void clearSerialBuffer(){
    while(Serial.available() > 0){
      Serial.read();
    }
}

int measurement() {
  measurement_pin = analogRead(A0);
  volt = measurement_pin * (5.0 / 1024.0);
  resistor = volt * referencia_resistor / (5 - volt);
  return resistor;
}

void led_on() {
  //digitalWrite(13, HIGH);
  for (int i = 0; i < NUMPIXELS; i++) {
    pixels.setPixelColor(i, pixels.Color(color[0], color[1], color[2]));
    pixels.show();
  }
  led_light = true;
}
void led_off() {
 // digitalWrite(13, LOW);
  for (int i = 0; i < NUMPIXELS; i++) {
    pixels.clear();
    pixels.show();
  }
  led_light = false;
}
void led_only_table() {
  led_off();
  for (int i = led_first; i < led_last; i++) {
    pixels.setPixelColor(i, pixels.Color(color[0], color[1], color[2]));
    pixels.show();
  }
}

void led_first_on() {
  for (int i = 0; i < NUMPIXELS; i++) {
    pixels.clear();
    pixels.setPixelColor(i, pixels.Color(green[0], green[1], green[2]));
    pixels.show();
    delay(20);
  }
  delay(100);
  led_on();
  delay(1000);
  led_off();
}


void displayNumbber(int number){
  switch (number){
    case 11:
 digitalWrite(pinA, HIGH);   
  digitalWrite(pinB, HIGH);   
  digitalWrite(pinC, HIGH);   
  digitalWrite(pinD, HIGH);   
  digitalWrite(pinE, HIGH);   
  digitalWrite(pinF, HIGH);   
  digitalWrite(pinG, HIGH);  
    break;
 case 0:
 digitalWrite(pinA, HIGH);   
  digitalWrite(pinB, HIGH);   
  digitalWrite(pinC, HIGH);   
  digitalWrite(pinD, HIGH);   
  digitalWrite(pinE, HIGH);   
  digitalWrite(pinF, HIGH);   
  digitalWrite(pinG, LOW);   
 break;
 case 1:
digitalWrite(pinA, LOW);   
  digitalWrite(pinB, LOW);   
  digitalWrite(pinC, LOW);   
  digitalWrite(pinD, HIGH);   
  digitalWrite(pinE, LOW);   
  digitalWrite(pinF, LOW);   
  digitalWrite(pinG, LOW);  
 break;
 case 2:
digitalWrite(pinA, HIGH);   
  digitalWrite(pinB, HIGH);   
  digitalWrite(pinC, LOW);   
  digitalWrite(pinD, HIGH);   
  digitalWrite(pinE, LOW);   
  digitalWrite(pinF, HIGH);   
  digitalWrite(pinG, LOW);  
 break;
 case 3:
  digitalWrite(pinA, LOW);   
  digitalWrite(pinB, HIGH);   
  digitalWrite(pinC, LOW);   
  digitalWrite(pinD, HIGH);   
  digitalWrite(pinE, LOW);   
  digitalWrite(pinF, LOW);   
  digitalWrite(pinG, HIGH); 
 break;
 case 4:
  digitalWrite(pinA, HIGH);   
  digitalWrite(pinB, LOW);   
  digitalWrite(pinC, LOW);   
  digitalWrite(pinD, HIGH);   
  digitalWrite(pinE, HIGH);   
  digitalWrite(pinF, LOW);   
  digitalWrite(pinG, LOW); 
 break;
 case 5:
  digitalWrite(pinA, LOW);   
  digitalWrite(pinB, HIGH);   
  digitalWrite(pinC, LOW);   
  digitalWrite(pinD, LOW);   
  digitalWrite(pinE, HIGH);   
  digitalWrite(pinF, LOW);   
  digitalWrite(pinG, LOW); 
 break;
 case 6:
  digitalWrite(pinA, LOW);   
  digitalWrite(pinB, HIGH);   
  digitalWrite(pinC, LOW);   
  digitalWrite(pinD, LOW);   
  digitalWrite(pinE, LOW);   
  digitalWrite(pinF, LOW);   
  digitalWrite(pinG, LOW);     

 break;
 case 7:
  digitalWrite(pinA, LOW);   
  digitalWrite(pinB, LOW);   
  digitalWrite(pinC, LOW);   
  digitalWrite(pinD, HIGH);   
  digitalWrite(pinE, HIGH);   
  digitalWrite(pinF, HIGH);   
  digitalWrite(pinG, HIGH); 
 break;
 case 8:
 digitalWrite(pinA, LOW);   
  digitalWrite(pinB, LOW);   
  digitalWrite(pinC, LOW);   
  digitalWrite(pinD, LOW);   
  digitalWrite(pinE, LOW);   
  digitalWrite(pinF, LOW);   
  digitalWrite(pinG, LOW);  
 break;
 case 9:
  digitalWrite(pinA, LOW);   
  digitalWrite(pinB, LOW);   
  digitalWrite(pinC, LOW);   
  digitalWrite(pinD, HIGH);   
  digitalWrite(pinE, HIGH);   
  digitalWrite(pinF, LOW);   
  digitalWrite(pinG, LOW);  
 break;
case 99:
  digitalWrite(pinA, LOW);   
  digitalWrite(pinB, LOW);   
  digitalWrite(pinC, LOW);   
  digitalWrite(pinD, LOW);   
  digitalWrite(pinE, LOW);   
  digitalWrite(pinF, LOW);   
  digitalWrite(pinG, LOW);
  break;
  default:
   digitalWrite(D1, HIGH);
   delay(100);
   digitalWrite(D1, LOW);
   digitalWrite(D2, HIGH);
   delay(100);
   digitalWrite(D2, LOW); 
   digitalWrite(D3, HIGH);
   delay(100);
   digitalWrite(D3, LOW); 
   digitalWrite(D4, HIGH);
   delay(100);
   digitalWrite(D4, LOW);
  break;
 }
}