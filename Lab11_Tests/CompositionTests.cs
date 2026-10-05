using Xunit;
using Lab11_StudentApp;

namespace Lab11_Tests
{
    public class CompositionTests
    {
        [Fact]
        public void Task1_Engine_IsInitializedCorrectly()
        {
            var engine = new Engine(150, "Бензин");
            Assert.Equal(150, engine.Power);
            Assert.Equal("Бензин", engine.FuelType);
        }

        [Fact]
        public void Task2_Car_InternalComposition_EngineExists()
        {
            var car = new Car("Toyota");
            Assert.Equal("Toyota", car.Model);
            Assert.NotNull(car.CarEngine);
            Assert.True(car.CarEngine.Power > 0, "Двигун має бути ініціалізований даними");
        }

        [Fact]
        public void Task3_Car_Aggregation_DriverCanBeNullOrSet()
        {
            var car = new Car("Ford");
            Assert.Null(car.CarDriver); // За замовчуванням водія немає

            var driver = new Driver("Олег", 5);
            car.CarDriver = driver; // Призначаємо водія

            Assert.NotNull(car.CarDriver);
            Assert.Equal("Олег", car.CarDriver.Name);
        }

        [Fact]
        public void Task4_Car_ArrayComposition_WheelsAreCreated()
        {
            var car = new Car("Mazda");

            Assert.NotNull(car.Wheels);
            Assert.Equal(4, car.Wheels.Length); // Має бути 4 колеса

            for (int i = 0; i < 4; i++)
            {
                Assert.NotNull(car.Wheels[i]); // Кожне колесо має бути створене
                Assert.True(car.Wheels[i].Radius > 0);
            }
        }
    }
}