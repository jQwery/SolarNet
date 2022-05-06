using System;

namespace SolaraNet.BusinessLogic.Contracts
{
    /// <summary>
    /// Это вспомогательный класс, необходимый, чтобы проще отлавливать ошибки в try,catch
    /// Типо return something.Ok("какая-то строка, например"); - это норм
    /// return something.Failed("Неверный пароль"); - это плохо
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class OperationResult<TResult>
    {
        #region Variables
        public TResult Result { get; set; }
        public bool Success { get; set; }
        public string[] Errors { get; set; }
        #endregion
        #region Public Functions
        /// <summary>
        /// Успешный результат выполнения
        /// </summary>
        /// <param name="result">Результат</param>
        /// <returns></returns>
        public static OperationResult<TResult> Ok(TResult result)
        {
            return new()
            {
                Success = true,
                Result = result
            };
        }
        /// <summary>
        /// Ошибка выполнения
        /// </summary>
        /// <param name="errors">Список ошибок выполнения</param>
        /// <returns></returns>
        public static OperationResult<TResult> Failed(string[] errors)
        {
            return new()
            {
                Errors = errors,
                Success = false
            };
        }
        /// <summary>
        /// Возвращает список всех ошибок выполнения
        /// </summary>
        /// <returns></returns>
        public string GetErrors()
        {
            return string.Join(Environment.NewLine, Errors);
        }
        #endregion
    }
}
