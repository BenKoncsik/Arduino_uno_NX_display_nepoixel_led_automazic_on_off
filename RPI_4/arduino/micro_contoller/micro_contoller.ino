#include <Adafruit_NeoPixel.h>


//led control pin
#define PIN 7

#define NUMPIXELS 30

Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);

float referencia_resistor = 10000;
float measurement_pin;
float resistor;
float volt;
float led_resistor_on = 11000.0;
int redColor = 255;
int greenColor = 255;
int blueColor = 255;

int green[3] = { 0, 138, 5 };
int led_first = 5;
int led_last = 25;


void setup() {
  Serial.begin(9600);
  Serial.setTimeout(50);
  pinMode(13, OUTPUT);
  digitalWrite(13, HIGH);
  delay(1000);
  digitalWrite(13, LOW);
  pixels.begin();
  // led_first_on();
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
      led_on();
    }

     if (incomingByte == 50) {
        Serial.write(incomingByte);
        led_off();
    }
    if (incomingByte == 51) {
        float mes = measurement();
        Serial.write("55");        
    }
   
  }
}


float measurement() {
  measurement_pin = analogRead(A0);
  volt = measurement_pin * (5.0 / 1024.0);
  resistor = volt * referencia_resistor / (5.0 - volt);
  return resistor;
}

void led_on() {
  digitalWrite(13, HIGH);
  for (int i = 0; i < NUMPIXELS; i++) {
    pixels.setPixelColor(i, pixels.Color(redColor, greenColor, blueColor));
    pixels.show();
  }
}
void led_off() {
  digitalWrite(13, LOW);
  for (int i = 0; i < NUMPIXELS; i++) {
    pixels.clear();
    pixels.show();
  }
}
void led_only_table() {
  led_off();
  for (int i = led_first; i < led_last; i++) {
    pixels.setPixelColor(i, pixels.Color(redColor, greenColor, blueColor));
    pixels.show();
  }
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