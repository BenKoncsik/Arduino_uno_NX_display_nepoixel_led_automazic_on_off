#include <Adafruit_NeoPixel.h>
// NX kijelző 9600 baud
#include <Nextion.h>

#define PIN 7

#define NUMPIXELS 30

Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);


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
NexButton ledKi = NexButton(0, 3, "led_ki"); 
NexButton billFelett = NexButton(0, 5, "bill_felett"); 
NexButton felezes = NexButton(0, 6, "felezes"); 
NexButton elsoInditas = NexButton(0, 9, "elso_inditas");
 
NexText ledState1 = NexText(0, 10, "led_state"); 
NexText ledState2 = NexText(0, 13, "led_state_1"); 
NexText ledState3 = NexText(0, 14, "led_state_2"); 

NexTouch *nex_listen_list[] = {
  &autoBe,
  &autoKi,
  &ledBe,
  &ledKi,
  &billFelett,
  &felezes,
  &elsoInditas,
  NULL
};

void autoBePopCallback(void *ptr) {
 
  ledState1.setText("Automatik");
  ledState2.setText("us");
  ledState3.setText("");
    auto_led = true;
}
void autoKiPopCallback(void *ptr) {
  ledState1.setText("Manualis");
  ledState2.setText("");
  ledState3.setText("");
    auto_led = false;
}
void ledBePopCallback(void *ptr) {
  ledState1.setText("Be");
  ledState2.setText("");
  ledState3.setText("");
       auto_led = false;
      led_on();
}
void ledKiPopCallback(void *ptr) {
  ledState1.setText("Ki");
  ledState2.setText("");
  ledState3.setText("");
        auto_led = false;
      led_off();
}
void billFelettPopCallback(void *ptr) {
  ledState1.setText("Billentyu");
  ledState2.setText("zet ");
  ledState3.setText("felett");
       auto_led = false;
      led_only_table();
}
void felezesPopCallback(void *ptr) {
  ledState1.setText("Led ");
  
   auto_led = false;
      if (led_having == 8) {
        led_having = 0;
      }
      int led_number = led_halving_on();
      String led_number_str = String(led_number);
      char Buf[9];
       led_number_str.toCharArray(Buf, 9);
       ledState2.setText(Buf);
       ledState3.setText(" villagit");
      led_having++;
}
void elsoInditasPopCallback(void *ptr) {
  
  ledState1.setText("elso ");
  ledState2.setText("inditas");
  ledState3.setText(" moka");
    led_first_on();
}

void setup() {
  Serial.begin(9600);
  pinMode(13, OUTPUT);
  pixels.begin();
  led_first_on();

//  NX kijelző
  nexInit();
autoBe.attachPop(autoBePopCallback, &autoBe);
autoKi.attachPop(autoKiPopCallback, &autoKi);
ledBe.attachPop(ledBePopCallback, &ledBe);
ledKi.attachPop(ledKiPopCallback, &ledKi);
billFelett.attachPop(billFelettPopCallback, &billFelett);
felezes.attachPop(felezesPopCallback, &felezes);
elsoInditas.attachPop(elsoInditasPopCallback, &elsoInditas);
ledState3.setText("Be indult");
}
void loop(void) {
  led_main();

}

void led_main() {
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
    nx_touch_screen();
  pixels.show();
}
void nx_touch_screen(){
     nexLoop(nex_listen_list);
  }
void serial_monitor(){
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
    
  }
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
