using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

/*
    T: UI
    X: CookCtrl, Door, Button, Display, Light, PowerTube, Output
    S: Timer
*/

namespace Microwave.Test.Integration
{
    public class Tests
    {
        private IUserInterface _sut;

        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;

        private IOutput _output;
        private ILight _light;
        private IDoor _door;

        private CookController _cookController;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _timer;

        [SetUp]
        public void Setup()
        {
	        _output = Substitute.For<IOutput>();
	        _timer = Substitute.For<ITimer>();

	        _display = new Display(_output);
	        _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);

            _door = new Door();
            _light = new Light(_output);
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();

            _sut = new UserInterface(_powerButton, _timeButton, 
	            _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _sut;
        }

        [Test]
        public void LightTurnOn_RecieveOnDoorOpenEvent_LightTurnsOn()
        {
            _sut.OnDoorOpened(this,EventArgs.Empty);
            _output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void LightTurnOff_RecieveOnDoorClosedEvent_Light()
        {
            _sut.OnDoorClosed(this, EventArgs.Empty);
            _output.Received(1).OutputLine("Light is turned off");
        }

    }
}