using System;
using System.Collections.Generic;

namespace SolaraNet.Models
{
    public class CapchaGeneratorSimpleVersion
    {
        private static readonly IList<string> CapchaList = new List<string>();

        /// <summary>
        /// Возврвщает случайную последовательность чисел
        /// </summary>
        /// <returns></returns>
        public static string GetRandomCapcha()
        {
            var seed = (int)DateTime.Now.Ticks;
            Random random = new (seed);
            string randomValue = random.Next(1111, 9999).ToString();
            bool valueIsAlreadyInList = true;
            while (valueIsAlreadyInList) // проверка на наличие рандомного значения в списке
            {
                if (CapchaList.Contains(randomValue))
                {
                    randomValue = random.Next(1111, 9999).ToString();
                }
                else
                {
                    valueIsAlreadyInList = false;
                }
            }
            CapchaList.Add(randomValue); // добавляем случайное значение в список

            return randomValue; // возвращаем капчу
        }

        public static bool RemoveCapchaFromList(string capcha)
        {
            if (CapchaList.Contains(capcha))
            {
                CapchaList.Remove(capcha); // удаляем капчу по ненадобности
                return true; // всё прошло хорошо
            }
            else
            {
                return false; // операция прошла неуспешно
            }
        }
    }
}