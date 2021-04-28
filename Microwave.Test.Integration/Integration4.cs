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
        public void OnDoorOpened_stateReady_stateDoorOpen()
        {
            door.Open();
            light.Received(1).TurnOn();


            Assert.Pass();
        }

    }
}