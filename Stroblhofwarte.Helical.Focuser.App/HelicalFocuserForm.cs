using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stroblhofwarte.Helical.Focuser.App
{
    public partial class HelicalFocuserForm : Form
    {
        #region Properties

        private string _focuserId = string.Empty;
        private ASCOM.DriverAccess.Focuser _focuser = null;
        private int _focuserPos = 0;
        private bool _connected = false;
        private bool _moveLeft = false;
        private bool _moveRight = false;
        private int _speed = 200;
        #endregion

        public HelicalFocuserForm()
        {
            InitializeComponent();
            UpdateAscomDeviceLabels();
            _focuserId = Properties.Settings.Default["FocuserId"].ToString();
            UpdateAscomDeviceLabels();
            EnableFocuserControls(false);
            this.BackgroundImage = Properties.Resources.FocuserBackground;
        }


        private void UpdateAscomDeviceLabels()
        {
            string visibleText = _focuserId;
            if (visibleText.Length > 32)
                visibleText = visibleText.Substring(0, 32) + "...";
            labelASCOMDevice.Text = visibleText;
        }
        private void buttonASCOMChooser_Click(object sender, EventArgs e)
        {
            _focuserId = ASCOM.DriverAccess.Focuser.Choose(_focuserId);
            UpdateAscomDeviceLabels();
            Properties.Settings.Default["FocuserId"] = _focuserId;
            Properties.Settings.Default.Save();
        }

        private void EnableFocuserControls(bool val)
        {
            buttonRight.Enabled = val;
            buttonLeft.Enabled = val;
            buttonLeftSlow.Enabled = val;
            buttonRightSlow.Enabled = val;
            buttonMove.Enabled = val;
            labelPosition.Enabled = val;
            textBoxMoveTo.Enabled = val;
        }
        private void buttonASCOMConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (_focuser == null)
                {
                    _focuser = new ASCOM.DriverAccess.Focuser(_focuserId);
                    _focuser.Connected = true;
                    if (_focuser.Connected)
                    {
                        //this.buttonRotatorConnect.Image = global::OAGSeeker.Properties.Resources.On;
                        EnableFocuserControls(true);
                        _connected = true;
                        textBoxMoveTo.Text = _focuser.Position.ToString();
                        if(!_focuser.Absolute)
                        {
                            MessageBox.Show("This application will work only with absolute focusers!", "Incompatible focuser");
                            _focuser.Connected = false;
                            _focuser.Dispose();
                            _focuser = null;
                            _connected = false;
                            EnableFocuserControls(false);
                        }
                    }
                    else
                    {
                        EnableFocuserControls(false);
                        _focuser.Dispose();
                        _focuser = null;
                    }
                }
                else
                {
                    _focuser.Connected = false;
                    _focuser.Dispose();
                    _focuser = null;
                    //this.buttonRotatorConnect.Image = global::OAGSeeker.Properties.Resources.Off;
                    EnableFocuserControls(false);
                    _connected = false;
                }
            }
            catch (Exception ex)
            {
                if (_focuser != null)
                    _focuser.Dispose();
                _focuser = null;
                //this.buttonRotatorConnect.Image = global::OAGSeeker.Properties.Resources.Off;
                EnableFocuserControls(false);
                _connected = false;
            }
        }

   
        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if(_connected)
            {
                _focuserPos = _focuser.Position;
                labelPosition.Text = _focuserPos.ToString();

                if(_moveRight)
                {
                    _focuser.Move(_focuserPos + _speed);
                }
                if (_moveLeft)
                {
                    _focuser.Move(_focuserPos - _speed);
                }
            }
        }

        private void buttonRight_MouseDown(object sender, MouseEventArgs e)
        {
            _moveLeft = false;
            _moveRight = true;
            _speed = 200;
        }

        private void buttonRight_MouseUp(object sender, MouseEventArgs e)
        {
            _moveRight = false;
            if(_connected)
            {
                _focuser.Halt();
                textBoxMoveTo.Text = _focuser.Position.ToString();
            }
        }

        private void buttonLeft_MouseDown(object sender, MouseEventArgs e)
        {
            _moveLeft = true;
            _moveRight = false;
            _speed = 200;
        }

        private void buttonLeft_MouseUp(object sender, MouseEventArgs e)
        {
            _moveLeft = false;
            if (_connected)
            {
                _focuser.Halt();
                textBoxMoveTo.Text = _focuser.Position.ToString();
            }
        }

        private void buttonLeftSlow_MouseDown(object sender, MouseEventArgs e)
        {
            _moveLeft = true;
            _moveRight = false;
            _speed = 1;
        }

        private void buttonLeftSlow_MouseUp(object sender, MouseEventArgs e)
        {
            _moveLeft = false;
            if (_connected)
            {
                _focuser.Halt();
                textBoxMoveTo.Text = _focuser.Position.ToString();
            }
        }

        private void buttonRightSlow_MouseDown(object sender, MouseEventArgs e)
        {
            _moveLeft = false;
            _moveRight = true;
            _speed = 1;
        }

        private void buttonRightSlow_MouseUp(object sender, MouseEventArgs e)
        {
            _moveRight = false;
            if (_connected)
            {
                _focuser.Halt();
                textBoxMoveTo.Text = _focuser.Position.ToString();
            }
        }

        private void buttonMove_Click(object sender, EventArgs e)
        {
            int pos = int.Parse(textBoxMoveTo.Text);
            if (_connected)
            {
                _focuser.Move(pos);
            }
        }
    }
    
}
