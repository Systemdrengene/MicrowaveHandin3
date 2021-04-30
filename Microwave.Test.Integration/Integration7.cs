using System;
using System.IO;
using System.Threading;
using Castle.Core.Internal;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = Microwave.Classes.Boundary.Timer;

/*
    T: UI
    X: CookCtrl, Door, Button, Display, Light, Timer, PowerTube, Output
    S: 
*/

namespace Microwave.Test.Integration
{
    public class IT7_FullIntegration
    {
        private UserInterface userI;

        private CookController CookCtrl;
        private IDisplay display;
        private IDoor door;
        private IPowerTube PT;
        private IOutput output;

        private IButton pwrBtn, timeBtn, startCnlBtn;
        private ILight light;
        private ITimer timer;
        private StringWriter swr;

        [SetUp]
        public void Setup()
        {
            // Integrated
            pwrBtn = new Button();
            timeBtn = new Button();
            startCnlBtn = new Button();
            output = new Output();
            light = new Light(output);
            timer = new Timer();
            door = new Door();
            display = new Display(output);
            PT = new PowerTube(output);
            CookCtrl = new CookController(timer, display, PT);
            // Test
            userI = new UserInterface(pwrBtn, timeBtn, startCnlBtn, door, display, light, CookCtrl);

            CookCtrl.UI = userI;

            // Takes input from output and writes it to a StringWriter which we can test through
            swr = new StringWriter();
            Console.SetOut(swr);

        }


        [Test]
        public void CookDish_HappyScenarioMainUseCase()
        {
            door.Open();
            // Light goes on
            // User places dish and closes door
            door.Close();
            // Light turns off
            pwrBtn.Press(); // 50 W
            pwrBtn.Press(); // 100 W
            pwrBtn.Press(); // 150 W
            timeBtn.Press(); // 01:00
            timeBtn.Press(); // 02:00
            startCnlBtn.Press(); 
            Thread.Sleep(121000); // wait 2 min
            // Light goes on
            // Power tube turns on at desired power level
            // Displays shows and updates the remaining time
            // When timer expires power tube turns off
            // Light goes off
            // Display is blanked
            door.Open();
            // Light goes on
            // User removes food
            door.Close();
            // Light goes off
            Assert.Pass();
        }


        [Test]
        public void CookDish_Extension1MainUseCase()
        {
            door.Open();
            // Light goes on
            // User places dish and closes door
            door.Close();
            // Light turns off
            pwrBtn.Press(); // 50 W
            pwrBtn.Press(); // 100 W
            pwrBtn.Press(); // 150 W
            startCnlBtn.Press();
            // Test that it clears display
            Assert.That(swr.ToString().Contains($"Display cleared"));
            // Test that values are reset
            swr.GetStringBuilder().Clear();
            pwrBtn.Press();
            Assert.That(swr.ToString().Contains($"Display shows: 50 W"));
        }

        [Test]
        public void CookDish_Extension2MainUseCase()
        {
            door.Open();
            // Light goes on
            // User places dish and closes door
            door.Close();
            // Light turns off
            pwrBtn.Press(); // 50 W
            pwrBtn.Press(); // 100 W
            swr.GetStringBuilder().Clear();
            door.Open();
            Assert.That(swr.ToString().Contains($"Display cleared"));
            Assert.That(swr.ToString().Contains($"Light is turned on"));
        }

        [Test]
        public void CookDish_Extension3MainUseCase()
        {
            door.Open();
            // Light goes on
            // User places dish and closes door
            door.Close();
            // Light turns off
            pwrBtn.Press(); // 50 W
            pwrBtn.Press(); // 100 W
            timeBtn.Press(); // 01:00
            startCnlBtn.Press(); // State = Cooking
            // Test that extension 3 is met when user presses startCancelButton during cooking
            swr.GetStringBuilder().Clear();
            startCnlBtn.Press();
            Assert.That(swr.ToString().Contains($"PowerTube turned off"));
            Assert.That(swr.ToString().Contains($"Display cleared"));
            Assert.That(swr.ToString().Contains($"Light is turned off"));
            // Test that values are reset
            swr.GetStringBuilder().Clear();
            pwrBtn.Press();
            Assert.That(swr.ToString().Contains($"Display shows: 50 W"));
        }

        [Test]
        public void CookDish_Extension4MainUseCase()
        {
            door.Open();
            // Light goes on
            // User places dish and closes door
            door.Close();
            // Light turns off
            pwrBtn.Press(); // 50 W
            pwrBtn.Press(); // 100 W
            timeBtn.Press(); // 01:00
            startCnlBtn.Press(); // State = Cooking
            // Test that extension 3 is met when user presses startCancelButton during cooking
            swr.GetStringBuilder().Clear();
            door.Open();
            Assert.That(swr.ToString().Contains($"PowerTube turned off"));
            Assert.That(swr.ToString().Contains($"Display cleared"));
            // Test that values are reset
            door.Close();
            pwrBtn.Press();
            Assert.That(swr.ToString().Contains($"Display shows: 50 W"));
        }

    }
}