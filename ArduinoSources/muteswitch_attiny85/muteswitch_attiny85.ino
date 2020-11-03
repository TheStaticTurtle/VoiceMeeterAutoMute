#include "DigiKeyboard.h"

void setup() {
  pinMode(PB0,INPUT);
}

void loop() {
  DigiKeyboard.delay(5);
  if(!digitalRead(PB0)) {
    DigiKeyboard.sendKeyStroke(0);
    DigiKeyboard.sendKeyStroke(KEY_F2, MOD_ALT_LEFT | MOD_CONTROL_LEFT);
    while(!digitalRead(PB0)) { DigiKeyboard.delay(50); }
    DigiKeyboard.sendKeyStroke(KEY_F3, MOD_ALT_LEFT | MOD_CONTROL_LEFT);
  }
}
