using System;
using System.Diagnostics.CodeAnalysis;
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
    public class Integration6
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

	        display = new Display(output);
	        powerTube = new PowerTube(output);
            cookController = new CookController(timer, display, powerTube);

            door = new Door();
            light = new Light(output);
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();

            sut = new UserInterface(powerButton, timeButton, 
	            startCancelButton, door, display, light, cookController);
            cookController.UI = sut;
        }

        [Test]
        public void LightTurnOn_ReceiveOnDoorOpenEvent_LightTurnsOn()
        {
            // sut.OnDoorOpened(this,EventArgs.Empty); 
            door.Open();
            output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void LightTurnOff_ReceiveOnDoorClosedEvent_Light()
        {
	        //sut.OnDoorOpened(this, EventArgs.Empty); // Open Door = myState er DOOROPEN
            //sut.OnDoorClosed(this, EventArgs.Empty); 
            door.Open();
            door.Close();

            output.Received(1).OutputLine("Light is turned off");
        }

        [Test]
        public void OnPowerPressed_DefaultPower_DisplayShowsPower()
        {
            //sut.OnPowerPressed(this,EventArgs.Empty);
            powerButton.Press();
            output.Received(1).OutputLine($"Display shows: 50 W");
        }

        [Test]
        public void OnTimePressed_DisplayShowsTime()
        {
	        //sut.OnPowerPressed(this, EventArgs.Empty); // State.SetPower
            powerButton.Press();
	        //sut.OnTimePressed(this, EventArgs.Empty); // State.SetTime (Default time 1 min)
	        timeButton.Press();
            //sut.OnTimePressed(this, EventArgs.Empty); // Show time with 1 min added (2 min total)
            timeButton.Press();

            output.Received(1).OutputLine($"Display shows: 02:00");

        }

        [Test]
        public void OnStartCancelPressed_DisplayClear()
        {
	        //sut.OnPowerPressed(this, EventArgs.Empty); // State.SetPower
            powerButton.Press();
            //sut.OnStartCancelPressed(this, EventArgs.Empty);
            startCancelButton.Press();
            output.Received(1).OutputLine($"Display cleared");
        }

    }
}