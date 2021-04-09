using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace ClassTest
{

    public class Tests
    {
        public ChromeDriver driver;
        public WebDriverWait wait;

        private readonly By emailInputLocator = By.Name("email");
        private readonly By buttonLocator = By.Id("sendMe");
        private readonly By emailResultLocator = By.ClassName("your-email");
        private readonly By resultTextLocator = By.ClassName("result-text");
        private readonly By anotherEmailLinkLocator = By.Id("anotherEmail");
        private readonly By resultTextBlockLocator = By.Id("resultTextBlock");
        private readonly By formErrorLocator = By.ClassName("form-error");
        private readonly By radioBoyLocator = By.Id("boy");
        private readonly By radioGirlLocator = By.Id("girl");
        private readonly string expectedEmail = "test@mail.ru";
        private readonly string unexpectedEmail1 = "test@";
        private readonly string unexpectedEmail2 = "test.ru";

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
        }


        private void InputEmail_Click(string parameter, bool anotherEmailLink)
        {
            switch (parameter)
            {
                case "exp":
                    driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
                    break;
                case "unexp1":
                    driver.FindElement(emailInputLocator).SendKeys(unexpectedEmail1);
                    break;
                case "unexp2":
                    driver.FindElement(emailInputLocator).SendKeys(unexpectedEmail2);
                    break;
                case "empty":
                    driver.FindElement(emailInputLocator).SendKeys("");
                    break;
                default:
                    break;
            }
            driver.FindElement(buttonLocator).Click() ;
            if (anotherEmailLink)
            {
                driver.FindElement(anotherEmailLinkLocator).Click();
            }
        }

        [Test]
        public void ParrotNameSite_FillFormWithEmail_Success()
        {
            InputEmail_Click("exp",false);
            Assert.AreEqual(expectedEmail, driver.FindElement(emailResultLocator).Text, "Введенный email и email в итоговой заявке не совпадают");
        }

        [Test]
        public void ParrotNameSite_Enter_BoyDefault()
        {
            Assert.IsTrue(driver.FindElement(radioBoyLocator).Selected, "У радио кнопок по умолчанию выбран не мальчик");
        }

        [Test]
        public void ParrotNameSite_InputUnexpectedEmailWithWrongDomainName_ShowFormError()
        {
            InputEmail_Click("unexp1", false);
            Assert.AreEqual("Некорректный email", driver.FindElement(formErrorLocator).Text, "Сообщение о валидация адреса не верно");
        }

        [Test]
        public void ParrotNameSite_InputUnexpectedEmailWithWrongUserName_ShowFormError()
        {
            InputEmail_Click("unexp2", false);
            Assert.AreEqual("Некорректный email", driver.FindElement(formErrorLocator).Text, "Сообщение о валидация адреса не верно");
        }

        [Test]
        public void ParrotNameSite_InputEmptyEmail_ShowFormError()
        {
            InputEmail_Click("empty", false);
            Assert.AreEqual("Введите email", driver.FindElement(formErrorLocator).Text, "Сообщение о валидации пустого адреса не верно");
        }

        [Test]
        public void ParrotNameSite_InputEmail_ButtonDisappears()
        {
            InputEmail_Click("exp", false);
            Assert.IsFalse(driver.FindElement(buttonLocator).Displayed, "Кнопка остается видимой");
        }

        [Test]
        public void ParrotNameSite_InputEmail_RevealResultTextBlock()
        {
            InputEmail_Click("exp", false);
            Assert.IsTrue(driver.FindElement(resultTextBlockLocator).Displayed,"Итоговое поле с текстом не отобразилось");
        }

        [Test]
        public void ParrotNameSite_ClickAnotherEmail_EmailInputIsEmpty()
        {
            InputEmail_Click("exp", true); 
            Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "После клика по ссылке поле ввода почты не отчистилось");
        }

        [Test]
        public void ParrotNameSite_ClickAnotherEmail_AnotherEmailLinkDisappears()
        {
            InputEmail_Click("exp", true);
            Assert.IsFalse(driver.FindElement(anotherEmailLinkLocator).Displayed, "Не исчезла ссылка для ввода другого email" );
        }

        [Test]
        public void ParrotNameSite_ClickAnotherEmail_ResultTextBlockDisappears()
        {
            InputEmail_Click("exp", true);
            Assert.AreEqual(0, driver.FindElements(resultTextBlockLocator).Count, "Итоговое поле с текстом продолжает отображаться");
        }

        [Test]
        public void ParrotNameSite_ClickAnotherEmail_ButtonReveals()
        {
            InputEmail_Click("exp", true);
            Assert.IsTrue(driver.FindElement(buttonLocator).Displayed, "Кнопка не отобразилась");
        }

        [Test]
        public void ParrotNameSite_ChooseBoyAndFillEmail_ResultTestWithBoy()
        {
            InputEmail_Click("exp", false);
            Assert.IsTrue(driver.FindElement(resultTextLocator).Text.IndexOf("мальчика") != -1, "Мужской пол не был выбран");
        }

        public void ParrotNameSite_ChooseGirlAndFillEmail_ResultTestWithBoy()
        {
            driver.FindElement(radioGirlLocator).Click();
            InputEmail_Click("exp", false);
            Assert.IsTrue(driver.FindElement(resultTextLocator).Text.IndexOf("девочки") != -1, "Женский пол не был выбран");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
