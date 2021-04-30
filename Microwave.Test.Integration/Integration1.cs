using System;
using System.Runtime.InteropServices;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.Exceptions;

/*
    T: CookCtrl
    X: Display, PowerTube, Output
    S: Timer
*/

namespace Microwave.Test.Integration
{
    public class Integration1
    {
        private IOutput output;

        private IDisplay display;
        private IPowerTube powerTube;
        private ITimer stubbedTimer;
        private IUserInterface stubbedUI;

        private CookController sut;

        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            stubbedUI = Substitute.For<IUserInterface>();
            stubbedTimer = Substitute.For<ITimer>();

            display = new Display(output);
            powerTube = new PowerTube(output);

            sut = new CookController(stubbedTimer, display, powerTube, stubbedUI);
        }

        // Power skal være 50 - 700
        [TestCase(50, 1)]
        [TestCase(51, 1)]
        [TestCase(400, 1)]
        [TestCase(699, 1)]
        [TestCase(700, 1)]
        public void StartCooking_SetPowerAndTime_PowerTubeIsOn(int power, int time)
        {
            // Arrange

            // Act
            sut.StartCooking(power, time);
            // Assert 
            //output.Received(1).OutputLine($"PowerTube works with {power} %");

            output.Received(1).OutputLine(Arg.Is<string>(str => 
                str.Contains(power.ToString())
            ));
        }

        // Power out of range (Exception)
        [TestCase(1000, 1)]
        [TestCase(701, 1)]
        [TestCase(49, 1)]
        [TestCase(-5, 1)]
        public void StartCooking_SetPowerAndTime_PowerExceptionOOR(int power, int time)
        {
            // Arrange

            // Act
            
            // Assert 
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.StartCooking(power, time));
        }

        // Test Display
        [Test]
        public void Display_ThreeSecondsRemaining_OutputWrite()
        {
            sut.StartCooking(50, 115);
            stubbedTimer.TimeRemaining.Returns(115);
            stubbedTimer.TimerTick += Raise.EventWith(this, EventArgs.Empty);
            output.Received(1).OutputLine($"Display shows: 01:55");
        }

        [Test]
        public void Stop_StopTheCookingWhileOn_CookingIsStopped()
        {
            // Act
            sut.StartCooking(50, 50);
            sut.Stop();
            // Assert
            output.Received(1).OutputLine("PowerTube turned off");
        }

        [Test]
        public void Stop_StopTheCookingWhileOff_CookingIsStopped()
        {
            sut.Stop();

            output.Received(0).OutputLine("PowerTube turned off");
        }

        [Test]
        public void StartStart_StartWhileTurnOn_ThrowsException()
        {
            sut.StartCooking(50,50);
            Assert.Throws<ApplicationException>(() => sut.StartCooking(50, 50));
        }
    }
}