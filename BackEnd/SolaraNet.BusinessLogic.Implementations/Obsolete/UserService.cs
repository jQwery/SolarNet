using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.BusinessLogic.Implementations.Validators;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.Common.Validators;

namespace SolaraNet.BusinessLogic.Implementations
{
    /// <summary>
    /// Сервис для работы с пользователями.Это высокоуровневый модуль, который работает с низкоуровневыми(репозиториями из DAL)
    /// </summary>
    [Obsolete("Этот класс устарел, это было актуально до тех пор, пока мы не перешли на Identity. Просто так удалять жалко, столько кода.")]
    public class UserService : IUserService
    {
        #region Много переменных
        private readonly IUserRepository _userRepository;
        private readonly ISaver _saver;
        private readonly IMapper _mapper;
        #endregion
        /// <summary>
        /// Конструктор, в который параметры будут подставляться автоматически
        /// </summary>
        /// <param name="userRepository"></param>
        public UserService(IUserRepository userRepository, ISaver saver, IMapper mapper)
        {
            _userRepository = userRepository;
            _saver = saver;
            _mapper = mapper;
        }

        public async Task<OperationResult<bool>> AddNewUser(UserDTO userToBeAdded, CancellationToken cancellationToken)
        {
            var validResult = await IsUserDTOValid(userToBeAdded, cancellationToken);
            if (!validResult)
            {
                return OperationResult<bool>.Failed(new[] { "Невалидный юзер DTO" });
            }
            validResult = await _userRepository.CreateUser(_mapper.Map<DBUser>(userToBeAdded), cancellationToken);
            if (!validResult)
            {
                return OperationResult<bool>.Failed(new[] { "Не удалось создать пользователя. Ошибка на стороне БД." });
            }
            await _saver.SaveAllChanges();
            return OperationResult<bool>.Ok(true);
        }
        public async Task<OperationResult<UserDTO>> GetUserById(int id, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetUserWhere(u => u.Id == id.ToString(), cancellationToken);
            if (result == null)
            {
                return OperationResult<UserDTO>.Failed(new[] { "Не удалось получить пользователя. Его просто нет в БД. С репозитория пришёл null" });
            }

            var userToBeReturned = _mapper.Map<UserDTO>(result);
            var nextResult = IsUserDTOValid(userToBeReturned, cancellationToken);
            if (!nextResult.Result)
            {
                return OperationResult<UserDTO>.Failed(new[] { "Почему-то в БД лежал невалидный юзер. Его нельзя выдавать наружу..." });
            }

            return OperationResult<UserDTO>.Ok(userToBeReturned);
        }

        public async Task<OperationResult<UserDTO>> GetUserByEmailAndPassword(string email, string password, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetUserWhere(u => u.Email == email && u.PasswordHash == password,
                cancellationToken);
            if (result == null)
            {
                return OperationResult<UserDTO>.Failed(new[] { "Не удалось получить пользователя. Его просто нет в БД. С репозитория пришёл null" });
            }
            var userToBeReturned = _mapper.Map<UserDTO>(result);
            var nextResult = IsUserDTOValid(userToBeReturned, cancellationToken);
            if (!nextResult.Result)
            {
                return OperationResult<UserDTO>.Failed(new[] { "Почему-то в БД лежал невалидный юзер. Его нельзя выдавать наружу..." });
            }
            return OperationResult<UserDTO>.Ok(userToBeReturned);
        } // тут происходит какой-то копипаст предыдущего метода. Может, их можно как-то порефакторить?


        public async Task<OperationResult<UserDTO>> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetUserWhere(u => u.Email == email, cancellationToken);
            if (result == null)
            {
                return OperationResult<UserDTO>.Failed(new[] { "Не удалось получить пользователя. Его просто нет в БД. С репозитория пришёл null" });
            }
            var userToBeReturned = _mapper.Map<UserDTO>(result);
            var nextResult = IsUserDTOValid(userToBeReturned, cancellationToken);
            if (!nextResult.Result)
            {
                return OperationResult<UserDTO>.Failed(new[] { "Почему-то в БД лежал невалидный юзер. Его нельзя выдавать наружу..." });
            }
            return OperationResult<UserDTO>.Ok(userToBeReturned);
        }



        public async Task<OperationResult<bool>> UpdateUserEmail(int idUser, string email, CancellationToken cancellationToken)
        {
            var result = await _userRepository.UpdateUserEmail(u => u.Id == idUser.ToString(), email, cancellationToken);
            if (!result)
            {
                return OperationResult<bool>.Failed(new[] { $"Не удалось обновить мыло пользователя с id: {idUser}. Ошибка произошла на уровне DAL, при чём там даже Exception вылетел какой-то. Короче, иди туда и разбирайся с методом UpdateUserEmail. Больше сказать нечего." });
            }
            await _saver.SaveAllChanges(); // ну, это я обрабатывать не буду, это уже шизофрения. Хотя... 
            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> UpdateUserPassword(int idUser, string newPassword, CancellationToken cancellationToken)
        {
            var result = await _userRepository.UpdateUserPassword(u => u.Id == idUser.ToString(), newPassword, cancellationToken);
            if (!result)
            {
                return OperationResult<bool>.Failed(new[] { $"Не удалось обновить пароль пользователя с id: {idUser}. Ошибка произошла на уровне DAL, при чём там даже Exception вылетел какой-то. Короче, иди туда и разбирайся с методом UpdateUserEmail. Больше сказать нечего." });
            }
            await _saver.SaveAllChanges();
            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> DeleteUserById(int id, CancellationToken cancellationToken)
        {
            var result = await _userRepository.DeleteUser(id, cancellationToken);
            if (!result)
            {
                return OperationResult<bool>.Failed(new[] { $"Пользователя с id: {id}" });
            }
            await _saver.SaveAllChanges();
            return OperationResult<bool>.Ok(true);
        }
        // только что пришло в голову, что при получении чего-либо мы не проверяем, было ли оно удалено. Надо сделать проверку по статусу
        private async Task<bool> IsUserDTOValid(UserDTO user, CancellationToken cancellationToken)
        {
            UserDTOValidatorFromApi validator = new UserDTOValidatorFromApi();
            if (!(await validator.ValidateAsync(user, cancellationToken)).IsValid && !StringValidator.CheckString(user.Name) && !StringValidator.CheckString(user.Mail))
            {
                return false;
            }

            return true;
        }



    }
}
