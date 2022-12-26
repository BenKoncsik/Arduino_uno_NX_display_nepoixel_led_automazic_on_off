#include <Adafruit_NeoPixel.h>


//led control pin
#define PIN 7

#define NUMPIXELS 30

Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);

float referencia_resistor = 10000;
float measurement_pin;
int resistor;
float volt;
int led_resistor_on = 11000;

// red, green, blue
int color[3] = { 255, 255, 255 };
int green[3] = { 0, 138, 5 };
int led_first = 5;
int led_last = 25;
bool led_light = false;
bool auto_led = true;
bool led_power = false;


void setup() {
  Serial.begin(9600);
  Serial.setTimeout(50);
  pinMode(13, OUTPUT);
  digitalWrite(13, HIGH);
  delay(1000);
  digitalWrite(13, LOW);
  pixels.begin();
  led_first_on();
}

void loop(void) {
  led_main();
}
void led_main() {
  auto_led_switch();
  int incomingInt = 0;
  byte incomingByte = 0;
  if (Serial.available() > 0) {
    incomingByte = Serial.read();
    // on
    if (incomingByte == 49) {
      Serial.write(incomingByte);
      led_on();
      led_power = true;
    }
    //off
    if (incomingByte == 50) {
      Serial.write(incomingByte);
      led_off();
      led_power = false;
    }
    // measurment
    if (incomingByte == 51) {
      Serial.flush();
      int mes = (measurement() + 492) / 1000;
      Serial.write(mes);
    }
    // automatic lamp
    if (incomingByte == 54) {
      Serial.flush();
      auto_led = !auto_led;
      Serial.write(auto_led);
    }
    // white brightness
    if (incomingByte == 55) {
      Serial.flush();
      int level = 0;
      delay(500);
      level = Serial.read();
      Serial.write(level);
      for (int i = 0; i < 3; i++) {
        color[i] = level;
      }
      if (led_light) {
        led_on();
      }
    }
    // sett rgb colore
    if (incomingByte == 56) {
      int clorComponent = 0;
      for (int i = 0; i < 3; i++) {
        Serial.flush();
        delay(500);
        clorComponent = Serial.read();
        Serial.write(clorComponent);
        color[i] = clorComponent;
      }
      if (led_light) {
        led_on();
      }
    }
    //set color brightness
    if (incomingByte == 57) {
      Serial.flush();
      int level_read = 0;
      delay(500);
      level_read = Serial.read();
      Serial.write(level_read);
      float level = 100 / level_read;

      for (int i = 0; i < 3; i++) {
        color[i] = color[i] * level;
      }
      if (led_light) {
        led_on();
      }
    }
  }
  // automatic on brightness level
  if (incomingByte == 58) {
    Serial.flush();
    int read_led = 0;
    delay(500);
    read_led = Serial.read();
    Serial.write(read_led);
    led_resistor_on = ((read_led * 690.9090588) + 502.56);
  }
}


void clearSerialBuffer() {
  while (Serial.available() > 0) {
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
  digitalWrite(13, HIGH);
  for (int i = 0; i < NUMPIXELS; i++) {
    pixels.setPixelColor(i, pixels.Color(color[0], color[1], color[2]));
    pixels.show();
  }
  led_light = true;
}
void led_off() {
  digitalWrite(13, LOW);
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

void auto_led_switch() {
  if (auto_led) {
    resistor = measurement();
    if (resistor >= led_resistor_on) {
      led_on();
    } else {
      led_off();
    }
  } else{
    if (led_power) {
      led_on();      
    } else {
      led_off();
    }
  }
}