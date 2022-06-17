//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Focuser driver for Stroblhofwarte.Helical.Focuser
//
// Description:	Stepper motor based relative helical Focuser. The electronic based
//              on a arduino uno with an CNC board. Only the Y-Motor is used on 
//              the CNC board.
//              The communication between this driver and the arduino is done with a simple
//              serial protocol, 9600 baud. A command from this driver to the arduino ends with
//              a colon (:), the answer of the arduino ends with a hash (#).
//              
//              Command         Response        Description
//              -----------------------------------------------------------------------------
//              ID:             FOCUSER#        Device identification
//              TRxxx:          1#              Move right xxx steps
//              TLxxx:          1#              Move left xxx steps
//              ST:             1#              Stop the current movement
//              MV:             0# or 1#        #1: Rotator is moving, otherwise 0#
//              MOFF:           1#              The motor is disabled after movement
//              MON:            1#              The motor is always powerd
//
// Implements:	ASCOM Focuser interface 
// Author:		Othmar Ehrhardt <othmar.ehrhardt@stroblhof-oberrohrbach.de>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 17.06.2022	0.9.0	    Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//

#define Focuser

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.Stroblhofwarte.Helical.Focuser
{
    //
    // DeviceID is ASCOM.Stroblhofwarte.Helical.Focuser.Focuser
    //
 

    /// <summary>
    /// ASCOM Focuser Driver for Stroblhofwarte.Helical.Focuser.
    /// </summary>
    [Guid("d781659c-0423-40de-99e2-21132601059f")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuserV3
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.Stroblhofwarte.Helical.Focuser.Focuser";
      
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "ASCOM Focuser Driver for Stroblhofwarte.Helical.Focuser.";

        internal static string comPortProfileName = "COM Port"; // Constants used for Profile persistence
        internal static string comPortDefault = "COM1";
        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";

        internal static string comPort; // Variables to hold the current device configuration

        private ASCOM.Utilities.Serial _serial;

        internal static string _doNotSwitchPoerOffProfileName = "DoNotSwitchPowerOff";
        private bool _doNotSwitchPowerOff = false;
        private object _lock = new object();

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal TraceLogger tl;
        public TraceLogger Logger { get { return tl; } }
        /// <summary>
        /// Initializes a new instance of the <see cref="Stroblhofwarte.Helical.Focuser"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Focuser()
        {
            tl = new TraceLogger("", "Stroblhofwarte.Helical.Focuser");
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl.LogMessage("Focuser", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro-utilities object
           

            tl.LogMessage("Focuser", "Completed initialisation");
        }


        //
        // PUBLIC COM INTERFACE IFocuserV3 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm(this))
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public bool DoNotSwitchOffMotorPower
        {
            get
            {
                return _doNotSwitchPowerOff;
            }
            set
            {
                _doNotSwitchPowerOff = value;
                WriteProfile();
                if (!connectedState) return;
                if (value)
                {
                    _serial.Transmit("MON:");
                    _serial.ReceiveTerminated("#");
                }
                else
                {
                    _serial.Transmit("MOFF:");
                    _serial.ReceiveTerminated("#");
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // TODO The optional CommandBlind method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBlind must send the supplied command to the mount and return immediately without waiting for a response

            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            // TODO The optional CommandBool method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBool must send the supplied command to the mount, wait for a response and parse this to return a True or False value

            // string retString = CommandString(command, raw); // Send the command and wait for the response
            // bool retBool = XXXXXXXXXXXXX; // Parse the returned string and create a boolean True / False value
            // return retBool; // Return the boolean value to the client

            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // TODO The optional CommandString method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandString must send the supplied command to the mount and wait for a response before returning this to the client

            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the trace logger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        private bool CheckForStroblFocuserDevice()
        {
            lock (_lock)
            {
                string idString = String.Empty;
                int retry = 3;
                while (idString != "FOCUSER#")
                {
                    try
                    {
                        _serial.Transmit("ID:");
                        idString = _serial.ReceiveTerminated("#");
                    }
                    catch (Exception ex)
                    {
                        retry--;
                        if (retry == 0) return false;
                        continue;
                    }
                }
                return true;
            }
        }
        public bool Connected
        {
            get
            {
                LogMessage("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected", "Set {0}", value);
                if (value == IsConnected)
                    return;
                if (value)
                {
                    LogMessage("Connected Set", "Connecting to address {0}", comPort);
                    try
                    {
                        LogMessage("Connected Set", "Connecting to port {0}", comPort);
                        lock (_lock)
                        {
                            _serial = new ASCOM.Utilities.Serial();
                            _serial.PortName = comPort;
                            _serial.StopBits = SerialStopBits.One;
                            _serial.Parity = SerialParity.None;
                            _serial.Speed = SerialSpeed.ps9600;
                            _serial.DTREnable = false;
                            _serial.Connected = true;
                            if (CheckForStroblFocuserDevice())
                            {
                                connectedState = true;
                                // set the motor power off state again 
                                // to transmit this setting also to the arduino device:
                                bool state = DoNotSwitchOffMotorPower;
                                DoNotSwitchOffMotorPower = state;
                            }
                            else
                                connectedState = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        _serial.Connected = false;
                        _serial.Dispose();
                        LogMessage("Connected Set", ex.ToString());
                    }
                }
                else
                {
                    connectedState = false;
                    _serial.Connected = false;
                    _serial.Dispose();
                    LogMessage("Connected Set", "Disconnecting from adress {0}", comPort);
                }

            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "3");
                return Convert.ToInt16("3");
            }
        }

        public string Name
        {
            get
            {
                string name = "Short driver name - please customise";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IFocuser Implementation

        private int focuserPosition = 6400; // After switchon the focuser starts always in the middle of the field....
        private const int focuserSteps = 12800;

        // It is a relative device
        public bool Absolute
        {
            get
            {
                tl.LogMessage("Absolute Get", false.ToString());
                return false; // This is an absolute focuser
            }
        }

        public void Halt()
        {
            if (!connectedState) return;
            _serial.Transmit("ST:");
            string ret = _serial.ReceiveTerminated("#");
        }

        public bool IsMoving
        {
            get
            {
                if (!connectedState) return false;
                tl.LogMessage("IsMoving Get", false.ToString()); // This rotator has instantaneous movement
                _serial.Transmit("MV:");
                string ret = _serial.ReceiveTerminated("#");
                if (ret == "1#") return true;
                return false;
            }
        }

        public bool Link
        {
            get
            {
                tl.LogMessage("Link Get", this.Connected.ToString());
                return this.Connected; // Direct function to the connected method, the Link method is just here for backwards compatibility
            }
            set
            {
                tl.LogMessage("Link Set", value.ToString());
                this.Connected = value; // Direct function to the connected method, the Link method is just here for backwards compatibility
            }
        }

        public int MaxIncrement
        {
            get
            {
                tl.LogMessage("MaxIncrement Get", focuserSteps.ToString());
                return focuserSteps; // Maximum change in one move
            }
        }

        public int MaxStep
        {
            get
            {
                tl.LogMessage("MaxStep Get", focuserSteps.ToString());
                return focuserSteps; // Maximum extent of the focuser, so position range is 0 to 10,000
            }
        }

        public void Move(int Position)
        {
            tl.LogMessage("Move", Position.ToString());
            if (Position > focuserSteps) return;
            if (Position < 0) return;
            int steps = Position - focuserPosition;
            if (steps > 0)
            {
                _serial.Transmit("TR" + steps.ToString() + ":");
            }
            else if (steps < 0)
            {
                _serial.Transmit("TL" + (-steps).ToString() + ":");
            }
            else
                return;
            string ret = _serial.ReceiveTerminated("#");
            focuserPosition = Position; // Set the focuser position
        }

        public int Position
        {
            get
            {
                return focuserPosition; // Return the focuser position
            }
        }

        public double StepSize
        {
            get
            {
                tl.LogMessage("StepSize Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("StepSize", false);
            }
        }

        public bool TempComp
        {
            get
            {
                tl.LogMessage("TempComp Get", false.ToString());
                return false;
            }
            set
            {
                tl.LogMessage("TempComp Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("TempComp", false);
            }
        }

        public bool TempCompAvailable
        {
            get
            {
                tl.LogMessage("TempCompAvailable Get", false.ToString());
                return false; // Temperature compensation is not available in this driver
            }
        }

        public double Temperature
        {
            get
            {
                tl.LogMessage("Temperature Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Temperature", false);
            }
        }

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Focuser";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);
                _doNotSwitchPowerOff = Convert.ToBoolean(driverProfile.GetValue(driverID, _doNotSwitchPoerOffProfileName, string.Empty, "false"));
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());
                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());
                driverProfile.WriteValue(driverID, _doNotSwitchPoerOffProfileName, _doNotSwitchPowerOff.ToString());
            }
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion
    }
}
