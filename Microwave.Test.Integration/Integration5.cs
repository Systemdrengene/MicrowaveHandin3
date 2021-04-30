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
        private IUserInterface sut;

        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;

        private IOutput output;
        private ILight light;
        private IDoor door;

        private CookController cookController;
        private IDisplay display;
        private IPowerTube powerTube;
        private ITimer timer;

        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            timer = Substitute.For<ITimer>();
            display = Substitute.For<IDisplay>();
            powerTube = new PowerTube(output);
            cookController = new CookController(timer, display, powerTube);

            door = new Door();
            light = Substitute.For<ILight>();
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();

            sut = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light,
                cookController);
            cookController.UI = sut;
        }

        [Test]
        public void PowerButtonPress_PressPowerButtonWhileReady_DisplayIsCalled()
        {
            powerButton.Press();

            display.Received(1).ShowPower(50);
        }

        [Test]
        public void PowerButtonPress_PressPowerButton3Times_DisplayCalledWithArgs()
        {
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();

            display.Received(1).ShowPower(50);
            display.Received(1).ShowPower(100);
            display.Received(1).ShowPower(150);
        }

        [Test]
        public void PowerButtonPress_PressPowerButtonManyTimes_PowerKeepsWithinThreshold()
        {
            for(int i = 0; i < 30; i++)
                powerButton.Press();

            display.Received(0).ShowPower(750);
        }

        [Test]
        public void TimeButtonPress_PressWhileStateIsReady_NothingHappens()
        {
            timeButton.Press();

            display.Received(0);
        }

        [Test]
        public void TimeButtonPress_WhileStateSetPower_DisplayIsCalled()
        {
            powerButton.Press();
            timeButton.Press();

            display.Received(1).ShowTime(1, 0);
        }

        [Test]
        public void StartCancelButtonPress_StateIsReady_NothingHappens()
        {
            startCancelButton.Press();

            display.Received(0);
        }

        [Test]
        public void StartCancelButtonPress_StateIsSetPower_ValuesAreReset()
        {
            powerButton.Press();
            startCancelButton.Press();

            display.Received(1).Clear();
        }
    }   
}