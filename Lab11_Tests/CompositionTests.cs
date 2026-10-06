using Xunit;
using Lab11_StudentApp;

namespace Lab11_Tests
{
    public class CompositionTests
    {
        // =====================================================================
        // Завдання 1. Engine
        // =====================================================================

        [Theory]
        [InlineData(200, "Дизель")]
        [InlineData(75, "Бензин")]
        public void Task1_Engine_InitializesProperties(int power, string fuelType)
        {
            var engine = new Engine(power, fuelType);

            Assert.True(engine.Power == power,
                $"Engine.Power має дорівнювати переданому параметру ({power}), а зараз {engine.Power}");
            Assert.True(engine.FuelType == fuelType,
                $"Engine.FuelType має дорівнювати переданому параметру (\"{fuelType}\"), а зараз \"{engine.FuelType}\"");
        }

        // =====================================================================
        // Завдання 2. Композиція: Car створює власний Engine
        // =====================================================================

        [Theory]
        [InlineData("Tesla")]
        [InlineData("Toyota")]
        public void Task2_Car_StoresModel(string model)
        {
            var car = new Car(model);

            Assert.True(car.Model == model,
                $"Car.Model має дорівнювати переданій моделі (\"{model}\"), а зараз \"{car.Model}\"");
        }

        [Fact]
        public void Task2_Car_CreatesValidEngineInConstructor()
        {
            var car = new Car("Tesla");

            Assert.True(car.CarEngine != null, "CarEngine має бути створений у конструкторі Car (new Engine(...))");
            Assert.True(car.CarEngine.Power > 0, "Потужність двигуна (Power) має бути більшою за 0");
            Assert.False(string.IsNullOrWhiteSpace(car.CarEngine.FuelType), "Тип пального (FuelType) не має бути порожнім");
        }

        [Fact]
        public void Task2_Engine_IsNotSharedBetweenCars()
        {
            var first = new Car("A");
            var second = new Car("B");

            Assert.False(ReferenceEquals(first.CarEngine, second.CarEngine),
                "Кожен автомобіль має мати власний двигун: не використовуйте static або один спільний об'єкт Engine");
        }

        [Fact]
        public void Task2_Car_HasNoParameterlessConstructor()
        {
            var ctor = typeof(Car).GetConstructor(Type.EmptyTypes);

            Assert.True(ctor == null,
                "Конструктор Car() без параметрів не потрібен: автомобіль не може існувати без двигуна й коліс");
        }

        // =====================================================================
        // Завдання 3. Агрегація: Driver
        // =====================================================================

        [Theory]
        [InlineData("Олег", 5)]
        [InlineData("Ірина", 12)]
        public void Task3_Driver_InitializesProperties(string name, int experience)
        {
            var driver = new Driver(name, experience);

            Assert.True(driver.Name == name, $"Driver.Name має дорівнювати \"{name}\", а зараз \"{driver.Name}\"");
            Assert.True(driver.Experience == experience,
                $"Driver.Experience має дорівнювати {experience}, а зараз {driver.Experience}");
        }

        [Fact]
        public void Task3_Car_HasNoDriverByDefault()
        {
            var car = new Car("Ford");

            Assert.True(car.CarDriver == null,
                "Агрегація: після створення авто водія немає, CarDriver має бути null (не створюйте водія в конструкторі Car)");
        }

        [Fact]
        public void Task3_Driver_CanBeSeatedAndLeave_AndLivesIndependently()
        {
            var driver = new Driver("Олег", 5);
            var car = new Car("Ford");

            car.CarDriver = driver;
            Assert.True(ReferenceEquals(car.CarDriver, driver), "Після призначення CarDriver має вказувати на того самого водія");

            car.CarDriver = null;
            Assert.True(car.CarDriver == null, "Після CarDriver = null водія в авто немає");
            Assert.True(driver.Name == "Олег" && driver.Experience == 5,
                "Об'єкт водія існує незалежно від авто: його дані мають зберегтися");
        }

        [Fact]
        public void Task3_RemoveDriver_ReturnsSeatedDriver_AndClearsCar()
        {
            var driver = new Driver("Олег", 5);
            var car = new Car("Ford");
            car.CarDriver = driver;

            Driver? left = car.RemoveDriver();

            Assert.True(ReferenceEquals(left, driver), "RemoveDriver() має повернути водія, який був в авто (той самий об'єкт)");
            Assert.True(car.CarDriver == null, "Після RemoveDriver() у авто не має бути водія (CarDriver == null)");
            Assert.True(driver.Name == "Олег" && driver.Experience == 5,
                "Водій, який вийшов, нікуди не зникає: його дані мають зберегтися");
        }

        [Fact]
        public void Task3_RemoveDriver_WhenNoDriver_ReturnsNull_AndDoesNotThrow()
        {
            var car = new Car("Ford");

            Driver? left = car.RemoveDriver();

            Assert.True(left == null, "Якщо водія не було, RemoveDriver() має повернути null (і не кидати винятків)");
            Assert.True(car.CarDriver == null, "Авто без водія має лишитися без водія");
        }

        [Fact]
        public void Task3_Driver_CanMoveToAnotherCar()
        {
            var driver = new Driver("Ірина", 12);
            var first = new Car("A");
            var second = new Car("B");
            first.CarDriver = driver;

            second.CarDriver = first.RemoveDriver();

            Assert.True(first.CarDriver == null, "Перше авто лишилося без водія");
            Assert.True(ReferenceEquals(second.CarDriver, driver), "Той самий водій має сидіти у другому авто");
        }

        // =====================================================================
        // Завдання 4. Масив вкладених об'єктів: Wheels
        // =====================================================================

        [Theory]
        [InlineData(16)]
        [InlineData(19)]
        public void Task4_Wheel_InitializesRadius(int radius)
        {
            var wheel = new Wheel(radius);

            Assert.True(wheel.Radius == radius, $"Wheel.Radius має дорівнювати {radius}, а зараз {wheel.Radius}");
        }

        [Fact]
        public void Task4_Car_AllocatesArrayOfFourWheels()
        {
            var car = new Car("Mazda");

            Assert.True(car.Wheels != null, "Масив Wheels має бути створений у конструкторі Car (new Wheel[4])");
            Assert.True(car.Wheels.Length == 4, $"Масив Wheels має мати рівно 4 елементи, а зараз {car.Wheels.Length}");
        }

        [Fact]
        public void Task4_Car_FillsEveryWheelSlot_WithPositiveRadius()
        {
            var car = new Car("Mazda");

            for (int i = 0; i < 4; i++)
            {
                Assert.True(car.Wheels[i] != null,
                    $"Wheels[{i}] дорівнює null: new Wheel[4] лише виділяє пам'ять, колеса треба створити в циклі");
                Assert.True(car.Wheels[i].Radius > 0, $"Радіус колеса Wheels[{i}] має бути більшим за 0");
            }
        }

        [Fact]
        public void Task4_Car_WheelsAreFourDistinctObjects()
        {
            var car = new Car("Mazda");

            int distinct = car.Wheels.Distinct(ReferenceEqualityComparer.Instance).Count();

            Assert.True(distinct == 4,
                $"Потрібно 4 окремих об'єкти Wheel (new Wheel(...) на кожній ітерації циклу), а різних об'єктів лише {distinct}");
        }

        [Fact]
        public void Task4_Wheels_AreNotSharedBetweenCars()
        {
            var first = new Car("A");
            var second = new Car("B");

            Assert.False(ReferenceEquals(first.Wheels, second.Wheels), "Кожне авто має мати власний масив Wheels");
            Assert.False(ReferenceEquals(first.Wheels[0], second.Wheels[0]), "Кожне авто має мати власні об'єкти Wheel");
        }

        // =====================================================================
        // Завдання 5. IsReadyToDrive()
        // =====================================================================

        [Fact]
        public void Task5_FullyEquippedCar_IsReady()
        {
            var car = new Car("Audi");

            Assert.True(car.IsReadyToDrive(), "Автомобіль із двигуном і 4 колесами має бути готовим до поїздки");
        }

        [Fact]
        public void Task5_NoEngine_IsNotReady()
        {
            var car = new Car("Audi");
            car.CarEngine = null!;

            Assert.False(car.IsReadyToDrive(), "Без двигуна авто не готове до поїздки");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Task5_MissingWheel_IsNotReady(int index)
        {
            var car = new Car("Audi");
            car.Wheels[index] = null!;

            Assert.False(car.IsReadyToDrive(), $"Якщо Wheels[{index}] дорівнює null, авто не готове до поїздки");
        }

        [Fact]
        public void Task5_NullWheelsArray_IsNotReady_AndDoesNotThrow()
        {
            var car = new Car("Audi");
            car.Wheels = null!;

            Assert.False(car.IsReadyToDrive(),
                "Якщо масив Wheels дорівнює null, метод має повернути false (без NullReferenceException)");
        }

        [Fact]
        public void Task5_WrongWheelCount_IsNotReady()
        {
            var car = new Car("Audi");

            car.Wheels = new[] { new Wheel(16), new Wheel(16), new Wheel(16) };
            Assert.False(car.IsReadyToDrive(), "Із 3 колесами авто не готове до поїздки");

            car.Wheels = new[] { new Wheel(16), new Wheel(16), new Wheel(16), new Wheel(16), new Wheel(16) };
            Assert.False(car.IsReadyToDrive(), "Із 5 колесами авто не готове: потрібно рівно 4");
        }

        [Fact]
        public void Task5_ArrayOfFourNulls_IsNotReady()
        {
            var car = new Car("Audi");
            car.Wheels = new Wheel[4];

            Assert.False(car.IsReadyToDrive(), "Масив із 4 порожніх комірок (null) не означає 4 колеса");
        }

        // =====================================================================
        // Завдання 6. Main
        // =====================================================================

        [Fact]
        public void Task6_Main_RunsWithoutExceptions_AndPrintsReport()
        {
            string text = RunMain();

            int lines = text
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Length;

            Assert.True(lines >= 7,
                $"Main має вивести звіт за Кроками 1-6 (модель, потужність, готовність, водій до і після посадки, водій вийшов, виняток); зараз виведено лише {lines} непорожніх рядків");
        }

        [Fact]
        public void Task6_Main_CatchesAndReports_NullReferenceException()
        {
            string text = RunMain();

            Assert.True(text.Contains("NullReferenceException"),
                "Крок 6: після try / catch (NullReferenceException) виведіть ex.GetType().Name, щоб було видно, який саме виняток спіймано");
        }

        private static string RunMain()
        {
            var originalOut = Console.Out;
            var originalIn = Console.In;
            var output = new StringWriter();

            try
            {
                Console.SetOut(output);
                Console.SetIn(new StringReader(string.Empty));

                Program.Main(Array.Empty<string>());
            }
            finally
            {
                Console.SetOut(originalOut);
                Console.SetIn(originalIn);
            }

            return output.ToString();
        }
    }
}
