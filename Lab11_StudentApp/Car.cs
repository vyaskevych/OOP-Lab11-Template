using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab11_StudentApp
{
    public class Car
    {
        public string Model { get; set; }
        public Engine CarEngine { get; set; } // Композиція
        public Driver CarDriver { get; set; } // Агрегація
        public Wheel[] Wheels { get; set; }   // Масив об'єктів

        public Car() { }

        public Car(string model)
        {
            // TODO: Завдання 2. 
            // 1. Збережіть назву моделі у властивість Model.
            // 2. Створіть новий об'єкт Engine та запишіть його у властивість CarEngine (композиція).
            
            // TODO: Завдання 4. 
            // 1. Виділіть пам'ять для масиву Wheels на 4 елементи.
            // 2. У циклі створіть 4 об'єкти Wheel і заповніть масив коліс.
            
        }

        // Завдання 5: Метод перевірки готовності автомобіля до поїздки
        public bool IsReadyToDrive()
        {
            // TODO: Реалізуйте логіку: 
            // Поверніть true, якщо двигун і всі 4 колеса існують (не дорівнюють null).
            
            return false;
        }
    }
}