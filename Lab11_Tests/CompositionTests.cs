using Xunit;
using Lab11_StudentApp;

namespace Lab11_Tests
{
    public class CompositionTests
    {
        [Fact]
        public void Task1_Engine_IsInitializedCorrectly()
        {
            var engine = new Engine(200, "Дизель");
            Assert.Equal(200, engine.Power);
            Assert.Equal("Дизель", engine.FuelType);
        }

        [Fact]
        public void Task2_Car_InternalComposition_EngineExists()
        {
            var car = new Car("Tesla");
            Assert.Equal("Tesla", car.Model);
            
            // Перевіряємо факт композиції: двигун існує і має валідні характеристики
            Assert.NotNull(car.CarEngine); 
            Assert.True(car.CarEngine.Power > 0, "Двигун повинен мати потужність більше 0");
            Assert.False(string.IsNullOrEmpty(car.CarEngine.FuelType), "Тип пального не має бути порожнім");
        }

        [Fact]
        public void Task3_Car_Aggregation_DriverCanBeNullOrSet()
        {
            var car = new Car("Ford");
            Assert.Null(car.CarDriver); // За замовчуванням водія немає (агрегація)

            var driver = new Driver("Олег", 5);
            car.CarDriver = driver; // Призначаємо водія ззовні

            Assert.NotNull(car.CarDriver);
            Assert.Equal("Олег", car.CarDriver.Name);
        }

        [Fact]
        public void Task4_Car_ArrayComposition_WheelsAreCreated()
        {
            var car = new Car("Mazda");
            
            Assert.NotNull(car.Wheels);
            Assert.Equal(4, car.Wheels.Length); // Має бути рівно 4 колеса
            
            for (int i = 0; i < 4; i++)
            {
                Assert.NotNull(car.Wheels[i]); 
                Assert.True(car.Wheels[i].Radius > 0, "Колесо повинно мати радіус");
            }
        }

        [Fact]
        public void Task5_Car_IsReadyToDrive_ReturnsTrueWhenFullyEquipped()
        {
            var car = new Car("Audi");
            
            // Перевіряємо метод бізнес-логіки студента
            bool isReady = car.IsReadyToDrive();
            
            Assert.True(isReady, "Автомобіль із двигуном та колесами має бути готовим до поїздки");
        }
    }
}