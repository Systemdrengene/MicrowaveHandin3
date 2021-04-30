using System;
using System.IO;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

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

            Assert.Pass();
        }

        [Test]
        public void CookDish_Extension2MainUseCase()
        {

            Assert.Pass();
        }

        [Test]
        public void CookDish_Extension3MainUseCase()
        {

            Assert.Pass();
        }

        [Test]
        public void CookDish_Extension4MainUseCase()
        {

            Assert.Pass();
        }

    }
}