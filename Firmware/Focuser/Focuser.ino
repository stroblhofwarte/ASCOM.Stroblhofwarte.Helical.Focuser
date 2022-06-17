
#define STEP 3
#define DIR  6
#define EN   8
#define SW 10

// Enable only one stepper motor driver!
//#define NODRV
#define TMC2130_STANDALONE // TMC2130 SilentStick with SPI jumper closed (Standalone) and all three jumpers open (1/16 µStepping interpolate to 256 steps, realy silent!)
//#define DRV8825 // DRV8825: Must be set to 32 microsteps
//#define DRVST810 // ST820: Must be set to 256 microsteps
///////////////////////////////////////

#ifdef TMC2130_STANDALONE
  #define STEPPER_ENABLE LOW
  #define STEPPER_DISABLE HIGH

  #define RIGHT_DIRECTION HIGH
  #define LEFT_DIRECTION LOW
  #define STEP_DELAY_US 1600
  #define STEPS_PER_REVOLUTION 3200 * GEAR_RATIO
#endif


#ifdef DRV8825
  #define STEPPER_ENABLE LOW
  #define STEPPER_DISABLE HIGH

  #define RIGHT_DIRECTION HIGH
  #define LEFT_DIRECTION LOW
  #define STEP_DELAY_US 800
  #define STEPS_PER_REVOLUTION 6400 * GEAR_RATIO
#endif

#ifdef ST820
  #define STEPPER_ENABLE HIGH
  #define STEPPER_DISABLE LOW

  #define RIGHT_DIRECTION LOW
  #define LEFT_DIRECTION HIGH
  #define STEP_DELAY_US 100
  #define STEPS_PER_REVOLUTION 51200 * GEAR_RATIO
  
#endif

#ifdef NODRV
  #define STEPPER_ENABLE LOW
  #define STEPPER_DISABLE HIGH

  #define RIGHT_DIRECTION LOW
  #define LEFT_DIRECTION HIGH

  #define STEPS_PER_REVOLUTION 0
#endif

#define DEVICE_IDENTIFICATION "FOCUSER"
#define CMD_IDENTIFICATION "ID"
#define CMD_TURN_RIGHT "TR"
#define CMD_TURN_LEFT "TL"
#define CMD_STOP "ST"
#define CMD_IS_MOVING "MV"
#define CMD_MOTOR_POWER_OFF "MOFF"
#define CMD_MOTOR_POWER_ON "MON"

int g_speed;
long g_pos_mech = 0;
long g_pos_goal = 0;

bool _notMotorPowerOff = false;

String g_command = "";
bool g_commandComplete = false;

void setup() { 
  pinMode(STEP, OUTPUT);
  pinMode(DIR, OUTPUT);
  pinMode(EN, OUTPUT);
  pinMode(SW, INPUT_PULLUP);
  digitalWrite(EN, STEPPER_DISABLE);
  g_speed = STEP_DELAY_US;
  Serial.begin(9600);
}

void MoveRight(long steps)
{
  g_pos_goal = g_pos_mech + steps;
}

void MoveLeft(long steps)
{
  g_pos_goal = g_pos_mech -  steps;
}

int Extract(String cmdid, String cmdstring)
{
  cmdstring.remove(0, cmdid.length());
  cmdstring.replace(':', ' ');
  cmdstring.trim();
  return cmdstring.toInt();
}

void Dispatcher()
{
  if(g_command.startsWith(CMD_IDENTIFICATION))
  {
    Serial.print(DEVICE_IDENTIFICATION);
    Serial.print('#');
  }
  else if(g_command.startsWith(CMD_TURN_RIGHT))
  {
    int val = Extract(CMD_TURN_RIGHT, g_command);
    long steps = val;
    MoveRight(steps);
    Serial.print("1#");
  }
  else if(g_command.startsWith(CMD_TURN_LEFT))
  {
    int val = Extract(CMD_TURN_RIGHT, g_command);
    long steps = val;
    MoveLeft(steps);
    Serial.print("1#");
  }
  else if(g_command.startsWith(CMD_STOP))
  {
    g_pos_goal = g_pos_mech;
    Serial.print("1#");
  }
  else if(g_command.startsWith(CMD_IS_MOVING))
  {
    if(g_pos_goal == g_pos_mech) 
      Serial.print("0#");
    else
      Serial.print("1#");
  }
  else if(g_command.startsWith(CMD_MOTOR_POWER_OFF))
  {
    _notMotorPowerOff = false;
    Serial.print("1#");
  }
  else if(g_command.startsWith(CMD_MOTOR_POWER_ON))
  {
    _notMotorPowerOff = true;
    Serial.print("1#");
  }
  else
    Serial.print("0#");
  
  g_command = "";
  g_commandComplete = false;
}

void loop() {
  if(g_commandComplete)
  {
    Dispatcher();
  }

  if(g_pos_goal > g_pos_mech)
  {
    digitalWrite(EN, STEPPER_ENABLE);
    digitalWrite(DIR, RIGHT_DIRECTION);
   
    delayMicroseconds(g_speed);
    digitalWrite(STEP, HIGH); 
    delayMicroseconds(g_speed);
    digitalWrite(STEP, LOW); 
    g_pos_mech++;
  }
  if(g_pos_goal < g_pos_mech)
  {
    digitalWrite(EN, STEPPER_ENABLE);
    digitalWrite(DIR, LEFT_DIRECTION);
   
    delayMicroseconds(g_speed);
    digitalWrite(STEP, HIGH); 
    delayMicroseconds(g_speed);
    digitalWrite(STEP, LOW); 
    g_pos_mech--;
  }
  if(g_pos_goal == g_pos_mech && _notMotorPowerOff == false)
    digitalWrite(EN, STEPPER_DISABLE); 
}

void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read();
    if(inChar == '\n') continue;
    if(inChar == '\r') continue;
    // add it to the inputString:
    g_command += inChar;
    // if the incoming character is a newline, set a flag so the main loop can
    // do something about it:
    if (inChar == ':') {
      g_commandComplete = true;
    }
  }
}
