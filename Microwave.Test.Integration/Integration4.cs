using System;
using System.IO;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.Exceptions;
using NUnit.Framework;

/*
    T: UI
    X: CookCtrl, Display, Door, PowerTube, Output
    S:  Button, Light, Timer
*/

namespace Microwave.Test.Integration
{
    public class Integration4
    {
        private UserInterface userI; 

        private CookController cookCtrl;
        private IDisplay display;
        private IDoor door;
        private IPowerTube powerTube;
        private IOutput output;

        private IButton pwrBtn, timeBtn, startCancelBtn;
        private ILight light;
        private ITimer timer;

        [SetUp]
        public void Setup()
        {
            // Stubs
            pwrBtn = Substitute.For<IButton>();
            timeBtn = Substitute.For<IButton>();
            startCancelBtn = Substitute.For<IButton>();
            light = Substitute.For<ILight>();
            timer = Substitute.For<ITimer>();
            // Integrated
            door = new Door();
            output = Substitute.For<IOutput>();
            display = new Display(output);
            powerTube = new PowerTube(output);
            cookCtrl = new CookController(timer, display, powerTube);
            // Test
            userI = new UserInterface(pwrBtn, timeBtn, startCancelBtn, door, display, light, cookCtrl);
            
            cookCtrl.UI = userI;
        }

        [Test]
        public void OnDoorOpened_stateReady()
        {
            // State is READY
            door.Open();

            // Only light should turnOn when READY and door open
            light.Received(1).TurnOn();
        }

        [Test]
        public void OnDoorOpened_stateSetPower()
        {
            // Sets state to "SETPOWER"
            userI.OnPowerPressed(this, EventArgs.Empty);

            door.Open();
            light.Received(1).TurnOn();
            output.Received(1).OutputLine($"Display cleared");
        }

        [Test]
        public void OnDoorOpened_stateSetTime()
        {
            // Sets state to "SETTIME"
            userI.OnPowerPressed(this, EventArgs.Empty);
            userI.OnTimePressed(this, EventArgs.Empty);

            // Calls onDoorOpened
            door.Open();
            light.Received(1).TurnOn();
            output.Received(1).OutputLine($"Display cleared");
        }

        [Test]
        public void OnDoorOpened_stateCooking()
        {
            // Sets state to COOKING
            userI.OnPowerPressed(this, EventArgs.Empty);
            userI.OnTimePressed(this, EventArgs.Empty);
            output.Received(1).OutputLine($"Display shows: 01:00");
            
            userI.OnStartCancelPressed(this, EventArgs.Empty);
            output.Received(1).OutputLine($"PowerTube works with 50");

            door.Open();
            light.Received(1).TurnOn(); // Doesnt receive 2 cause already on
            output.Received(1).OutputLine($"Display cleared");
        }
    }
}