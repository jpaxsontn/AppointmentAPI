using System;
using AppointmentDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppointmentTests
{
    [TestClass]
    public class CodingChallengeTests
    {
        [TestMethod]
        public void LetterCount_Should_Return_Word_With_Most_Repeated_Characters_Given_A_Word_With_More_Than_1_Repeating_Character()
        {
            //Arrange
            var letterCountDomain = new LetterCountDomain();

            //Act
            var highestScoreWord = letterCountDomain.LetterCount("Today, is the greatest day ever!");

            //Assert
            Assert.IsTrue(highestScoreWord == "greatest");
        }

        [TestMethod]
        public void LetterCount_Should_Return_Negetive_1_When_There_Are_No_Repeated_Characters_In_Any_Words()
        {
            //Arrange
            var letterCountDomain = new LetterCountDomain();

            //Act
            var highestScoreWord = letterCountDomain.LetterCount("Have A Great Day!");

            //Assert
            Assert.IsTrue(highestScoreWord == "-1");
        }

        [TestMethod]
        public void LetterCount_Should_Return_Negetive_1_When_There_Is_An_Empty_String()
        {
            //Arrange
            var letterCountDomain = new LetterCountDomain();

            //Act
            var highestScoreWord = letterCountDomain.LetterCount("");

            //Assert
            Assert.IsTrue(highestScoreWord == "-1");
        }
    }
}
