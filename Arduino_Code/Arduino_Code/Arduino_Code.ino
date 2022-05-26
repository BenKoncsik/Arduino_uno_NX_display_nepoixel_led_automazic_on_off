#include <Adafruit_NeoPixel.h>

#define PIN 7

#define NUMPIXELS 100      

Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);

int delayval = 0; // timing delay in milliseconds

int redColor = 125;
int greenColor = 125;
int blueColor = 125;

float referenciaEllenalas = 10000;
float meres;
float ellenalas;
float volt;
void setup() {
 Serial.begin(9600);
 pinMode(4, OUTPUT);
  pinMode(13, OUTPUT);
   pixels.begin();
}
void loop() {
meres = analogRead(A0);
volt = meres * (5.0 / 1024.0);
ellenalas = volt * referenciaEllenalas / (5.0 - volt);
Serial.println(ellenalas);
if(ellenalas >= 11000.0){
    digitalWrite(4, HIGH);
    digitalWrite(13, HIGH);
  for(int i = 0; i < NUMPIXELS; i++){
   pixels.setPixelColor(i, pixels.Color(redColor, greenColor, blueColor));
  }  
  }else{
    digitalWrite(4, LOW);
     digitalWrite(13, LOW);
    for(int i = 0; i < NUMPIXELS; i++){
   pixels.clear();   
  }
  
  }
  pixels.show();
}
