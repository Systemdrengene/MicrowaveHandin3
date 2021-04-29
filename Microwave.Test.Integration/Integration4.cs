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

        private CookController CookCtrl;
        private IDisplay display;
        private IDoor door;
        private IPowerTube PT;
        private IOutput output;

        private IButton pwrBtn, timBtn, strtCnlBtn;
        private ILight light;
        private ITimer timer;

        [SetUp]
        public void Setup()
        {
            // Stubs
            pwrBtn = Substitute.For<IButton>();
            timBtn = Substitute.For<IButton>();
            strtCnlBtn = Substitute.For<IButton>();
            light = Substitute.For<ILight>();
            timer = Substitute.For<ITimer>();
            // Integrated
            door = new Door();
            output = Substitute.For<IOutput>();
            display = new Display(output);
            PT = new PowerTube(output);
            CookCtrl = new CookController(timer, display, PT);
            // Test
            userI = new UserInterface(pwrBtn, timBtn, strtCnlBtn, door, display, light, CookCtrl);
            
            CookCtrl.UI = userI;
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