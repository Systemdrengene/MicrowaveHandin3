using System;
using System.IO;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

/*
    T: UI
    X: CookCtrl, Door, Button, Display, PowerTube, Output
    S: Light, Timer
*/

namespace Microwave.Test.Integration
{
    public class Integration5
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
            _display = Substitute.For<IDisplay>();
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);

            _door = new Door();
            _light = Substitute.For<ILight>();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();

            _sut = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light,
                _cookController);
            _cookController.UI = _sut;
        }

        [Test]
        public void PowerButtonPress_PressPowerButtonWhileReady_DisplayIsCalled()
        {
            _powerButton.Press();

            _display.Received(1).ShowPower(50);
        }

        [Test]
        public void PowerButtonPress_PressPowerButton3Times_DisplayCalledWithArgs()
        {
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();

            _display.Received(1).ShowPower(50);
            _display.Received(1).ShowPower(100);
            _display.Received(1).ShowPower(150);
        }

        [Test]
        public void PowerButtonPress_PressPowerButtonManyTimes_PowerKeepsWithinThreshold()
        {
            for(int i = 0; i < 30; i++)
                _powerButton.Press();

            _display.Received(0).ShowPower(750);
        }

        [Test]
        public void TimeButtonPress_PressWhileStateIsReady_NothingHappens()
        {
            _timeButton.Press();

            _display.Received(0);
        }

        [Test]
        public void TimeButtonPress_WhileStateSetPower_DisplayIsCalled()
        {
            _powerButton.Press();
            _timeButton.Press();

            _display.Received(1).ShowTime(1, 0);
        }

        [Test]
        public void StartCancelButtonPress_StateIsReady_NothingHappens()
        {
            _startCancelButton.Press();

            _display.Received(0);
        }

        [Test]
        public void StartCancelButtonPress_StateIsSetPower_ValuesAreReset()
        {
            _powerButton.Press();
            _startCancelButton.Press();

            _display.Received(1).Clear();
        }
    }   
}